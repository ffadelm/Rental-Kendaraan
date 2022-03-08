using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http.Headers;
using System.Threading.Tasks;

namespace RentalKendaraan.Models.Upload
{
    /// <summary>
    /// main class file ekstensi 
    /// </summary>
    /// <remarks>class yang digunakan untuk get file yang di upload</remarks>
    public static class IFormFileExtentions
    {
        /// <summary>
        /// method get nama file 
        /// </summary>
        /// <param name="file">inisiasi dari method IFormFile di class File input model</param>
        /// <returns>menampilkan hasil trim string spasi dalam nama file yang di upload</returns>
        /// <remarks>method yang digunakan untuk menghilangkan spasi dalam nama file yang di upload oelh user</remarks>
        public static string GetFilename(this IFormFile file)
        {
            return ContentDispositionHeaderValue.Parse(
                file.ContentDisposition).FileName.ToString().Trim('"');
        }

        /// <summary>
        /// method Get File Stream
        /// </summary>
        /// <param name="file">inisiasi dari method IFormFile di class File input model</param>
        /// <returns>menampilkan hasil dari menginisialisasi instance baru dari kelas MemoryStream</returns>
        /// <remarks>method ini digunakan untuk mengakses penyimpanan file yang di upload</remarks>
        public static async Task<MemoryStream> GetFileStream(this IFormFile file)
        {
            MemoryStream filestream = new MemoryStream();
            await file.CopyToAsync(filestream);
            return filestream;
        }

        /// <summary>
        /// method getfileAraay
        /// </summary>
        /// <param name="file">inisiasi dari method IFormFile di class File input model</param>
        /// <returns>menampilkan hasil dari menginisialisasi instance baru dari kelas MemoryStream yang di konversi ke dalam Array</returns>
        /// <remarks>method yang digunakan untuk membuat array penyimpanan sementara file yang di upload </remarks>
        public static async Task<byte[]> GetFileArray(this IFormFile file)
        {
            MemoryStream filestream = new MemoryStream();
            await file.CopyToAsync(filestream);
            return filestream.ToArray();
        }
    }
}
