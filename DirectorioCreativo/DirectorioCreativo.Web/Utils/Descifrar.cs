using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Threading.Tasks;

namespace DirectorioCreativo.Web.Utils
{
    public class Descifrar
    {
        //public static byte[] Salt { get; set; }

        public static string _DesifrarClave(string password, byte[] Salt)
        {
            using (var derivedBytes = new Rfc2898DeriveBytes(password, Salt, iterations: 1000, HashAlgorithmName.SHA256))
            {
                byte[] key = derivedBytes.GetBytes(16);
                var resp = Convert.ToBase64String(key);

                return resp;
            }
        }
    }
}
