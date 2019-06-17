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
        public string CurrentUserId { get; set; }
        public string ErrorMessage { get; set; }


        public LoginResult(bool authenticated, string token, string userid, string error = "")
        {
            Authenticated = authenticated;
            Token = token;
            CurrentUserId = userid;
            ErrorMessage = error;
        }

        public LoginResult()
        {
        }
    }
}