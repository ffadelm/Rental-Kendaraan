using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RentalKendaraan.Models.Upload
{
    /// <summary>
    /// main class file input model
    /// </summary>
    public class FileInputModel
    {
        /// <summary>
        /// method upload file
        /// </summary>
        /// <remarks>method untuk upload file menggunakan property</remarks>
        public IFormFile FileToUpload { get; set; }
    }
}
