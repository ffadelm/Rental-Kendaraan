using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.FileProviders;
using RentalKendaraan.Models.Upload;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace RentalKendaraan.Controllers
{
    /// <summary>
    /// Main Class Controller Upload
    /// </summary>
    /// <remarks>class yang di gunakan untuk upload data seperti file-file</remarks>
    public class UploadController : Controller
    {
        /// <summary>
        /// mengakses konstruktor file provider
        /// </summary>
        private readonly IFileProvider fileProvider; 

        /// <summary>
        /// inject file provider
        /// </summary>
        /// <param name="fileProvider">inisiasi class kontruktor file provider</param>
        /// <remarks>membuat akses ke file</remarks>
        public UploadController(IFileProvider fileProvider) 
        { 
            this.fileProvider = fileProvider; 
        }
        
        /// <summary>
        /// method index
        /// </summary>
        /// <returns>mengembalikan hasil tampilan</returns>
        public IActionResult Index() 
        { 
            return View(); 
        }

        /// <summary>
        /// method Upload file
        /// </summary>
        /// <param name="file">inisiasi dari Ifromfile</param>
        /// <returns>setelah di proses akan di kembalikan Files</returns>
        /// <remarks>membuat method yang beruga mengakses file di end device user</remarks>
        [HttpPost]
        public async Task<IActionResult> UploadFile(IFormFile file)
        {
            if (file == null || file.Length == 0)
                return Content("File Tidak Terpilih");

            var path = Path.Combine(
                Directory.GetCurrentDirectory(), "wwwroot",
                file.GetFilename());

            using(var stream = new FileStream(path, FileMode.Create))
            {
                await file.CopyToAsync(stream);
            }
            return RedirectToAction("Files");
        }

        /// <summary>
        /// method upload file
        /// </summary>
        /// <param name="files">inisiasi list files</param>
        /// <returns></returns>
        /// <remarks>membuat method yang beruga mengakses file di end device user dan di simpan di list</remarks>
        [HttpPost]
        public async Task<IActionResult> UploadFiles(List<IFormFile> files)
        {
            if (files == null || files.Count == 0)
                return Content("File Tidak Terpilih");

            foreach (var file in files)
            {
                var path = Path.Combine(
                Directory.GetCurrentDirectory(), "wwwroot",
                file.GetFilename());

                using (var stream = new FileStream(path, FileMode.Create))
                {
                    await file.CopyToAsync(stream);
                }
            }
            return RedirectToAction("Files");
        }

        /// <summary>
        /// method upload via model
        /// </summary>
        /// <param name="model">inisiasi konstruktor file input model</param>
        /// <returns>setelah di proses akan di arahkan ke files</returns>
        /// <remarks>menginput file yang user pilih melalui model</remarks>
        [HttpPost]
        public async Task<IActionResult> UploadFileViaModel(FileInputModel model)
        {
            if (model == null || model.FileToUpload.Length == 0)
                return Content("File Tidak Terpilih");

            var path = Path.Combine(
                Directory.GetCurrentDirectory(), "wwwroot",
                model.FileToUpload.GetFilename());

            using (var stream = new FileStream(path, FileMode.Create))
            {
                await model.FileToUpload.CopyToAsync(stream);
            }
            return RedirectToAction("Files");
        }

        /// <summary>
        /// method Files directory
        /// </summary>
        /// <returns>menampilkan hasil dari model</returns>
        /// <remarks>membuat direktori setelah file di upload</remarks>
        public IActionResult Files()
        {
            var model = new FilesViewModel();
            foreach (var item in this.fileProvider.GetDirectoryContents(""))
            {
                model.Files.Add(new FileDetails
                {
                    Name = item.Name,
                    Path = item.PhysicalPath
                });
            }
            return View(model);
        }

        /// <summary>
        /// method download file
        /// </summary>
        /// <param name="filename">parameter untuk di gunakan di method ini</param>
        /// <returns>memproses method File untuk mendapatkan path direktori</returns>
        /// <remarks>method yang di guanakan untu mendowload fole yang user telah upload dan di simpan di direktori yag telah di tentukan</remarks>
        public async Task<IActionResult> Download(string filename)
        {
            if (filename == null) return Content("filename tidak terlihat");

            var path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", filename);

            var memory = new MemoryStream(); 
            using (var stream = new FileStream(path, FileMode.Open)) 
            { 
                await stream.CopyToAsync(memory); 
            }
            memory.Position = 0; 
            return File(memory, GetContentType(path), Path.GetFileName(path));
        }

        /// <summary>
        /// method type file
        /// </summary>
        /// <param name="path">variable baru</param>
        /// <returns>mengembalikan ke method Mime</returns>
        /// <remarks>Mengembalikan tipe MIME untuk data. Ini adalah metode akses sederhana yang mengembalikan nilai atribut mimeType.</remarks>
        private string GetContentType(string path)
        {
            var types = GetMimeTypes();
            var ext = Path.GetExtension(path).ToLowerInvariant();
            return types[ext];
        }

        /// <summary>
        /// method tipe file
        /// </summary>
        /// <returns>data file yang dapat di upload </returns>
        /// <remarks>method yang digunakan untuk membatasi tipe file yang dapat di upload</remarks>
        private Dictionary<string, string> GetMimeTypes()
        {
            return new Dictionary<string, string>
            {
                {".png", "image/png"},
                {".jpg", "image/jpeg"},
                {".jpeg", "image/jpeg"},
                {".gif", "image/gif"},                 
                {".csv", "text/csv"},
                {".txt", "text/plain"},                 
                {".pdf", "application/pdf"},                 
                {".doc", "application/vnd.ms-word"},                 
                {".docx", "application/vnd.ms-word"},                 
                {".xls", "application/vnd.ms-excel"},   
                {".xlsx", "application/vnd.openxmlformats  officedocument.spreadsheetml.sheet"}
            };
        }
    }
}
