using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using RentalKendaraan.Models;

namespace RentalKendaraan.Controllers
{
    /// <summary>
    /// main class Controller Kendaraan
    /// </summary>
    /// <remarks>Controller di gunakan untuk menerima memproses dan mengirim Http request maupun response</remarks>
    public class KendaraansController : Controller
    {
        /// <summary>
        /// class level variable hanya dapat diatur dalam konstruktor dan tidak dapat diubah
        /// </summary>
        private readonly RentKendaraanContext _context;

        /// <summary>
        /// menerapkan akses readonly dalam method KendaraanController
        /// </summary>
        /// <param name="context">menginisiaisi variable readonly</param>
        public KendaraansController(RentKendaraanContext context)
        {
            _context = context;
        }

        /// <summary>
        /// GET: Kendaraans
        /// </summary>
        /// <param name="ktsd">ketersediaan data kendaraan</param>
        /// <param name="searchString">Membuat metode Search data</param>
        /// <param name="sortOrder">Membuat metode pengurutan data</param>
        /// <param name="currentFilter">Membuat filterisasi pada data</param>
        /// <param name="pageNumber">Membuat Pagination pada data yang ditampilkan</param>
        /// <returns>menampilkan semua data dari kendaraan</returns>
        public async Task<IActionResult> Index(string ktsd, string searchString, string sortOrder, string currentFilter, int? pageNumber)
        {
            var ktsdList = new List<string>();
            var ktsdQuery = from d in _context.Kendaraans orderby d.Ketersediaan select d.Ketersediaan;

            ktsdList.AddRange(ktsdQuery.Distinct());
            ViewBag.ktsd = new SelectList(ktsdList);

            var menu = from m in _context.Kendaraans.Include(k => k.IdJenisKendaraanNavigation) select m;

            if (!string.IsNullOrEmpty(ktsd))
            {
                menu = menu.Where(x => x.Ketersediaan == ktsd);
            }

            if (!string.IsNullOrEmpty(searchString))
            {
                menu = menu.Where(s => s.NoPolisi.Contains(searchString) || s.NamaKendaraan.Contains(searchString) || s.NoStnk.Contains(searchString) || s.IdJenisKendaraanNavigation.NamaJenisKendaraan.Contains(searchString));
            }

            //
            ViewData["CurrentFilter"] = searchString;
            ViewData["CurrentSort"] = sortOrder;
            //definiosi jumlah maksimum data yang akan di tampilkan
            int pageSize = 5;

            if (searchString != null)
            {
                pageNumber = 1;
            }
            else
            {
                searchString = currentFilter;
            }

            //untuk sorting
            ViewData["NameSortParm"] = String.IsNullOrEmpty(sortOrder) ? "name_desc" : "";
            ViewData["KtsdSortParm"] = sortOrder == "Ktsd" ? "ktsd_desc" : "Ktsd";

            switch (sortOrder)
            {
                case "name_desc":
                    menu = menu.OrderByDescending(s => s.NamaKendaraan);
                    break;
                case "Ktsd":
                    menu = menu.OrderBy(s => s.Ketersediaan);
                    break;
                case "ktsd_desc":
                    menu = menu.OrderByDescending(s => s.Ketersediaan);
                    break;
                default:
                    menu = menu.OrderBy(s => s.NamaKendaraan);
                    break;
            }

            return View(await PaginatedList<Kendaraan>.CreateAsync(menu.AsNoTracking(), pageNumber ?? 1, pageSize));
        }

        /// <summary>
        /// GET: Kendaraans/Details/5
        /// </summary>
        /// <param name="id">parameter ini digunakan untuk menangkap id yang di kirim dari http request</param>
        /// <returns>menampilkan hasil request detail data kendaraan berdasarkan id yang diminta</returns>
        /// <remarks>method yang digunakan untuk menampilakn detail data kendaraan berdasarkan data yang dipilih</remarks>
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var kendaraan = await _context.Kendaraans
                .Include(k => k.IdJenisKendaraanNavigation)
                .FirstOrDefaultAsync(m => m.IdKendaraan == id);
            if (kendaraan == null)
            {
                return NotFound();
            }

            return View(kendaraan);
        }

