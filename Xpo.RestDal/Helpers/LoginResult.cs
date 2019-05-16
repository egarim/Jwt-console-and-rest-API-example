using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Xpo.RestDataStore
{
    public class LoginResult
    {
        public bool Authenticated { get; set; }
        public string Token { get; set; }

        public LoginResult(bool authenticated, string token)
        {
            Authenticated = authenticated;
            Token = token;
        }

        public LoginResult()
        {
        }
    }
}