using DevExpress.Xpo;
using DevExpress.Xpo.DB;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Linq;
using System.Threading.Tasks;
using Xpo.RestDataStore;

namespace WebApiDemo.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    //[Authenticate]
    public class AdminController : ControllerBase
    {


        [HttpPost]
        [Route("[action]")]

        public async Task<byte[]> ExecuteQuery()
        {
            try
            {
                byte[] Bytes = null;

                Bytes = await Request.GetRawBodyBytesAsync();

                
                
                var Query=  RestApiDataStore.GetObjectsFromByteArray<string>(Bytes);
                //string myConnString = "User Id=shopcontroller;Password=Scvs!2012;server=192.163.193.68;Database=scaccess;Persist Security Info=True;";
                var connectionString = MySqlConnectionProvider.GetConnectionString("192.163.193.68", "shopcontroller", "Scvs!2012", "scaccess");

                // Create the default DAL which is used by the default Session and newly created objects to access a data store. 
              
                var Dal= XpoDefault.GetDataLayer(connectionString, AutoCreateOption.DatabaseAndSchema);
                
                Session session = new Session(Dal);
                var Result= session.ExecuteQuery(Query);

                return RestApiDataStore.ToByteArray(Result);
            }
            catch (Exception ex)
            {
                var test = ex.Message;
                throw;
            }

        }



    }
}