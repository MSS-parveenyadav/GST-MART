<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="5A-Total_Value_Of_Stadard_Rated_Supply.aspx.cs" Inherits="Gst_RPT.ReportViewers._5A_Total_Value_Of_Stadard_Rated_Supply" %>

<%@ Register assembly="AjaxControlToolkit" namespace="AjaxControlToolkit" tagprefix="asp" %>
<%@ Register assembly="Microsoft.ReportViewer.WebForms, Version=11.0.0.0, Culture=neutral, PublicKeyToken=89845dcd8080cc91" namespace="Microsoft.Reporting.WebForms" tagprefix="rsweb" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <style type="text/css">
        .auto-style1 {
            width: 100%;
        }
        .auto-style4 {
            width: 159px;
        }
        .auto-style5 {
            width: 620px;
        }
        .auto-style6 {
            width: 139px;
        }
        .auto-style7 {
            width: 151px;
        }
        .auto-style8 {
            width: 385px;
        }
        .auto-style9 {
            width: 386px;
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
                <td class="auto-style7">Start Date</td>
                <td class="auto-style4">
                    <asp:TextBox ID="txtstartdatepicker" runat="server"></asp:TextBox>
                    <asp:CalendarExtender ID="txtstartdatepicker_CalendarExtender" runat="server" Enabled="True" TargetControlID="txtstartdatepicker">
                    </asp:CalendarExtender>
                </td>
                <td class="auto-style6">End Date</td>
                <td class="auto-style8">
                    <asp:TextBox ID="txtenddatepicker" runat="server"></asp:TextBox>
                    <asp:CalendarExtender ID="txtenddatepicker_CalendarExtender" runat="server" Enabled="True" TargetControlID="txtenddatepicker">
                    </asp:CalendarExtender>
                &nbsp;</td>
                <td class="auto-style9">
                    Customer Name</td>
                <td class="auto-style5">
                    <asp:DropDownList ID="ddlCustomerName" runat="server">
                    </asp:DropDownList>
                </td>
                <td class="auto-style5">
                    Invoice Number</td>
                <td class="auto-style5">
                    <asp:TextBox ID="txtInvoicenumber" runat="server"></asp:TextBox>
                </td>
                <td>
                    <asp:Button ID="BtnViewReport" runat="server" OnClick="BtnViewReport_Click" Text="View Report" />
                </td>
            </tr>
        </table>
        <br />
        <rsweb:ReportViewer ID="rptViewer" runat="server" Height="800px" Width="100%"></rsweb:ReportViewer>
    
    </div>
    </form>
</body>
</html>
