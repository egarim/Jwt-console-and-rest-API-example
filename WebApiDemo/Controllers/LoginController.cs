using DevExpress.Data.Filtering;
using DevExpress.Xpo;
using Microsoft.AspNetCore.Mvc;
using SimpleJwt;
using Solution1.Module;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using WebApiDemo.Controllers.Helpers;
using Xpo.RestDataStore;


namespace WebApiDemo.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LoginController : ControllerBase
    {
        public const string Key = "JIHHKS304SS1E9SXT2P1OXHN3Y1XHETRVSVB71EPYHR8U6FO2GMKYX4WB3NW0GTZ9BFWORZ1XACHGI26SM5B2J27X89DOSYSY18S0TAKRUWYVXQ2BRDNLR70JI8PRVNE";
        public const string Issuer = "Jose Columbie";

        [HttpPost]
        [Route("[action]")]
        public async Task<LoginResult> Login()
        {
            //List of standard Payload claims https://en.wikipedia.org/wiki/JSON_Web_Token#Standard_fields

            byte[] ParametersByte = await Request.GetRawBodyBytesAsync();

            LoginParameters LoginParameters = RestApiDataStore.GetObjectsFromByteArray<LoginParameters>(ParametersByte);
            string Database;
            string Server;
            Employee User = null;
            try
            {

                string UserName = WebUtility.UrlDecode(LoginParameters.Username);
                string Password = WebUtility.UrlDecode(LoginParameters.Password);
               
                Server = WebUtility.UrlDecode(LoginParameters.Server);

                Database = WebUtility.UrlDecode(LoginParameters.Database);
                UnitOfWork UoW = XpoProxyHelper.GetUnitOfWork(Database, Server);

                User = UoW.FindObject<Employee>(new BinaryOperator("UserName", UserName));
               
              
                if (User == null)
                {
                    return new LoginResult() { Authenticated = false, Token = "" };
                }
                if (!User.ComparePassword(Password))
                {

                    return new LoginResult() { Authenticated = false, Token = "" }; //TODO invalid password

                }

               

            }
            catch (Exception exception)
            {
                return new LoginResult() { Authenticated = false, Token = "", ErrorMessage= exception.Message };
            }
      

             JwtPayload InitialPayload;
                InitialPayload = new JwtPayload
                {
                    { JwtRegisteredClaimNames.NameId, LoginParameters.Username },
                    { JwtRegisteredClaimNames.Iat, JwtHelper.ConvertToUnixTime(DateTime.Now).ToString() },
                    { JwtRegisteredClaimNames.Iss, Issuer },
                    { "DatabaseId", Database },
                    { "ServerId", Server },
                };

                var StringToken = JwtHelper.GenerateToken(Key, InitialPayload);
                return new LoginResult() { Authenticated = true, Token = StringToken, CurrentUserId = User?.Oid.ToString()};
           
        }
    }
}