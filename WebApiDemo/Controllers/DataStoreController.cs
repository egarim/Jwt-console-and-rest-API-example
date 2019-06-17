using DevExpress.Xpo.DB;
using DevExpress.Xpo.Helpers;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Xpo.RestDataStore;

namespace WebApiDemo.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
   // [Authenticate]
    public class DataStoreController : BaseController
    {
        //private IDataStore _DataStore;
        private Dictionary<string, IDataStore> _DataStores;


        //public DataStoreController(IDataStore DataStore)
        //{
        //    _DataStore = DataStore;
        //}
        public DataStoreController(Dictionary<string, IDataStore> DataStores)
        {
            _DataStores = DataStores;
        }
        [HttpPost]
        [Route("[action]")]

        public byte[] GetAutoCreateOptions()
        {
            // return RestApiDataStore.ToByteArray(_DataStore.AutoCreateOption);
            return RestApiDataStore.ToByteArray(GetDataStore().AutoCreateOption);

        }

        IDataStore GetDataStore()
        {
            var PayLoad = this.GetPayload();
            var DatabaseId = PayLoad["DatabaseId"].ToString();
            var ServerId = PayLoad["ServerId"].ToString();

            //this._DataStores.TryGetValue
            IDataStore RestDataStore;
            bool found = this._DataStores.TryGetValue(DatabaseId, out RestDataStore);
            if (!found)
            {


                SqlConnectionStringBuilder builder = new SqlConnectionStringBuilder();
                builder.DataSource = ServerId;
                builder.UserID = DatabaseId;
                builder.Password = "*.mk3247";
                builder.InitialCatalog = DatabaseId;

                SqlConnection connection = new SqlConnection(builder.ConnectionString);                
                {
                    RestDataStore = DevExpress.Xpo.DB.MSSqlConnectionProvider.CreateProviderFromConnection(connection, AutoCreateOption.SchemaOnly);
                    this._DataStores.Add(DatabaseId, RestDataStore);
                }
              

            }
            return RestDataStore;
        }
        [HttpPost]
        [Route("[action]")]

        public async Task<byte[]> SelectData()
        {

            try
            {
                byte[] Bytes = null;

                Bytes = await Request.GetRawBodyBytesAsync();

                //SelectedData SelectedData = _DataStore.SelectData(RestApiDataStore.GetObjectsFromByteArray<SelectStatement[]>(Bytes));
                SelectedData SelectedData = GetDataStore().SelectData(RestApiDataStore.GetObjectsFromByteArray<SelectStatement[]>(Bytes));
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

                //var Result = _DataStore.ModifyData(RestApiDataStore.GetObjectsFromByteArray<ModificationStatement[]>(Bytes));
                var Result = GetDataStore().ModifyData(RestApiDataStore.GetObjectsFromByteArray<ModificationStatement[]>(Bytes));
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
        public async Task<byte[]> Do()
        {

            byte[] Bytes = null;
            try
            {
                Bytes = await Request.GetRawBodyBytesAsync();

                var Parameters = RestApiDataStore.GetObjectsFromByteArray<DoArgs>(Bytes);
                //var Result = (_DataStore as ICommandChannel).Do(Parameters.Command, Parameters.Parameters);
                var Result = (GetDataStore() as ICommandChannel).Do(Parameters.Command, Parameters.Parameters);
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
                //UpdateSchemaResult updateSchemaResult = _DataStore.UpdateSchema(Parameters.dontCreateIfFirstTableNotExist, Parameters.tables);
                UpdateSchemaResult updateSchemaResult = GetDataStore().UpdateSchema(Parameters.dontCreateIfFirstTableNotExist, Parameters.tables);
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