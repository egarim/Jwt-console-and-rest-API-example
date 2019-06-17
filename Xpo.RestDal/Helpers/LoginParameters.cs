using System;
using System.Linq;

namespace Xpo.RestDataStore
{
    public class LoginParameters
    {
        public string Username { get; set; }
        public string Password { get; set; }
        public string Server { get; set; }        
        public string Database { get; set; }

        public LoginParameters(string username, string password, string server, string database)
        {
            Username = username;
            Password = password;
            Server = server;
            Database = database;
        }

        public LoginParameters()
        {
        }
    }
}