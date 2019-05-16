﻿using System;
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
    //[Authenticate]
    public class DataStoreController : BaseController
    {
        private IDataStore _DataStore;

        public DataStoreController(IDataStore DataStore)
        {
            _DataStore = DataStore;
        }
        [HttpPost]
        [Route("[action]")]

        public byte[] GetAutoCreateOptions()
        {
            return RestApiDataStore.ToByteArray(_DataStore.AutoCreateOption);

        }
        [HttpPost]
        [Route("[action]")]

        public async Task<byte[]> SelectData()
        {
            try
            {
                byte[] Bytes = null;

                Bytes = await Request.GetRawBodyBytesAsync();

                SelectedData SelectedData = _DataStore.SelectData(RestApiDataStore.GetObjectsFromByteArray<SelectStatement[]>(Bytes));
                return RestApiDataStore.ToByteArray(SelectedData);
            }
            catch (Exception ex)
            {
                var test = ex.Message;
                throw;
            }
         
        }

        [HttpPost]
        [Route("[action]")]
        public async Task<byte[]> ModifyData()
        {

            byte[] Bytes = null;
            try
            {
                Bytes = await Request.GetRawBodyBytesAsync();

                var Result = _DataStore.ModifyData(RestApiDataStore.GetObjectsFromByteArray<ModificationStatement[]>(Bytes));
                return RestApiDataStore.ToByteArray(Result);
            }
            catch (Exception ex)
            {
                var test = ex.Message;
                throw;
            }
           
        }

        [HttpPost]
        [Route("[action]")]
        public async Task<UpdateSchemaResult> UpdateSchema()
        {
            try
            {
                byte[] Bytes = null;
                Bytes = await Request.GetRawBodyBytesAsync();
                var Parameters = RestApiDataStore.GetObjectsFromByteArray<UpdateSchemaParameters>(Bytes);
                UpdateSchemaResult updateSchemaResult = _DataStore.UpdateSchema(Parameters.dontCreateIfFirstTableNotExist, Parameters.tables);
                return updateSchemaResult;
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
            return new UpdateSchemaResult();
           
        }
    }
}