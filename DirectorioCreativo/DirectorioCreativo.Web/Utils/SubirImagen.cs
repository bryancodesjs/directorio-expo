using Microsoft.AspNetCore.Hosting;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace DirectorioCreativo.Web.Utils
{
    public class SubirImagen
    {
        private readonly IHostingEnvironment _env;        

        public void SubirImagenServidor(string base64String, string nombreIMG)
        {
            // SERVIDOR
            //var folderPath = Path.Combine(_env.ContentRootPath, @"C:\inetpub\wwwroot\directorio-creativo\assets\img");

            // DESARROLLO
            var folderPath = Path.Combine(_env.ContentRootPath, @"C:\Users\josue\Desktop\Trabajo\directorio_creativo\directorio_creativo\FrontEnd\src\assets\img");

            try
            {
                if (!Directory.Exists(folderPath))
                {
                    Directory.CreateDirectory(folderPath);
                }

                File.WriteAllBytes(Path.Combine(folderPath, nombreIMG), Convert.FromBase64String(base64String));
            }
            catch (Exception ex)
            {
                throw ex;
            }

            
        }

    }
}
