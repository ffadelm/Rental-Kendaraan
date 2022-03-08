using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using RentalKendaraan.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace RentalKendaraan.Controllers
{
    /// <summary>
    /// main class Controller Home
    /// </summary>
    /// <remarks>Controller yang digunakan unk membuat cache dan handling error</remarks>
    public class HomeController : Controller
    {
        /// <summary>
        /// class level variable hanya dapat diatur dalam konstruktor dan tidak dapat diubah
        /// </summary>
        private readonly ILogger<HomeController> _logger;

        /// <summary>
        /// menerapkan akses readonly dalam method HomeController
        /// </summary>
        /// <param name="logger"></param>
        /// <remarks>membuat file logger kesalahan yang disesuaikan atau kesalahan dapat didaftarkan sebagai entri log di Windows Event Log</remarks>
        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        /// <summary>
        /// method yang mewakili berbagai kode status HTTP
        /// </summary>
        /// <returns>menampilkan hasil view()</returns>
        public IActionResult Index()
        {
            return View();
        }

        /// <summary>
        /// method yang mewakili berbagai kode status HTTP
        /// </summary>
        /// <returns>menampilkan hasil view()</returns>
        public IActionResult Privacy()
        {
            return View();
        }

        /// <summary>
        /// method yang mewakili berbagai kode status HTTP Error
        /// </summary>
        /// <returns>menampilkan hasil Error view model </returns>
        /// <remarks>method yang digunakan untuk membuat durasi cache untuk login</remarks>
        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
