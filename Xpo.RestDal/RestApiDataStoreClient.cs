using DevExpress.Xpo;
using DevExpress.Xpo.Metadata;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;

namespace Xpo.RestDataStore
{
    public class RestApiDataStoreClient
    {
        public string LoginUrl { get; set; }
        public string DataStoreUrl { get; set; }
        public Assembly[] Assemblies { get; set; }

        private XPDictionary dictionary;

        public LoginResult LoginResult { get; private set; }

        private SimpleDataLayer dal;
        public SimpleDataLayer Dal { get => dal; set => dal = value; }

        private RestObjectLayer objectLayer;
        public RestObjectLayer ObjectLayer { get => objectLayer; set => objectLayer = value; }

        public RestApiDataStoreClient(string loginUrl, string dataStoreUrl, params Assembly[] assemblies)
        {
            LoginUrl = loginUrl;
            DataStoreUrl = dataStoreUrl;
            Assemblies = assemblies;
            dictionary = new ReflectionDictionary();
            dictionary.GetDataStoreSchema(Assemblies);
        }

        public void Login(string Username, string Password)
        {
            var client = new RestClient(LoginUrl);

            var request = new RestRequest(Method.POST);
            request.Resource = "Login";

            request.AddHeader("Content-Type", "application/octet-stream");

            request.AddParameter("dmlStatements", RestApiDataStore.ToByteArray(new LoginParameters(Username, Password)), ParameterType.RequestBody);

            IRestResponse<LoginResult> response = client.Execute<LoginResult>(request);
            if (response.IsSuccessful)
            {
                if (response.Data.Authenticated)
                {
                    this.LoginResult = response.Data;
                    dal = new SimpleDataLayer(dictionary, new RestApiDataStore(DataStoreUrl, DevExpress.Xpo.DB.AutoCreateOption.SchemaOnly, this.LoginResult.Token));
                    objectLayer = new RestObjectLayer(dal, Username);
                }
            }
        }

        public UnitOfWork CreateNewUnitOfWork()
        {
            return new UnitOfWork(ObjectLayer);
        }

        public void UpdateSchema()
        {
            if (!this.LoginResult.Authenticated)
            {
                throw new InvalidOperationException("Please login before trying to update schema");
            }
            ThreadSafeDataLayer UpdateSchemaDal = new ThreadSafeDataLayer(dictionary, new RestApiDataStore(DataStoreUrl, DevExpress.Xpo.DB.AutoCreateOption.DatabaseAndSchema, this.LoginResult.Token));

            UnitOfWork UpdateSchemaUoW = new UnitOfWork(UpdateSchemaDal);

            UpdateSchemaUoW.UpdateSchema();
            UpdateSchemaUoW.CreateObjectTypeRecords();
        }
    }
}