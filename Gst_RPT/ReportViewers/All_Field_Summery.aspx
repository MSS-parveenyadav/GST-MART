<%@ Page Language="C#" AutoEventWireup="true"  MasterPageFile="~/ReportViewers/Reports.Master" CodeBehind="All_Field_Summery.aspx.cs" Inherits="WebApplication1.All_Field_Summery" %>

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
            <asp:TextBox ID="TxtYear" runat="server"></asp:TextBox>
              <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ErrorMessage="Field Required" ControlToValidate="TxtYear"></asp:RequiredFieldValidator>
          </div>
          <div class="  button">
              <asp:Button ID="btnViewReport" runat="server"  Text="ViewReport" OnClick="btnViewReport_Click" />
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
    All Summery Report
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="CPH_For_ForReport" runat="server">
    <asp:Label ID="LblErrorMessage" runat="server" Style="font:14px arial;color:#bf2928" Text=""></asp:Label>
    <rsweb:ReportViewer ID="reportViewer" runat="server" Height="800px" Width="100%"></rsweb:ReportViewer>
    </asp:Content>
