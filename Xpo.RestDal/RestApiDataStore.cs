using DevExpress.Xpo;
using DevExpress.Xpo.DB;
using Newtonsoft.Json;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.IO.Compression;
using System.Net;
using System.Runtime.Serialization.Json;
using System.Xml;
using System.Xml.Serialization;

namespace Xpo.RestDataStore
{
    public class RestApiDataStore : IDataStore
    {
        private readonly string Url;

        public RestApiDataStore(string Url, AutoCreateOption AutoCreateOption)

        {
            this.Url = Url;
            this.AutoCreateOption = AutoCreateOption;
        }

        public AutoCreateOption AutoCreateOption { get; private set; }

        public ModificationResult ModifyData(params ModificationStatement[] dmlStatements)
        {
            var client = new RestClient(Url);

            var request = new RestRequest(Method.POST);
            request.Resource = "ModifyData";
            request.AddHeader("Postman-Token", "45efa94f-1de9-4b24-bf68-e030a1256c36");
            request.AddHeader("Cache-Control", "no-cache");
            request.AddHeader("Content-Type", "application/octet-stream");

            request.AddParameter("dmlStatements", ToByteArray(dmlStatements), ParameterType.RequestBody);

            IRestResponse response = client.Execute(request);
            var Bytes = JsonConvert.DeserializeObject<Byte[]>(response.Content);
            var SelectData = GetObjectsFromByteArray<ModificationResult>(Bytes);
            return SelectData;
        }

        private static byte[] Compress(byte[] raw)
        {
            using (MemoryStream memory = new MemoryStream())
            {
                using (GZipStream gzip = new GZipStream(memory,
                    CompressionMode.Compress, true))
                {
                    gzip.Write(raw, 0, raw.Length);
                }

                return memory.ToArray();
            }
        }

        public static T GetObjectsFromByteArray<T>(byte[] bytes)
        {
            var Type = typeof(T);//OperationsAssmbly.GetType(ObjectType);
            using (MemoryStream fs = new MemoryStream(bytes))
            using (var gZipStream = new GZipStream(fs, CompressionMode.Decompress))
            {
                using (XmlDictionaryReader reader =
                    XmlDictionaryReader.CreateTextReader(gZipStream, XmlDictionaryReaderQuotas.Max))
                {
                    XmlSerializer serializer = new XmlSerializer(Type);
                    var Statement = (T)Convert.ChangeType(serializer.Deserialize(reader), Type);
                    return Statement;
                }
            }
        }

        public static byte[] ToByteArray<T>(T Data)
        {
            try
            {
                var StatementType = typeof(T);

                var fs = new MemoryStream();
                using (XmlDictionaryWriter writer = XmlDictionaryWriter.CreateTextWriter(fs))
                {
                    XmlSerializer serializer = new XmlSerializer(StatementType);
                    serializer.Serialize(writer, Data);
                }
                if (System.Diagnostics.Debugger.IsAttached)
                {
                    //HACK only for debug how much data are we sending
                    var array = fs.ToArray();
                    Debug.WriteLine(string.Format("{0}:{1} kb", "Length before compression", Convert.ToDecimal(array.Length) / Convert.ToDecimal(1000)));
                    array = Compress(array);
                    Debug.WriteLine(string.Format("{0}:{1} kb", "Length after compression", Convert.ToDecimal(array.Length) / Convert.ToDecimal(1000)));
                    return array;
                }
                return Compress(fs.ToArray());
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

        public SelectedData SelectData(params SelectStatement[] selects)
        {
            var client = new RestClient(Url);

            var request = new RestRequest(Method.POST);
            request.Resource = "SelectData";
            request.AddHeader("Postman-Token", "45efa94f-1de9-4b24-bf68-e030a1256c36");
            request.AddHeader("Cache-Control", "no-cache");
            request.AddHeader("Content-Type", "application/octet-stream");

            request.AddParameter("selects", ToByteArray(selects), ParameterType.RequestBody);

            IRestResponse response = client.Execute(request);
            var Bytes = JsonConvert.DeserializeObject<Byte[]>(response.Content);
            var SelectData = GetObjectsFromByteArray<SelectedData>(Bytes);
            return SelectData;
        }

        public UpdateSchemaResult UpdateSchema(bool dontCreateIfFirstTableNotExist, params DBTable[] tables)
        {
            var client = new RestClient(Url);

            var stringjson = JsonConvert.SerializeObject(tables);

            var request = new RestRequest(Method.POST);
            request.Resource = "UpdateSchema";
            request.AddHeader("Postman-Token", "45efa94f-1de9-4b24-bf68-e030a1256c36");
            request.AddHeader("Cache-Control", "no-cache");
            request.AddHeader("Content-Type", "application/json");

            request.AddParameter("dontCreateIfFirstTableNotExist", dontCreateIfFirstTableNotExist, ParameterType.HttpHeader);
            IRestResponse<UpdateSchemaResult> response = client.Execute<UpdateSchemaResult>(request);

            return response.Data;
        }
    }
}