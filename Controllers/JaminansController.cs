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
    /// main class Controller Jaminan
    /// </summary>
    /// <remarks>Controller di gunakan untuk menerima memproses dan mengirim Http request maupun response</remarks>
    public class JaminansController : Controller
    {
        /// <summary>
        /// class level variable hanya dapat diatur dalam konstruktor dan tidak dapat diubah
        /// </summary>
        private readonly RentKendaraanContext _context;

        /// <summary>
        /// menerapkan akses readonly dalam method JaminansController
        /// </summary>
        /// <param name="context">menginisiaisi variable readonly</param>
        public JaminansController(RentKendaraanContext context)
        {
            _context = context;
        }

        /// <summary>
        /// GET: Jaminans
        /// </summary>
        /// <param name="ktsd">ketersediaan data jaminan</param>
        /// <param name="searchString">Membuat metode Search data</param>
        /// <param name="sortOrder">Membuat metode pengurutan data</param>
        /// <param name="currentFilter">Membuat filterisasi pada data</param>
        /// <param name="pageNumber">Membuat Pagination pada data yang ditampilkan</param>
        /// <returns>menampilkan semua data dari jaminan</returns>
        public async Task<IActionResult> Index(string ktsd, string searchString, string sortOrder, string currentFilter, int? pageNumber)
        {
            var ktsdList = new List<string>();
            var ktsdQuery = from d in _context.Jaminans orderby d.NamaJaminan select d.NamaJaminan;

            ktsdList.AddRange(ktsdQuery.Distinct());
            ViewBag.ktsd = new SelectList(ktsdList);

            var menu = from m in _context.Jaminans select m;

            if (!string.IsNullOrEmpty(ktsd))
            {
                menu = menu.Where(x => x.NamaJaminan == ktsd);
            }

            if (!string.IsNullOrEmpty(searchString))
            {
                menu = menu.Where(s => s.NamaJaminan.Contains(searchString));
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

            switch (sortOrder)
            {
                case "name_desc":
                    menu = menu.OrderByDescending(s => s.NamaJaminan);
                    break;
                default:
                    menu = menu.OrderBy(s => s.NamaJaminan);
                    break;
            }

            return View(await PaginatedList<Jaminan>.CreateAsync(menu.AsNoTracking(), pageNumber ?? 1, pageSize));
        }

        /// <summary>
        /// GET: Jaminans/Details/5
        /// </summary>
        /// <param name="id">parameter ini digunakan untuk menangkap id yang di kirim dari http request</param>
        /// <returns>menampilkan hasil request detail data jaminan berdasarkan id yang diminta</returns>
        /// <remarks>method yang digunakan untuk menampilakn detail data jaminan berdasarkan data yang dipilih</remarks>
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var jaminan = await _context.Jaminans
                .FirstOrDefaultAsync(m => m.IdJaminan == id);
            if (jaminan == null)
            {
                return NotFound();
            }

            return View(jaminan);
        }

        /// <summary>
        /// GET: Jaminans/Create
        /// </summary>
        /// <returns>menampilkan data</returns>
        public IActionResult Create()
        {
            return View();
        }

        /// <summary>
        /// POST: Jaminans/Create
        /// </summary>
        /// <param name="jaminan">digunakan sebagai binding data dari database</param>
        /// <returns>menampilkan hasil input data</returns>
        /// <remarks>method yang digunakan untuk add/menambahkan data baru</remarks>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("IdJaminan,NamaJaminan")] Jaminan jaminan)
        {
            if (ModelState.IsValid)
            {
                _context.Add(jaminan);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(jaminan);
        }

        /// <summary>
        /// GET: Jaminans/Edit/5
        /// </summary>
        /// <param name="id">parameter ini digunakan untuk menangkap id yang di kirim dari http request</param>
        /// <returns>menampilkan hasil data jaminan yang diminta berdasarkan id</returns>
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var jaminan = await _context.Jaminans.FindAsync(id);
            if (jaminan == null)
            {
                return NotFound();
            }
            return View(jaminan);
        }

        /// <summary>
        /// POST: Jaminans/Edit/5
        /// </summary>
        /// <param name="id">parameter ini digunakan untuk menangkap id yang di kirim dari http request</param>
        /// <param name="jaminan">digunakan sebagai binding data dari database ke form</param>
        /// <returns>menampilkan hasil editing data jaminan</returns>
        /// <remarks>method yang di gunakan untuk edit/mengubah data sesuai id/data yang di pilih</remarks>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("IdJaminan,NamaJaminan")] Jaminan jaminan)
        {
            if (id != jaminan.IdJaminan)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(jaminan);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!JaminanExists(jaminan.IdJaminan))
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
            return View(jaminan);
        }

        /// <summary>
        /// GET: Jaminans/Delete/5
        /// </summary>
        /// <param name="id">parameter ini digunakan untuk menangkap id yang di kirim dari http request</param>
        /// <returns>menampilkan hasil data jaminan berdasarkan id yang diminta</returns>
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var jaminan = await _context.Jaminans
                .FirstOrDefaultAsync(m => m.IdJaminan == id);
            if (jaminan == null)
            {
                return NotFound();
            }

            return View(jaminan);
        }

        /// <summary>
        /// Jaminan Delete Confirm 
        /// </summary>
        /// <param name="id">parameter ini digunakan untuk menangkap id yang di kirim dari http request</param>
        /// <returns>mengembalikan ke halaman index</returns>
        /// <remarks>method yang di gunakan untuk menghapus/delete data sesuai id/data yang di pilih</remarks>
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var jaminan = await _context.Jaminans.FindAsync(id);
            _context.Jaminans.Remove(jaminan);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        /// <summary>
        /// cek ketersediaaan data Jaminan
        /// </summary>
        /// <param name="id">membuat variable id</param>
        /// <returns>menampilkan hasil pengecekan apakah tersedia id jaminan</returns>
        /// <remarks>method ini digunakan untuk mengcek ketersediaan data jaminan berdasarkan id yang di tangkap dari database</remarks>
        private bool JaminanExists(int id)
        {
            return _context.Jaminans.Any(e => e.IdJaminan == id);
        }
    }
}
