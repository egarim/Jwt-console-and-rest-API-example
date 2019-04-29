using System;
using System.Linq;

namespace Xpo.RestDataStore
{
    public class LoginParameters
    {
        public string Username { get; set; }
        public string Password { get; set; }

        public LoginParameters(string username, string password)
        {
            Username = username;
            Password = password;
        }

        public LoginParameters()
        {
        }
    }
}