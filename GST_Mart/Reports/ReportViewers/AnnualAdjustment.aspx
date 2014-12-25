<%@ Page Language="C#" MasterPageFile="~/Reports/ReportViewers/Reports.Master" AutoEventWireup="true" CodeBehind="AnnualAdjustment.aspx.cs" Inherits="Gst_RPT.AnnualAdjustment" %>

<%@ Register assembly="Microsoft.ReportViewer.WebForms, Version=11.0.0.0, Culture=neutral, PublicKeyToken=89845dcd8080cc91" namespace="Microsoft.Reporting.WebForms" tagprefix="rsweb" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
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
            <label>Year :</label>
            <asp:TextBox ID="TxtYear" placeholder="Year :" runat="server"></asp:TextBox>
              <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ErrorMessage="Field Required" ControlToValidate="TxtYear"></asp:RequiredFieldValidator>
          </div>
          <div class="col-md-3">
              <label>Cycle Id :</label>
              <asp:TextBox ID="TxtCycleId" Placeholder="Cycle Id" runat="server"></asp:TextBox>
              <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" ErrorMessage="Field Required" ControlToValidate="TxtCycleId"></asp:RequiredFieldValidator>
          </div>
          <div class="col-md-3">
            <label>Company Code :</label>
              <asp:TextBox ID="TxtCompanyCode" runat="server"></asp:TextBox>
              <asp:RequiredFieldValidator ID="RequiredFieldValidator3" runat="server" ErrorMessage="Field Required" ControlToValidate="TxtCompanyCode"></asp:RequiredFieldValidator>
          </div>
          <div class="  button">
              <asp:Button ID="BtnViewReport" runat="server" OnClick="BtnViewReport_Click" Text="View Report" />
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
    ANNUAL ADJUSTMENT
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="CPH_For_ForReport" runat="server">
    <asp:Label ID="LblErrorMessage" Style="font:14px arial;color:#bf2928" runat="server" Text=""></asp:Label>
    <rsweb:ReportViewer ID="rptViewer" Height="800px" Width="100%" runat="server"></rsweb:ReportViewer>
</asp:Content>

