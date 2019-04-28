using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using DevExpress.Xpo;
using DevExpress.Xpo.DB;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Xpo.RestDataStore;

namespace WebApiDemo.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DataStoreController : ControllerBase
    {
        private IDataStore _DataStore;

        public DataStoreController(IDataStore DataStore)
        {
            _DataStore = DataStore;
        }

        [HttpPost]
        [Route("[action]")]
        public Byte[] SelectData()
        {
            try
            {
                byte[] Bytes = null;
                Task.Run(async () =>
                {
                    Bytes = await Request.GetRawBodyBytesAsync();
                }).Wait();

                SelectedData SelectedData = _DataStore.SelectData(RestApiDataStore.GetObjectsFromByteArray<SelectStatement[]>(Bytes));
                return RestApiDataStore.ToByteArray(SelectedData);
            }
            catch (Exception exception)
            {
                Debug.WriteLine(string.Format("{0}:{1}", "exception.Message", exception.Message));
                if (exception.InnerException != null)
                {
                    Debug.WriteLine(string.Format("{0}:{1}", "exception.InnerException.Message", exception.InnerException.Message));
                }
                Debug.WriteLine(string.Format("{0}:{1}", " exception.StackTrace", exception.StackTrace));
            }
            return null;
        }

        [HttpPost]
        [Route("[action]")]
        public byte[] ModifyData()
        {
            try
            {
                try
                {
                    byte[] Bytes = null;
                    Task.Run(async () =>
                    {
                        Bytes = await Request.GetRawBodyBytesAsync();
                    }).Wait();

                    var Result = _DataStore.ModifyData(RestApiDataStore.GetObjectsFromByteArray<ModificationStatement[]>(Bytes));
                    return RestApiDataStore.ToByteArray(Result);
                }
                catch (Exception exception)
                {
                    Debug.WriteLine(string.Format("{0}:{1}", "exception.Message", exception.Message));
                    if (exception.InnerException != null)
                    {
                        Debug.WriteLine(string.Format("{0}:{1}", "exception.InnerException.Message", exception.InnerException.Message));
                    }
                    Debug.WriteLine(string.Format("{0}:{1}", " exception.StackTrace", exception.StackTrace));
                }
                return null;
            }
            catch (Exception exception)
            {
                Debug.WriteLine(string.Format("{0}:{1}", "exception.Message", exception.Message));
                if (exception.InnerException != null)
                {
                    Debug.WriteLine(string.Format("{0}:{1}", "exception.InnerException.Message", exception.InnerException.Message));
                }
                Debug.WriteLine(string.Format("{0}:{1}", " exception.StackTrace", exception.StackTrace));
            }
            return null;
        }

        [HttpPost]
        [Route("[action]")]
        public UpdateSchemaResult UpdateSchema()
        {
            //[FromHeader]bool dontCreateIfFirstTableNotExist, [FromBody]  DBTable[] tables
            UpdateSchemaResult Resut = new UpdateSchemaResult();
            try
            {
                byte[] Bytes = null;
                Task.Run(async () =>
                {
                    Bytes = await Request.GetRawBodyBytesAsync();
                }).Wait();
                var Parameters = RestApiDataStore.GetObjectsFromByteArray<MyClass>(Bytes);
                Resut = _DataStore.UpdateSchema(Parameters.dontCreateIfFirstTableNotExist, Parameters.tables);
            }
            catch (Exception exception)
            {
                Debug.WriteLine(string.Format("{0}:{1}", "exception.Message", exception.Message));
                if (exception.InnerException != null)
                {
                    Debug.WriteLine(string.Format("{0}:{1}", "exception.InnerException.Message", exception.InnerException.Message));
                }
                Debug.WriteLine(string.Format("{0}:{1}", " exception.StackTrace", exception.StackTrace));
            }
            return Resut;
        }
    }
}