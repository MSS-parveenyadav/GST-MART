<%@ Page Title="" Language="C#" MasterPageFile="~/ReportViewers/Reports.Master" AutoEventWireup="true" CodeBehind="5A-Total_Value_Of_Stadard_Rated_Supply1.aspx.cs" Inherits="Gst_RPT.ReportViewers.WebForm1" %>

<%@ Register Assembly="Microsoft.ReportViewer.WebForms, Version=11.0.0.0, Culture=neutral, PublicKeyToken=89845dcd8080cc91" Namespace="Microsoft.Reporting.WebForms" TagPrefix="rsweb" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="CPH_For_InputControls" runat="server">
    <div class="tab-content">
      <div class="tab-pane active" id="home">
        <div class=" col-lg-12 sec-login" >
          <h1>Search <span class="glyphicon glyphicon-question-sign"></span></h1>
        </div>
        <form  class="serch">
          <div class="col-md-3">
            <label>Invioce Number :</label>
              <asp:TextBox ID="TxtInvioceNumber" runat="server" placeholder="Invioce Number :"></asp:TextBox>
          </div>
          <div class="col-md-3">
            <label>Customer Name :</label>
              <asp:DropDownList ID="ddlCustomername" runat="server"></asp:DropDownList>
          </div>
          <div class="col-md-3">
            <label>Start Date :</label>
              <asp:TextBox ID="TxtStartDate" runat="server" placeholder="Start Date :"></asp:TextBox>
          </div>
          <div class="col-md-3">
            <label>End Date :</label>
              <asp:TextBox ID="TxtEndDate" runat="server" placeholder="End Date :"></asp:TextBox>
          </div>
          <div class=" button">
              <asp:Button ID="BtnViewReport" runat="server" Text="View Report" OnClick="BtnViewReport_Click" />
            </div>
        </form>
      </div>
      <div class="tab-pane" id="profile">
        <div class=" col-lg-12 sec-login" >
          <h1>Security Code Access <span class="glyphicon glyphicon-question-sign"></span></h1>
        </div>
        empty </div>
    </div>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="CPH_For_RptLable" runat="server">
    5a – Total Value of Standard Rated Supply
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="CPH_For_ForReport" runat="server">
    
    <rsweb:reportviewer id="RptViewer" runat="server" Height="800px" Width="100%"></rsweb:reportviewer>
</asp:Content>
