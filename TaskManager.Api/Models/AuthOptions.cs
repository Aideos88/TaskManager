using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace TaskManager.Api.Models
{
    public class AuthOptions
    {
        public const string ISSUER = "MyAuthServer"; //издатель токена
        public const string AUDIENCE = "MyAuthClient"; // потребитель токена
        //private const string KEY = "mysupersecret_secretkey!123"; // ключ для шифрации
        private const string KEY = "this is my custom Secret key for authentication!"; // ключ для шифрации
        public const int LIFETIME = 1; // время жизни токена - 1 минута
        public static SymmetricSecurityKey GetSymmetricSecurityKey() => 
            new SymmetricSecurityKey(Encoding.UTF8.GetBytes(KEY)); 
    }
}
