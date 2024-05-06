using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using System.IO;

namespace PeroShopWeb.Providers
{
    public enum Folders
    {
        Productos = 0, Images = 1, Documents = 2, Temp = 3
    }

    public class PathProvider
    {
        private IWebHostEnvironment hostEnvironment;

        public PathProvider(IWebHostEnvironment hostEnvironment)
        {
            this.hostEnvironment = hostEnvironment;
        }

        public string MapPath(string fileName, Folders folder)
        {
            string path = "";
            if(folder == Folders.Productos)
            {
                string carpeta = "Images/Products";
                path = Path.Combine(this.hostEnvironment.WebRootPath, carpeta, fileName);
            }
            else if(folder == Folders.Images)
            {
                string carpeta = "Images/Usuarios";
                path = Path.Combine(this.hostEnvironment.WebRootPath, carpeta, fileName);
            }

            return path;
        }
    }
}