        /// <summary>
        /// GET: Kendaraans/Create
        /// </summary>
        /// <returns>menampilkan data id jenis kendaraan</returns>
        public IActionResult Create()
        {
            ViewData["IdJenisKendaraan"] = new SelectList(_context.JenisKendaraans, "IdJenisKendaraan", "IdJenisKendaraan");
            return View();
        }

        /// <summary>
        /// POST: Kendaraans/Create
        /// </summary>
        /// <param name="kendaraan">berguna untuk binding data dari database</param>
        /// <returns>menampilkan data hasil input data</returns>
        /// <remarks>method yang di gunakan untuk add/menambahkan data baru</remarks>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("IdKendaraan,NamaKendaraan,NoPolisi,NoStnk,IdJenisKendaraan,Ketersediaan")] Kendaraan kendaraan)
        {
            if (ModelState.IsValid)
            {
                _context.Add(kendaraan);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["IdJenisKendaraan"] = new SelectList(_context.JenisKendaraans, "IdJenisKendaraan", "IdJenisKendaraan", kendaraan.IdJenisKendaraan);
            return View(kendaraan);
        }

        /// <summary>
        /// GET: Kendaraans/Edit/5
        /// </summary>
        /// <param name="id">parameter ini digunakan untuk menangkap id yang di kirim dari http request</param>
        /// <returns>menampilkan data serta id jenis kendaraan berdasarkan id yang diminta</returns>
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var kendaraan = await _context.Kendaraans.FindAsync(id);
            if (kendaraan == null)
            {
                return NotFound();
            }
            ViewData["IdJenisKendaraan"] = new SelectList(_context.JenisKendaraans, "IdJenisKendaraan", "IdJenisKendaraan", kendaraan.IdJenisKendaraan);
            return View(kendaraan);
        }

        /// <summary>
        /// POST: Kendaraans/Edit/5
        /// </summary>
        /// <param name="id">parameter ini digunakan untuk menangkap id yang di kirim dari http request</param>
        /// <param name="kendaraan">berguna untuk binding data dari database ke form</param>
        /// <returns>menampilkan hasil editing data kendaraan</returns>
        /// <remarks>method yang di gunakan untuk edit/mengubah data sesuai id/data yang di pilih</remarks>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("IdKendaraan,NamaKendaraan,NoPolisi,NoStnk,IdJenisKendaraan,Ketersediaan")] Kendaraan kendaraan)
        {
            if (id != kendaraan.IdKendaraan)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(kendaraan);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!KendaraanExists(kendaraan.IdKendaraan))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            ViewData["IdJenisKendaraan"] = new SelectList(_context.JenisKendaraans, "IdJenisKendaraan", "IdJenisKendaraan", kendaraan.IdJenisKendaraan);
            return View(kendaraan);
        }

        /// <summary>
        /// GET: Kendaraans/Delete/5
        /// </summary>
        /// <param name="id">parameter ini digunakan untuk menangkap id yang di kirim dari http request</param>
        /// <returns>menampilkan data kendaraan berdasarkan id yang diminta</returns>
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var kendaraan = await _context.Kendaraans
                .Include(k => k.IdJenisKendaraanNavigation)
                .FirstOrDefaultAsync(m => m.IdKendaraan == id);
            if (kendaraan == null)
            {
                return NotFound();
            }

            return View(kendaraan);
        }

        /// <summary>
        /// Kendaraan Delete Confirm
        /// </summary>
        /// <param name="id">parameter ini digunakan untuk menangkap id yang di kirim dari http request</param>
        /// <returns>mengembalikan ke halaman index</returns>
        /// <remarks>method yang di gunakan untuk menghapus/delete data sesuai id/data yang di pilih</remarks>
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var kendaraan = await _context.Kendaraans.FindAsync(id);
            _context.Kendaraans.Remove(kendaraan);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        /// <summary>
        /// cek ketersediaaan data kendaraan
        /// </summary>
        /// <param name="id">membuat variable id</param>
        /// <returns>menampilkan hasil pengecekan apakah tersedia id kendaraan</returns>
        /// <remarks>method ini digunakan untuk mengcek ketersediaan data kendaraan berdasarkan id yang di tangkap dari database</remarks>
        private bool KendaraanExists(int id)
        {
            return _context.Kendaraans.Any(e => e.IdKendaraan == id);
        }
    }
}
