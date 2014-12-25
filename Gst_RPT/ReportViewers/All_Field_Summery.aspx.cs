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

namespace WebApplication1
{
    public partial class All_Field_Summery : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        protected void btnViewReport_Click(object sender, EventArgs e)
        {
            int year = Convert.ToInt32(TxtYear.Text);
            var query = "SELECT rptID, Field, FieldName, Year,CASE WHEN [Month] = 1 THEN 'JAN' WHEN [Month] = 2 THEN 'FEB' WHEN [Month] = 3 THEN 'MAR' WHEN [Month] = 4 THEN 'APR' WHEN [Month] = 5 THEN 'MAY' WHEN [Month] = 6 THEN 'JUN' WHEN [Month] = 7 THEN 'JUL' WHEN [Month] = 8 THEN 'AUG' WHEN [Month] = 9 THEN 'SEP' WHEN [Month] = 10 THEN 'OCT' WHEN [Month] = 11 THEN 'NOV' WHEN [Month] = 12 THEN 'DEC' END AS Month_W, Month, TotAmt, TotAmtTax, TotAmtCur, TotAmtCurTax, RptValue FROM  SUMMARY_ALL_FIELDS WHERE (Year ='" + year + "')";
            SqlDataAdapter adp = new SqlDataAdapter(query, ConfigurationManager.ConnectionStrings["GSTReportConnectionString"].ConnectionString);
            DataSet ds = new DataSet();
            adp.Fill(ds);

            reportViewer.ProcessingMode = ProcessingMode.Local;
            reportViewer.LocalReport.ReportPath = Server.MapPath("~/Reports/rptAllFieldSummery.rdlc");
            ReportDataSource datasource = new ReportDataSource("DataSet1", ds.Tables[0]);
            reportViewer.LocalReport.DataSources.Clear();
            reportViewer.LocalReport.DataSources.Add(datasource);
        }
    }
}