﻿Imports System
Imports System.Collections.Generic
Imports System.ComponentModel
Imports System.Data
Imports System.Drawing
Imports System.Linq
Imports System.Text
Imports System.Windows.Forms
Imports DevExpress.XtraReports.UI
Imports DevExpress.XtraBars
Imports DevExpress.XtraPrinting.Preview
Imports DevExpress.XtraPrinting.Preview.Native
Imports DevExpress.XtraPrinting
Imports DevExpress.XtraRichEdit
Imports System.IO
Imports System.Diagnostics
Namespace WindowsFormsApplication1
	Partial Public Class Form1
		Inherits Form

		Public Sub New()
			InitializeComponent()
		End Sub
		Private report As XtraReport1
		Private Sub button1_Click(ByVal sender As Object, ByVal e As EventArgs) Handles button1.Click
			report = New XtraReport1()
			Using rpt As New ReportPrintTool(report)
				report.CreateDocument(False)
				AddHandler rpt.PreviewForm.Shown, AddressOf PreviewForm_Shown
				rpt.ShowPreviewDialog()
			End Using
		End Sub

		Private Sub PreviewForm_Shown(ByVal sender As Object, ByVal e As EventArgs)
			Dim form As PrintPreviewFormEx = DirectCast(sender, PrintPreviewFormEx)
			Dim item As PrintPreviewBarItem = CType(form.PrintBarManager.GetBarItemByCommand(PrintingSystemCommand.ExportFile), PrintPreviewBarItem)
			Dim control As PopupMenu = CType((CType(item, DevExpress.XtraBars.BarButtonItem)).DropDownControl, PopupMenu)
			Dim barItem As New BarButtonItem()
			AddHandler barItem.ItemClick, AddressOf barItem_ItemClick
			barItem.Caption = "DOC File"
			control.AddItem(barItem)

			barItem = New BarButtonItem()
			AddHandler barItem.ItemClick, AddressOf barItem_ItemClick3
			barItem.Caption = "ODT File"
			control.AddItem(barItem)
		End Sub

		Private Sub barItem_ItemClick3(ByVal sender As Object, ByVal e As ItemClickEventArgs)
			ExportToDOC("odt", DocumentFormat.OpenDocument)
		End Sub

		Private Sub barItem_ItemClick(ByVal sender As Object, ByVal e As ItemClickEventArgs)
			ExportToDOC("doc", DocumentFormat.Doc)

		End Sub

		Private Sub ExportToDOC(ByVal extension As String, ByVal df As DocumentFormat)
			Dim sfd As New SaveFileDialog()
			sfd.FileName = Environment.CurrentDirectory & "\" & report.ExportOptions.PrintPreview.DefaultFileName & "." & extension
			sfd.Filter = String.Format("{0} File|*.{0}", extension)
			If sfd.ShowDialog() = System.Windows.Forms.DialogResult.OK Then
				Using docServer As New RichEditDocumentServer()
				Using ms As New MemoryStream()
					report.ExportToRtf(ms, New RtfExportOptions() With {.ExportMode = RtfExportMode.SingleFile})
					ms.Position = 0
					docServer.LoadDocument(ms, DocumentFormat.Rtf)
					docServer.SaveDocument(sfd.FileName, df)
				End Using
				End Using
				If MessageBox.Show("Would you like to open file exported file?", extension & " export", MessageBoxButtons.YesNo) = System.Windows.Forms.DialogResult.Yes Then
					Process.Start(sfd.FileName)
				End If
			End If
		End Sub

	End Class
End Namespace
