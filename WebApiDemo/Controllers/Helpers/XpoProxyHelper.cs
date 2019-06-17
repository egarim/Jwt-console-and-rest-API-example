using DevExpress.Xpo;
using DevExpress.Xpo.DB;
using DevExpress.Xpo.Metadata;
using Solution1.Module;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebApiDemo.Controllers.Helpers
{
    public static class XpoProxyHelper
    {

        static System.Reflection.Assembly currentAssembly;
        private readonly static object lockObject = new object();
        private static ConcurrentDictionary<string, IDataLayer> ConcurrentLayerDictionary = new ConcurrentDictionary<string, IDataLayer>();
        static string Server;


        static XpoProxyHelper()
        {
            currentAssembly = typeof(EventClass).Assembly;
        }





        public static Session GetNewSession(string db)
        {
            return new Session(GetDataLayer(db));
        }
        public static UnitOfWork GetUnitOfWork(string db, string server)
        {
            Server = server;
            return new UnitOfWork(GetDataLayer(db));
        }
        private static IDataLayer GetDataLayer(string db)
        {
            IDataLayer dal;
            if (ConcurrentLayerDictionary.TryGetValue(db, out dal)) return dal;
            lock (lockObject)
            {
                return ConcurrentLayerDictionary.GetOrAdd(db, CreateDataLayer);
            }
        }



        private static Func<string, IDataLayer> CreateDataLayer = db =>
        {
            string conn;
            XPDictionary dict;
            IDataStore store;
            IDataLayer dl;



            //conn =
            //System.Configuration.ConfigurationManager.
            //ConnectionStrings[db].ConnectionString;
            conn = MSSqlConnectionProvider.GetConnectionString(Server, db, "*.mk3247", db);
            dict = new ReflectionDictionary();
            store = XpoDefault.GetConnectionProvider(conn, AutoCreateOption.SchemaAlreadyExists);
            dict.GetDataStoreSchema(currentAssembly);

            //dl = new ThreadSafeDataLayer(dict, store);
            dl = new SimpleDataLayer(dict, store);
            return dl;
        };

    }
}
