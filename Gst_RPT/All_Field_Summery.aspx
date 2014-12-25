<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="All_Field_Summery.aspx.cs" Inherits="WebApplication1.All_Field_Summery" %>

<%@ Register assembly="Microsoft.ReportViewer.WebForms, Version=11.0.0.0, Culture=neutral, PublicKeyToken=89845dcd8080cc91" namespace="Microsoft.Reporting.WebForms" tagprefix="rsweb" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <style type="text/css">
        .auto-style1 {
            width: 100%;
        }
        .auto-style2 {
            width: 58px;
        }
        .auto-style3 {
            width: 939px;
        }
    </style>
</head>
<body>
    <form id="form1" runat="server">
    <div>
    
        <asp:ScriptManager ID="ScriptManager1" runat="server">
        </asp:ScriptManager>
        <br />
        <table class="auto-style1">
            <tr>
                <td class="auto-style2">Year</td>
                <td class="auto-style3">
                    <asp:TextBox ID="TxtYear" runat="server"></asp:TextBox>
                </td>
                <td>
                    <asp:Button ID="btnViewReport" runat="server"  Text="ViewReport" OnClick="btnViewReport_Click" />
                </td>
            </tr>
        </table>
    
        <br />
        <rsweb:ReportViewer ID="ReportViewer1" runat="server" Width="100%">
        </rsweb:ReportViewer>
    
    </div>
    </form>
</body>
</html>
