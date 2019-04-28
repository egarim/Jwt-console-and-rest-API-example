
using Microsoft.IdentityModel.Tokens;
using System;
using System.Diagnostics;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;

namespace SimpleJwt
{
    class Program
    {
        static void Main(string[] args)
        {
            //in production you should not generate a random key but use a fixed key
            var Key = GenerateKey(128);
            Debug.WriteLine(string.Format("{0}:{1}", "Key", Key));
            JwtPayload InitialPayload = new JwtPayload { { "UserOid ", "001" }, { JwtRegisteredClaimNames.Iat, ConvertToUnixTime(DateTime.Now).ToString() }, };
            var StringToken = GenerateToken(Key, InitialPayload);
            Debug.WriteLine(string.Format("{0}:{1}", "Token", StringToken));
            var PayloadFromValidation=ValidateJwtToken(StringToken);
            
            if(PayloadFromValidation.SerializeToJson()==InitialPayload.SerializeToJson())
            {
                Console.WriteLine("Payloads are equal");
                Console.WriteLine(string.Format("{0}:{1}", "PayloadFromValidation", PayloadFromValidation.SerializeToJson()));
            }
            else
            {
                Console.WriteLine("Payloads are NOT equal");
            }
            Console.WriteLine("Press any key to finish");
            Console.ReadLine();


        }
        private static string GenerateKey(int length)
        {
            Random random = new Random();

            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
            return new string(Enumerable.Repeat(chars, length)
              .Select(s => s[random.Next(s.Length)]).ToArray());
        }

   
        public static JwtPayload ValidateJwtToken(string StringToken)
        {
            var Handler = new JwtSecurityTokenHandler();

            var Token = Handler.ReadJwtToken(StringToken);

            JwtPayload Payload = Token.Payload;
            return Payload;
        }
        public static long ConvertToUnixTime(DateTime datetime)
        {
            DateTime sTime = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);

            return (long)(datetime - sTime).TotalSeconds;
        }
        private static string GenerateToken(string key, JwtPayload Payload)
        {



            var StringLenght = key.Length;
            var ByteLenght = System.Text.ASCIIEncoding.Unicode.GetByteCount(key);
            if (ByteLenght < 256)
            {
                throw new ArgumentException($"the byte count of the key should be greater than 256 bytes,the current length is:{ByteLenght}");
            }

            // Create Security key  using private key above:
            // not that latest version of JWT using Microsoft namespace instead of System
            var SecurityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(key));

            // securityKey length MUST be >256 based on the length of the key
            var credentials = new Microsoft.IdentityModel.Tokens.SigningCredentials(SecurityKey, SecurityAlgorithms.HmacSha256Signature);

            // Lets create the token
            var header = new JwtHeader(credentials);

            // Lets add the header and payload
            JwtSecurityToken SecurityToken = new JwtSecurityToken(header, Payload);

            var Handler = new JwtSecurityTokenHandler();

            // Convert the token to string and send it to your client 
            string TokenString = Handler.WriteToken(SecurityToken);
            return TokenString;


        }
    }
}
