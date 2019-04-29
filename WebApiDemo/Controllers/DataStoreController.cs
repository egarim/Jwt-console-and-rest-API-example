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
    [Authenticate]
    public class DataStoreController : BaseController
    {
        private IDataStore _DataStore;

        public DataStoreController(IDataStore DataStore)
        {
            _DataStore = DataStore;
        }

        [HttpPost]
        [Route("[action]")]

        public async Task<byte[]> SelectData()
        {
            byte[] Bytes = null;

            Bytes = await Request.GetRawBodyBytesAsync();

            SelectedData SelectedData = _DataStore.SelectData(RestApiDataStore.GetObjectsFromByteArray<SelectStatement[]>(Bytes));
            return RestApiDataStore.ToByteArray(SelectedData);
        }

        [HttpPost]
        [Route("[action]")]
        public async Task<byte[]> ModifyData()
        {
            byte[] Bytes = null;
            Bytes = await Request.GetRawBodyBytesAsync();

            var Result = _DataStore.ModifyData(RestApiDataStore.GetObjectsFromByteArray<ModificationStatement[]>(Bytes));
            return RestApiDataStore.ToByteArray(Result);
        }

        [HttpPost]
        [Route("[action]")]
        public async Task<UpdateSchemaResult> UpdateSchema()
        {
            byte[] Bytes = null;
            Bytes = await Request.GetRawBodyBytesAsync();
            var Parameters = RestApiDataStore.GetObjectsFromByteArray<UpdateSchemaParameters>(Bytes);
            return _DataStore.UpdateSchema(Parameters.dontCreateIfFirstTableNotExist, Parameters.tables);
        }
    }
}