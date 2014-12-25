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


namespace Gst_RPT
{
    public partial class AnnualAdjustment : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        protected void BtnViewReport_Click(object sender, EventArgs e)
        {
            try
            {
                int year = Convert.ToInt32(TxtYear.Text);
                int CycleId = Convert.ToInt32(TxtCycleId.Text);
                string CompanyCode = TxtCompanyCode.Text;

                ReportParameter Year = new ReportParameter("Year", year.ToString());
                ReportParameter Cycle_Id = new ReportParameter("Cycle_Id", CycleId.ToString());
                ReportParameter Company_Code = new ReportParameter("Company_Code", CompanyCode);

                string query1 = "select Invoice_Date,Invoice_No,Product_Description,Value_Exempt_Supplies, convert(money,(Value_Exempt_Supplies * 0.06),2) as GST, (select convert(money,amt) as amt from IRR_By_Cycle where [Year]=P.YR and [Month]=P.MTH and Tax_Code='IRR') as IRR,round((convert(money,(Value_Exempt_Supplies * 0.06),2)) * (select (convert(money,amt) / 100)from IRR_By_Cycle where [Year]=P.YR and [Month]=P.MTH and Tax_Code='IRR'),2) as [ITC_Claimed],Tax_Code from GAF_PURCHASE P where Value_Exempt_Supplies>0 and Cycle_ID='" + CycleId + "' and Company_Code='" + CompanyCode + "' and YR='" + year + "' order by Invoice_Date,Invoice_No";
                query1 = query1 + ";";
                query1 = query1 + "select 'TXN43' as 'Type',convert(money,(SUM(convert(decimal(11,2),amt)) * 0.06)) as [Amt] from IRR_By_Cycle where Mth_Yr in (select Mth_Yr from IRR_By_Cycle where tax_group='DMR Status' and amt='qualify') and tax_code='TX-RE' and Cycle_ID='" + CycleId + "' and Company_Code='" + CompanyCode + "' and [Year]='" + year + "'";
                query1 = query1 + ";";
                query1 = query1 + "select  'IRR' as 'Type',convert(money,(select convert(money,(SUM(convert(decimal(11,2),amt)))) from IRR_By_Cycle where tax_code in ('SR','ZRL','ZRE','DS','OS','RS','GS') and Cycle_ID='" + CycleId + "' and Company_Code='" + CompanyCode + "' and [Year]='" + year + "')/	(select convert(money,(SUM(convert(decimal(11,2),amt))))from IRR_By_Cycle where tax_code in ('SR','ZRL','ZRE','DS','OS','RS','GS','ES') and Cycle_ID='" + CycleId + "' and Company_Code='" + CompanyCode + "' and [Year]='" + year + "') * 100 )  as Amt";
                SqlDataAdapter adp = new SqlDataAdapter(query1, ConfigurationManager.ConnectionStrings["GSTReportConnectionString"].ConnectionString);
                DataSet ds = new DataSet();
                adp.Fill(ds);

                if (ds.Tables[0].Rows.Count > 0)
                {
                rptViewer.ProcessingMode = ProcessingMode.Local;
                rptViewer.LocalReport.ReportPath = Server.MapPath("~/Reports/Reports/rptAnnualAdjustment.rdlc");
                ReportDataSource datasource = new ReportDataSource("DataSet1", ds.Tables[0]);
                ReportDataSource datasource1 = new ReportDataSource("DataSet2", ds.Tables[1]);
                ReportDataSource datasource2 = new ReportDataSource("DataSet3", ds.Tables[2]);
                rptViewer.LocalReport.DataSources.Clear();
                rptViewer.LocalReport.DataSources.Add(datasource);
                rptViewer.LocalReport.DataSources.Add(datasource1);
                rptViewer.LocalReport.DataSources.Add(datasource2);
                rptViewer.LocalReport.SetParameters(new ReportParameter[] { Year, Cycle_Id, Company_Code });
                rptViewer.Visible = true;
                LblErrorMessage.Text = "";
                }
                else
                {
                    rptViewer.Visible = false;

                    LblErrorMessage.Text = "No records found.";
                }
            }
            catch(Exception Ex)
            {
                LblErrorMessage.Text = Ex.ToString();
            }
        }
    }
}