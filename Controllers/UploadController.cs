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
    /// 
    /// </summary>
    public class UploadController : Controller
    {
        /// <summary>
        /// 
        /// </summary>
        private readonly IFileProvider fileProvider; 

        /// <summary>
        /// 
        /// </summary>
        /// <param name="fileProvider"></param>
        public UploadController(IFileProvider fileProvider) 
        { 
            this.fileProvider = fileProvider; 
        }
        
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public IActionResult Index() 
        { 
            return View(); 
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="file"></param>
        /// <returns></returns>
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
        /// 
        /// </summary>
        /// <param name="files"></param>
        /// <returns></returns>
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
        /// 
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
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
        /// 
        /// </summary>
        /// <returns></returns>
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
        /// 
        /// </summary>
        /// <param name="filename"></param>
        /// <returns></returns>
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
        /// 
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        private string GetContentType(string path)
        {
            var types = GetMimeTypes();
            var ext = Path.GetExtension(path).ToLowerInvariant();
            return types[ext];
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
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
