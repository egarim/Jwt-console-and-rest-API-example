using DevExpress.ExpressApp;
using DevExpress.ExpressApp.ReportsV2;
using DevExpress.Persistent.Base.ReportsV2;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebApiDemo.Controllers.Helpers
{
    public class MyReportDataSourceHelper : ReportDataSourceHelper
    {
        IObjectSpaceProvider objectSpaceProvider;
        public MyReportDataSourceHelper(IObjectSpaceProvider objectSpaceProvider)
            : base(null)
        {
            this.objectSpaceProvider = objectSpaceProvider;
        }
        protected override IReportObjectSpaceProvider CreateReportObjectSpaceProvider()
        {
            return new MyReportObjectSpaceProvider(objectSpaceProvider);
        }
    }

    public class MyReportObjectSpaceProvider : IReportObjectSpaceProvider, IObjectSpaceCreator
    {
        IObjectSpaceProvider objectSpaceProvider;
        IObjectSpace objectSpace;
        public MyReportObjectSpaceProvider(IObjectSpaceProvider objectSpaceProvider)
        {
            this.objectSpaceProvider = objectSpaceProvider;
        }
        public void DisposeObjectSpaces()
        {
            if (objectSpace != null)
            {
                objectSpace.Dispose();
                objectSpace = null;
            }
        }
        public IObjectSpace GetObjectSpace(Type type)
        {
            if (objectSpace == null)
            {
                objectSpace = objectSpaceProvider.CreateObjectSpace();
            }
            return objectSpace;
        }
        public IObjectSpace CreateObjectSpace(Type type)
        {
            return objectSpaceProvider.CreateObjectSpace();
        }
    }
}
