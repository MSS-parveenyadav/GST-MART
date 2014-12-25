using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;
using Microsoft.Reporting.WebForms;

namespace Gst_RPT.ReportViewers
{
    public partial class WebForm1 : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Page.IsPostBack == false)
            {
                Bind_CustomerDropdown();
            }
        }

        public void Bind_CustomerDropdown()
        {
            SqlDataAdapter adp = new SqlDataAdapter("select DISTINCT Customer_Name from [GAF_SUPPLY]", ConfigurationManager.ConnectionStrings["ReportServiceConnectionString"].ConnectionString);
            DataSet ds = new DataSet();
            adp.Fill(ds);
            ddlCustomername.DataTextField = "Customer_Name";
            ddlCustomername.DataValueField = "Customer_Name";
            ddlCustomername.DataSource = ds;
            ddlCustomername.DataBind();
            ddlCustomername.Items.Insert(0, "Please Select");
        }

        //Start Dates  &  End Date
        private void BindReport(string query)
        {
            ReportParameter SDate = new ReportParameter("dtFrom", TxtStartDate.Text);
            ReportParameter EDate = new ReportParameter("dtTo", TxtEndDate.Text);
            DateTime StartDate = Convert.ToDateTime(TxtStartDate.Text);
            DateTime EndDate = Convert.ToDateTime(TxtEndDate.Text);

            string query1 = @"SELECT Customer_Name ,Customer_BRN ,
                            Invoice_Date ,Invoice_No ,
                            Line_No ,Product_Description ,
                            Sales_Value_MYR ,Tax_Code ,
                            Foreign_Currency_Code ,Sales_Foreign_Currency_Amount 
                            FROM GAF_SUPPLY WHERE Tax_Code in (SELECT Taxcode FROM dbo.ufnGetTaxCodeByItem('5A')) AND "+query;
            SqlDataAdapter adp = new SqlDataAdapter(query1, ConfigurationManager.ConnectionStrings["ReportServiceConnectionString"].ConnectionString);
            adp.SelectCommand.Parameters.AddWithValue("@STFrom", StartDate);
            adp.SelectCommand.Parameters.AddWithValue("@EnTo", EndDate);
            adp.SelectCommand.Parameters.AddWithValue("@custname", ddlCustomername.SelectedItem.Text);
            DataSet ds = new DataSet();
            adp.Fill(ds);


            RptViewer.ProcessingMode = ProcessingMode.Local;
            RptViewer.LocalReport.ReportPath = Server.MapPath("~/Reports/rpt5A-Total_Value_Of_Stadard_Rated_Supply.rdlc");
            ReportDataSource datasource = new ReportDataSource("field5a_ds", ds.Tables[0]);
            RptViewer.LocalReport.DataSources.Clear();
            RptViewer.LocalReport.DataSources.Add(datasource);
            RptViewer.LocalReport.SetParameters(new ReportParameter[] { SDate, EDate });
        }

        protected void BtnViewReport_Click(object sender, EventArgs e)
        {

        }

        

        //protected void BtnViewReport_Click(object sender, EventArgs e)
        //{
        //    string StartDate= TxtStartDate.Text;
        //    string EndDate=TxtEndDate.Text;
        //    string CustomerName = ddlCustomername.SelectedItem.Text;
        //    int InvioceNumber = Convert.ToInt32(TxtInvioceNumber.Text);
          
        //    if(StartDate!="" && CustomerName!="")
        //    {
        //        string qry = "Invoice_Date < @STFrom and Customer_Name=@custname";
        //        BindReport(qry);
        //    }

            
        //}
    }
}