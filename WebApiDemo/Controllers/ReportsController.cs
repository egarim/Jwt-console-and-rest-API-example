using DevExpress.Data.Filtering;
using DevExpress.ExpressApp;
using DevExpress.ExpressApp.DC;
using DevExpress.ExpressApp.DC.Xpo;
using DevExpress.ExpressApp.ReportsV2;
using DevExpress.ExpressApp.Xpo;
using DevExpress.Persistent.Base.ReportsV2;
using DevExpress.Persistent.BaseImpl;
using DevExpress.Xpo.DB;
using DevExpress.XtraReports.UI;
using Microsoft.AspNetCore.Mvc;
using Solution1.Module;
using System;
using System.IO;
using System.Linq;
using WebApiDemo.Controllers.Helpers;


namespace WebApiDemo.Controllers
{
    public class ReportsController : BaseController
    {

       // private IDataStore _DataStore;

        public ReportsController()
        {
           // _DataStore = DataStore;

        }


        [HttpGet]
        public ActionResult<MemoryStream> ExecuteXafReport()
        {

            MemoryStream dataStream = new MemoryStream(PrintReports(".Receipt", string.Empty));
            return dataStream;
        }
        public byte[] PrintReports(string ReportName, string CompanyId)
        {


            using (XPObjectSpaceProvider objectSpaceProvider = CreateObjectSpaceProvider(CompanyId))
            {
                IObjectSpace objectSpace = objectSpaceProvider.CreateObjectSpace();


                //ReportData reportData = objectSpace.FindObject<ReportData>(new BinaryOperator("Name", "Invoice"));
                //ReportData reportData = objectSpace.FindObject<ReportData>(new BinaryOperator("DisplayName", ReportName));

                ReportDataV2 reportData = objectSpace.FindObject<ReportDataV2>(new BinaryOperator("DisplayName", ReportName));
                //ReportDataV2 reportData = objectSpace.GetObjectByKey<ReportDataV2>(Guid.Parse("9D1BD93A-5E61-4193-A8FF-190916AFF298"));
              

                return ExportReport(reportData, objectSpaceProvider, string.Empty, objectSpace);

            }

        }

        private static byte[] ExportReport(ReportDataV2 reportData, IObjectSpaceProvider objectSpaceProvider, string CriteriaString, IObjectSpace objectSpace)
        {


            MyReportDataSourceHelper reportDataSourceHelper = new MyReportDataSourceHelper(objectSpaceProvider);
            ReportDataProvider.ReportObjectSpaceProvider = new MyReportObjectSpaceProvider(objectSpaceProvider);
            CriteriaOperator ReportCriteria = CriteriaOperator.Parse(CriteriaString);


            //XtraReport report = ReportDataProvider.ReportsStorage.LoadReport(reportData);
            XtraReport report = new XtraReport();

            byte[] content = reportData.Content;
            if (content != null && content.Length != 0)
            {
                int num = content.Length;
                while (content[num - 1] == 0)
                {
                    num--;
                }
                MemoryStream memoryStream = new MemoryStream(content, 0, num);

                report.LoadLayoutFromXml(memoryStream);

                // report.LoadLayout(memoryStream);
               

                memoryStream.Close();
            }

            DataSourceBase dataSourceBase = report.DataSource as DataSourceBase;
            if (dataSourceBase != null && reportData is IReportDataV2Writable)
            {
                ((IReportDataV2Writable)reportData).SetDataType(dataSourceBase.DataType);
            }
            report.DisplayName = reportData.DisplayName;

            //XtraReport report = reportData.LoadReport(objectSpace);



            report.Extensions["DataSerializationExtension"] = "XtraReport";
            report.Extensions["DataEditorExtension"] = "XtraReport";
            report.Extensions["ParameterEditorExtension"] = "XtraReport";

            reportDataSourceHelper.SetupBeforePrint(report, null, ReportCriteria, true, null, false);



            MemoryStream ReportStream = new MemoryStream();
            report.ExportToPdf(ReportStream);
            return ReportStream.ToArray();
        }


        // register your application types here
        private static void RegisterBOTypes(ITypesInfo typesInfo)
        {

            typesInfo.RegisterEntity(typeof(ReportDataV2));
            typesInfo.RegisterEntity(typeof(Employee));
            // typesInfo.RegisterEntity(typeof(ReportData));

        }
        private static XPObjectSpaceProvider CreateObjectSpaceProvider(string CompanyId) //TODO use companyID
        {

            XpoTypesInfoHelper.ForceInitialize();
            ITypesInfo typesInfo = XpoTypesInfoHelper.GetTypesInfo();
            XpoTypeInfoSource xpoTypeInfoSource = XpoTypesInfoHelper.GetXpoTypeInfoSource();
            RegisterBOTypes(typesInfo);
            ConnectionStringDataStoreProvider dataStoreProvider = new ConnectionStringDataStoreProvider(MSSqlConnectionProvider.GetConnectionString("weedware101.database.windows.net,1433", "demodbjose", "*.mk3247", "demodbjose"));
            return new XPObjectSpaceProvider(dataStoreProvider, typesInfo, xpoTypeInfoSource);
        }
    }
}
