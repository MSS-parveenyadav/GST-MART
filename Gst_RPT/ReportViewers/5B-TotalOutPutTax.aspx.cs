using Microsoft.Reporting.WebForms;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Gst_RPT.ReportViewers
{
    public partial class _5B_TotalOutputTax : System.Web.UI.Page
    {
        string query = @"SELECT Customer_Name, Customer_BRN, Invoice_Date, Invoice_No, Line_No, Product_Description, Sales_Value_GST_Amount, Tax_Code, Foreign_Currency_Code, 
                         Sales_Currency_Sales_Amount_GST FROM GAF_SUPPLY WHERE (Tax_Code IN (SELECT TaxCode FROM dbo.ufnGetTaxCodeByItem('5B') AS ufnGetTaxCodeByItem_1))";
        protected void Page_Load(object sender, EventArgs e)
        {
            if(Page.IsPostBack==false)
            {

                Bind_CustomerDropdown();
                BindReport(query);
            }

        }

        public void Bind_CustomerDropdown()
        {
            SqlDataAdapter adp = new SqlDataAdapter("SELECT distinct Customer_Name FROM GAF_SUPPLY WHERE (Tax_Code IN (SELECT TaxCode FROM dbo.ufnGetTaxCodeByItem('5B') AS ufnGetTaxCodeByItem_1))", ConfigurationManager.ConnectionStrings["GSTReportConnectionString"].ConnectionString);
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
           
            
            ReportParameter rpdtfrom = new ReportParameter("dtFrom", "01-01-2016");
            ReportParameter rpdtto = new ReportParameter("dtTo", "01-02-2016");
            SqlDataAdapter adp = new SqlDataAdapter(query, ConfigurationManager.ConnectionStrings["GSTReportConnectionString"].ConnectionString);
            DataSet ds = new DataSet();
            adp.SelectCommand.Parameters.AddWithValue("@dtFrom", txtstartdatepicker.Text);
            adp.SelectCommand.Parameters.AddWithValue("@dtTo", txtenddatepicker.Text);
            adp.Fill(ds);
            if(ds.Tables[0].Rows.Count>0)
            {
                rpvTotaloutputtax.Visible = true;
                lblerror.Text = "";

            rpvTotaloutputtax.ProcessingMode = ProcessingMode.Local;
            rpvTotaloutputtax.LocalReport.ReportPath = Server.MapPath("~/Reports/rpt5B-TotalOutputTax.rdlc");
            ReportDataSource datasource = new ReportDataSource("field5b_ds", ds.Tables[0]);
            rpvTotaloutputtax.LocalReport.DataSources.Clear();
            rpvTotaloutputtax.LocalReport.DataSources.Add(datasource);
            rpvTotaloutputtax.LocalReport.SetParameters(new ReportParameter[] { rpdtfrom, rpdtto });

            }
            else
            {
                rpvTotaloutputtax.Visible = false;

                lblerror.Text = "No records found.";
            }
            
        } 

        protected void BtnViewReport_Click(object sender, EventArgs e)
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