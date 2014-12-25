<%@ Page Title="" Language="C#" MasterPageFile="~/ReportViewers/Reports.Master" AutoEventWireup="true" CodeBehind="8-GST-Amt_Claim.aspx.cs" Inherits="Gst_RPT.ReportViewers._8_GST_Amt_Claim" %>

<%@ Register Assembly="Microsoft.ReportViewer.WebForms, Version=11.0.0.0, Culture=neutral, PublicKeyToken=89845dcd8080cc91" Namespace="Microsoft.Reporting.WebForms" TagPrefix="rsweb" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
        <script type="text/javascript">
            $(function () {
                $("#CPH_For_InputControls_txtstartdatepicker").datepicker();
            });
            $(function () {
                $("#CPH_For_InputControls_txtenddatepicker").datepicker();
            });
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="CPH_For_InputControls" runat="server">
    <div class="container-fluid">
    <div class="tab-content">
      <div class="tab-pane active" id="home">
        <div class=" col-lg-12 sec-login" >
          <h1>Search <span class="glyphicon glyphicon-question-sign"></span></h1>
        </div>
        <div class="serch">
          <div class="col-md-3">
            <label>Invioce Number :</label>
            <asp:TextBox ID="txtInvoicenumber" placeholder="Invioce Number :" runat="server"></asp:TextBox>
          </div>
          <div class="col-md-3">
              <label>Suplier Name :</label>
              <asp:DropDownList ID="ddlSuplierName" runat="server"></asp:DropDownList>
          </div>
          <div class="col-md-3">
            <label>Start Date :</label>
              <asp:TextBox ID="txtstartdatepicker" runat="server"></asp:TextBox>
          </div>
          <div class="col-md-3">
            <label>End Date :</label>
              <asp:TextBox ID="txtenddatepicker" runat="server"></asp:TextBox>
          </div>
          <div class="  button">
              <asp:Button ID="BtnViewReport" runat="server" Text="View Report" OnClick="BtnViewReport_Click1" />
          </div>
        </div>
      </div>
      <div class="tab-pane" id="profile">
        <div class=" col-lg-12 sec-login" >
          <h1>Security Code Access <span class="glyphicon glyphicon-question-sign"></span></h1>
        </div>
        <div class="security">
          <%--<form action="" method="get" >
            <div class="col-xs-8 security-form">
              <label>Security Code:</label>
              <input type="text" class="form-control" >
            </div>
            <div class=" btn2">
              <input name="" type="button" value="enter">
            </div>
          </form>--%>
          <div class="rest-pass"> <a href="#">Resend Security Code </a> </div>
        </div>
      </div>
    </div>
  </div>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="CPH_For_RptLable" runat="server">
    8-gst-amount-claimable
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="CPH_For_ForReport" runat="server">
    <asp:Label ID="LblErrorMessage" runat="server" Style="font:14px arial;color:#bf2928" Text=""></asp:Label>
    <rsweb:ReportViewer ID="ReportViewer1" Height="800px" Width="100%" runat="server"></rsweb:ReportViewer>
</asp:Content>
