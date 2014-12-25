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
    public partial class WebForm6 : System.Web.UI.Page
    {
        string query = @"SELECT Supplier_Name , Supplier_BRN , Invoice_Date , Invoice_No , Line_No , 
                         Product_Description , Tax_Code , Foreign_Currency_Code , Purchase_Value_GST_Amount , 
                         Purchase_Currency_Purchase_Amount_GST 
                         FROM GAF_PURCHASE
                         WHERE (Tax_Code IN (SELECT Taxcode FROM dbo.ufnGetTaxCodeByItem('15') )) ";
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Page.IsPostBack == false)
            {
                Bind_SuplierDropdown();
                BindReport(query);
            }

        }

        public void Bind_SuplierDropdown()
        {
            SqlDataAdapter adp = new SqlDataAdapter("SELECT   distinct   Supplier_Name FROM   GAF_PURCHASE WHERE        (Tax_Code IN (SELECT Taxcode FROM dbo.ufnGetTaxCodeByItem('15') )) ", ConfigurationManager.ConnectionStrings["GSTReportConnectionString"].ConnectionString);
            DataSet ds = new DataSet();
            adp.Fill(ds);
            ddlSuplierName.DataTextField = "Supplier_Name";
            ddlSuplierName.DataValueField = "Supplier_Name";
            ddlSuplierName.DataSource = ds;
            ddlSuplierName.DataBind();
            ddlSuplierName.Items.Insert(0, "Please Select");
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
            if (ds.Tables[0].Rows.Count > 0)
            {
            rptViewer.ProcessingMode = ProcessingMode.Local;
            rptViewer.LocalReport.ReportPath = Server.MapPath("~/Reports/rpt15-TVO_ GS.rdlc");
            ReportDataSource datasource = new ReportDataSource("field15_ds", ds.Tables[0]);
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

            if (ddlSuplierName.SelectedItem.Text != "Please Select")
            {
                query = query + " and Supplier_Name=" + "'" + ddlSuplierName.SelectedItem.Text + "'";

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