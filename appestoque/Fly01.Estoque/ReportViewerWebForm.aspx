﻿<%@ Page Language="C#" AutoEventWireup="True" CodeBehind="ReportViewerWebForm.aspx.cs" Inherits="ReportViewerForMvc.ReportViewerWebForm" %>

<%@ Register Assembly="Microsoft.ReportViewer.WebForms, Version=11.0.0.0, Culture=neutral, PublicKeyToken=89845dcd8080cc91" Namespace="Microsoft.Reporting.WebForms" TagPrefix="rsweb" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
</head>
<script runat="server">
    protected void ReportOnPreRender(object sender, System.EventArgs e)
    {
        Fly01.Estoque.Models.Reports.ReportViewerHelper<object>.DisableUnwantedExportFormat((ReportViewer)sender, "WORD", "WORDOPENXML");
        base.OnPreRender(e);
    }
</script>
<body style="margin: auto 0px; display: block; padding: 0px;">
    <form id="form1" runat="server">
        <div>
            <asp:ScriptManager ID="ScriptManager1" runat="server">
                <Scripts>
                    <asp:ScriptReference Assembly="ReportViewerForMvc" Name="ReportViewerForMvc.Scripts.PostMessage.js" />
                </Scripts>
            </asp:ScriptManager>
            <rsweb:ReportViewer ID="ReportViewer1" OnPreRender="ReportOnPreRender" runat="server"></rsweb:ReportViewer>
        </div>
    </form>
</body>
</html>
