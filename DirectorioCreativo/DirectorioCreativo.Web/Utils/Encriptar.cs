using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Threading.Tasks;

namespace DirectorioCreativo.Web.Utils
{
    public class Encriptar
    {
        public static List<object> EncryptarClave(string password)
        {
            List<object> callBack = new List<object>();

            byte[] salt;
            using (var derivedBytes = new System.Security.Cryptography.Rfc2898DeriveBytes(password, saltSize: 16, iterations: 1000, HashAlgorithmName.SHA256))
            {
                salt = derivedBytes.Salt;
                byte[] key = derivedBytes.GetBytes(16);
                var resp = Convert.ToBase64String(key);

                callBack.Add(salt);
                callBack.Add(resp);

                return callBack;
            }
        }


    }
}
