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
    public partial class _8_GST_Amt_Claim : System.Web.UI.Page
    {
        string SuplierQuery = @"SELECT Supplier_Name,Supplier_BRN,Invoice_Date,Invoice_No,Line_No,Product_Description,Tax_Code,Foreign_Currency_Code, 
                         Purchase_Value_GST_Amount,Purchase_Currency_Purchase_Amount_GST FROM  GAF_PURCHASE WHERE 
                         (Tax_Code IN (SELECT TaxCode FROM dbo.ufnGetTaxCodeByItem('6B') AS ufnGetTaxCodeByItem_1))";

        string CustomerQuery = @"SELECT Supplier_Name,Supplier_BRN,Invoice_Date,Invoice_No,Line_No,Product_Description,Tax_Code,Foreign_Currency_Code, 
                         Purchase_Value_GST_Amount,Purchase_Currency_Purchase_Amount_GST FROM  GAF_PURCHASE WHERE 
                         (Tax_Code IN (SELECT TaxCode FROM dbo.ufnGetTaxCodeByItem('6B') AS ufnGetTaxCodeByItem_1))";

        string MainQuery = "";

        string qry = "SELECT Purchase_Value_GST_Amount FROM GAF_PURCHASE WHERE (Tax_Code IN (SELECT Taxcode FROM dbo.ufnGetTaxCodeByItem('6B')));SELECT Sales_Value_GST_Amount FROM  GAF_SUPPLY WHERE (Tax_Code IN (SELECT TaxCode FROM dbo.ufnGetTaxCodeByItem('5B')))";
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Page.IsPostBack == false)
            {
                Bind_Report(qry);
                Bind_SuplierDropdown();
            }
        }

        private void Bind_Report(string query)
        {
            ReportParameter rpdtfrom = new ReportParameter("dtFrom", "01-01-2016");
            ReportParameter rpdtto = new ReportParameter("dtTo", "01-02-2016");
            SqlDataAdapter adp = new SqlDataAdapter(query, ConfigurationManager.ConnectionStrings["GSTReportConnectionString"].ConnectionString);
            DataSet ds = new DataSet();
            adp.Fill(ds);
            ReportViewer1.ProcessingMode = ProcessingMode.Local;
            ReportViewer1.LocalReport.ReportPath = Server.MapPath("~/Reports/rpt8-GST-Amt-Claim.rdlc");
            ReportDataSource datasource = new ReportDataSource("field6b_ds", ds.Tables[0]);
            ReportDataSource datasource1 = new ReportDataSource("field5b_ds", ds.Tables[1]);
            ReportViewer1.LocalReport.DataSources.Clear();
            ReportViewer1.LocalReport.DataSources.Add(datasource);
            ReportViewer1.LocalReport.DataSources.Add(datasource1);
            this.ReportViewer1.LocalReport.SubreportProcessing += new SubreportProcessingEventHandler(localReport_SubreportProcessing);
            ReportViewer1.LocalReport.SetParameters(new ReportParameter[] { rpdtfrom, rpdtto });           
        }

        public void Bind_SuplierDropdown()
        {
            SqlDataAdapter adp = new SqlDataAdapter("SELECT   distinct   Supplier_Name FROM   GAF_PURCHASE  ", ConfigurationManager.ConnectionStrings["GSTReportConnectionString"].ConnectionString);
            DataSet ds = new DataSet();
            adp.Fill(ds);
            ddlSuplierName.DataTextField = "Supplier_Name";
            ddlSuplierName.DataValueField = "Supplier_Name";
            ddlSuplierName.DataSource = ds;
            ddlSuplierName.DataBind();
            ddlSuplierName.Items.Insert(0, "Please Select");
        }
        void localReport_SubreportProcessing(object sender, SubreportProcessingEventArgs e)
        {

            MainQuery = SuplierQuery + ";" + CustomerQuery;
            ReportParameter rpdtfrom = new ReportParameter("dtFrom", "01-01-2016");
            ReportParameter rpdtto = new ReportParameter("dtTo", "01-02-2016");
            SqlDataAdapter adp = new SqlDataAdapter(MainQuery, ConfigurationManager.ConnectionStrings["GSTReportConnectionString"].ConnectionString);
            DataSet ds = new DataSet();
            adp.SelectCommand.Parameters.AddWithValue("@dtFrom", txtstartdatepicker.Text);
            adp.SelectCommand.Parameters.AddWithValue("@dtTo", txtenddatepicker.Text);
            adp.Fill(ds);
            e.DataSources.Add(new ReportDataSource("field6b_ds", ds.Tables[0]));
            e.DataSources.Add(new ReportDataSource("field5b_ds", ds.Tables[1]));
 
        }
        protected void BtnViewReport_Click1(object sender, EventArgs e)
        {
            if (txtInvoicenumber.Text != "")
            {
                SuplierQuery = SuplierQuery + " and Invoice_No like" + "'%" + txtInvoicenumber.Text + "%'";
                CustomerQuery = CustomerQuery + " and Invoice_No like" + "'%" + txtInvoicenumber.Text + "%'";
            }

            if (ddlSuplierName.SelectedItem.Text != "Please Select")
            {
                SuplierQuery = SuplierQuery + " and Supplier_Name=" + "'" + ddlSuplierName.SelectedItem.Text + "'";

            }
            if (txtstartdatepicker.Text != "" && txtenddatepicker.Text == "")
            {
                SuplierQuery = SuplierQuery + " and (Invoice_Date > @dtFrom)";
                CustomerQuery = CustomerQuery + " and (Invoice_Date > @dtFrom)";
            }
            if (txtenddatepicker.Text != "" && txtstartdatepicker.Text == "")
            {
                SuplierQuery = SuplierQuery + " and (Invoice_Date < @dtTo)";
                CustomerQuery = CustomerQuery + " and (Invoice_Date < @dtTo)";
            }
            if (txtstartdatepicker.Text != "" && txtenddatepicker.Text != "")
            {

                SuplierQuery = SuplierQuery + " and (Invoice_Date BETWEEN @dtFrom AND @dtTo)";
                CustomerQuery = CustomerQuery + " and (Invoice_Date BETWEEN @dtFrom AND @dtTo)";
            }

            Bind_Report(qry);
        }
    }
}