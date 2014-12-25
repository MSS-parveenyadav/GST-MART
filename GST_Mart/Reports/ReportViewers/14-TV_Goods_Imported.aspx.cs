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
    public partial class _14_TV_Goods_Imported : System.Web.UI.Page
    {
        string query = @"SELECT Supplier_Name,Supplier_BRN,Invoice_Date ,Invoice_No ,Line_No ,Product_Description ,Purchase_Value_MYR , Tax_Code,
                         Foreign_Currency_Code,Purchase_Foreign_Currency_Amount FROM GAF_PURCHASE WHERE Tax_Code in (SELECT Taxcode FROM dbo.ufnGetTaxCodeByItem('14'))";
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Page.IsPostBack == false)
            {
                Bind_SupplierDropdown();
                Bind_Report(query);
            }
        }
        public void Bind_SupplierDropdown()
        {
            SqlDataAdapter adp = new SqlDataAdapter("SELECT  distinct Supplier_Name FROM GAF_PURCHASE WHERE Tax_Code in (SELECT Taxcode FROM dbo.ufnGetTaxCodeByItem('14'))", ConfigurationManager.ConnectionStrings["GSTReportConnectionString"].ConnectionString);
            DataSet ds = new DataSet();
            adp.Fill(ds);
            ddlSuppliername.DataTextField = "Supplier_name";
            ddlSuppliername.DataValueField = "Supplier_name";
            ddlSuppliername.DataSource = ds;
            ddlSuppliername.DataBind();
            ddlSuppliername.Items.Insert(0, "Please Select");
        }
        private void Bind_Report(string query)
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
                rpvGoodimported.Visible = true;
                lblerror.Text = "";
                rpvGoodimported.ProcessingMode = ProcessingMode.Local;
                rpvGoodimported.LocalReport.ReportPath = Server.MapPath("~/Reports/Reports/rpt14-TVGoodsImported.rdlc");
                ReportDataSource datasource = new ReportDataSource("field14_ds", ds.Tables[0]);
                rpvGoodimported.LocalReport.DataSources.Clear();
                rpvGoodimported.LocalReport.DataSources.Add(datasource);
                rpvGoodimported.LocalReport.SetParameters(new ReportParameter[] { rpdtfrom, rpdtto });
                rpvGoodimported.Visible = true;
                lblerror.Text = "";
            }
            else
            {
                rpvGoodimported.Visible = false;

                lblerror.Text = "No records found.";
            }
        }

        protected void BtnViewReport_Click(object sender, EventArgs e)
        {
            if (txtInvoicenumber.Text != "")
            {
                query = query + " and Invoice_No like" + "'%" + txtInvoicenumber.Text + "%'";

            }

            if (ddlSuppliername.SelectedItem.Text != "Please Select")
            {
                query = query + " and Supplier_Name=" + "'" + ddlSuppliername.SelectedItem.Text + "'";

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

            Bind_Report(query);        
        }
    }
}