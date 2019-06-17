using Microsoft.AspNetCore.Mvc;
using SimpleJwt;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
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
            var ParametersByte = await Request.GetRawBodyBytesAsync();
            var LoginParameters = RestApiDataStore.GetObjectsFromByteArray<LoginParameters>(ParametersByte);
            if (LoginParameters.Username == "Joche" && LoginParameters.Password == "123")
            {
                JwtPayload InitialPayload;
                InitialPayload = new JwtPayload {
                { JwtRegisteredClaimNames.NameId, LoginParameters.Username },
                { JwtRegisteredClaimNames.Iat, JwtHelper.ConvertToUnixTime(DateTime.Now).ToString() },
                  { JwtRegisteredClaimNames.Iss, Issuer },};

                var StringToken = JwtHelper.GenerateToken(Key, InitialPayload);
                return new LoginResult() { Authenticated = true, Token = StringToken };
            }
            else
                return new LoginResult() { Authenticated = false, Token = "" };
        }
    }
}