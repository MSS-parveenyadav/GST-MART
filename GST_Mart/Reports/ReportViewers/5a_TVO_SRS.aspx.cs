using System;
using System.Collections.Generic;
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
    public partial class WebForm4 : System.Web.UI.Page
    {
        string query = @"SELECT Customer_Name ,Customer_BRN ,
                            Invoice_Date ,Invoice_No ,
                            Line_No ,Product_Description ,
                            Sales_Value_MYR ,Tax_Code ,
                            Foreign_Currency_Code ,Sales_Foreign_Currency_Amount 
                            FROM GAF_SUPPLY WHERE Tax_Code in (SELECT Taxcode FROM dbo.ufnGetTaxCodeByItem('5A'))";
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Page.IsPostBack == false)
            {
                Bind_CustomerDropdown();
                BindReport(query);
            }

        }

        public void Bind_CustomerDropdown()
        {
            SqlDataAdapter adp = new SqlDataAdapter("select DISTINCT Customer_Name from [GAF_SUPPLY]", ConfigurationManager.ConnectionStrings["GSTReportConnectionString"].ConnectionString);
            DataSet ds = new DataSet();
            adp.Fill(ds);
            ddlCustomerName.DataTextField = "Customer_Name";
            ddlCustomerName.DataValueField = "Customer_Name";
            ddlCustomerName.DataSource = ds;
            ddlCustomerName.DataBind();
            ddlCustomerName.Items.Insert(0, "Please Select");
        }

        private void BindReport(string query)
        {
            string StartDate = "-";
            string EndDate = "-";
            if (txtstartdatepicker.Text != "" && txtenddatepicker.Text == "")
            {
                query = query + " and (Invoice_Date > @dtFrom)";
                StartDate = Convert.ToDateTime(txtstartdatepicker.Text).ToString("dd MMM yyyy");
            }
            if (txtenddatepicker.Text != "" && txtstartdatepicker.Text == "")
            {
                EndDate = Convert.ToDateTime(txtenddatepicker.Text).ToString("dd MMM yyyy");
                query = query + " and (Invoice_Date < @dtTo)";

            }
            if (txtstartdatepicker.Text != "" && txtenddatepicker.Text != "")
            {

                StartDate = Convert.ToDateTime(txtstartdatepicker.Text).ToString("dd MMM yyyy");
                EndDate = Convert.ToDateTime(txtenddatepicker.Text).ToString("dd MMM yyyy");

                query = query + " and (Invoice_Date BETWEEN @dtFrom AND @dtTo)";

            }

            ReportParameter rpdtfrom = new ReportParameter("dtFrom", StartDate);
            ReportParameter rpdtto = new ReportParameter("dtTo", EndDate);
            SqlDataAdapter adp = new SqlDataAdapter(query, ConfigurationManager.ConnectionStrings["GSTReportConnectionString"].ConnectionString);
            DataSet ds = new DataSet();
            adp.SelectCommand.Parameters.AddWithValue("@dtFrom", txtstartdatepicker.Text);
            adp.SelectCommand.Parameters.AddWithValue("@dtTo", txtenddatepicker.Text);
            adp.Fill(ds);
            if (ds.Tables[0].Rows.Count > 0)
            {
            rptViewer.ProcessingMode = ProcessingMode.Local;
            rptViewer.LocalReport.ReportPath = Server.MapPath("~/Reports/Reports/rpt5A_TVO_SRA.rdlc");
            ReportDataSource datasource = new ReportDataSource("field5a_ds", ds.Tables[0]);
            rptViewer.LocalReport.DataSources.Clear();
            rptViewer.LocalReport.DataSources.Add(datasource);
            rptViewer.LocalReport.SetParameters(new ReportParameter[] { rpdtfrom, rpdtto });
            rptViewer.Visible = true;
            LblErrorMessage.Text = "";
            }
            else
            {
                rptViewer.Visible = false;

                LblErrorMessage.Text = "No records found.";
            }
        }


        protected void BtnViewReport_Click1(object sender, EventArgs e)
        {
            if (txtInvoicenumber.Text != "")
            {
                query = query + " and Invoice_No like" + "'%" + txtInvoicenumber.Text + "%'";

            }

            if (ddlCustomerName.SelectedItem.Text != "Please Select")
            {
                query = query + " and Customer_Name=" + "'" + ddlCustomerName.SelectedItem.Text + "'";

            }
            if (txtstartdatepicker.Text != "" && txtenddatepicker.Text == "")
            {
                query = query + " and (Invoice_Date > @dtFrom)";

            }
            if (txtenddatepicker.Text != "" && txtstartdatepicker.Text == "")
            {
                query = query + " and (Invoice_Date < @dtTo)";

            }
            if (txtstartdatepicker.Text != "" && txtenddatepicker.Text != "")
            {

                query = query + " and (Invoice_Date BETWEEN @dtFrom AND @dtTo)";

            }

            BindReport(query);
        }
    }
}