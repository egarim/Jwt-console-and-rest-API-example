using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebApiDemo.Models
{
    public class LoginResult
    {
        public bool Authenticated { get; set; }
        public string Token { get; set; }
    }
}