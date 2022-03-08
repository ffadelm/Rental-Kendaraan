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
    /// main class controller kondisi kendaraan 
    /// </summary>
    /// <remarks>Controller di gunakan untuk menerima memproses dan mengirim Http request maupun response</remarks>
    public class KondisiKendaraansController : Controller
    {
        /// <summary>
        /// class level variable hanya dapat diatur dalam konstruktor dan tidak dapat diubah
        /// </summary>
        private readonly RentKendaraanContext _context;

        /// <summary>
        /// menerapkan akses readonly dalam method Kondisi Kendaraan
        /// </summary>
        /// <param name="context">menginisiaisi variable readonly</param>
        public KondisiKendaraansController(RentKendaraanContext context)
        {
            _context = context;
        }

        /// <summary>
        /// GET: KondisiKendaraans
        /// </summary>
        /// <param name="ktsd">ketersediaan data jenis kendaraan</param>
        /// <param name="searchString">Membuat metode Search data</param>
        /// <param name="sortOrder">Membuat metode pengurutan data</param>
        /// <param name="currentFilter">Membuat filterisasi pada data</param>
        /// <param name="pageNumber">Membuat Pagination pada data yang ditampilkan</param>
        /// <returns>menampilkan semua data kondisi kendaraan</returns>
        public async Task<IActionResult> Index(string ktsd, string searchString, string sortOrder, string currentFilter, int? pageNumber)
        {
            var ktsdList = new List<string>();
            var ktsdQuery = from d in _context.KondisiKendaraans orderby d.NamaKondisi select d.NamaKondisi;

            ktsdList.AddRange(ktsdQuery.Distinct());
            ViewBag.ktsd = new SelectList(ktsdList);

            var menu = from m in _context.KondisiKendaraans select m;

            if (!string.IsNullOrEmpty(ktsd))
            {
                menu = menu.Where(x => x.NamaKondisi == ktsd);
            }

            if (!string.IsNullOrEmpty(searchString))
            {
                menu = menu.Where(s => s.NamaKondisi.Contains(searchString));
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
                    menu = menu.OrderByDescending(s => s.NamaKondisi);
                    break;
                default:
                    menu = menu.OrderBy(s => s.NamaKondisi);
                    break;
            }

            return View(await PaginatedList<KondisiKendaraan>.CreateAsync(menu.AsNoTracking(), pageNumber ?? 1, pageSize));
        }

        /// <summary>
        /// GET: KondisiKendaraans/Details/5
        /// </summary>
        /// <param name="id"></param>
        /// <param name="id">parameter ini digunakan untuk menangkap id yang di kirim dari http request</param>
        /// <returns>menampilkan hasil request detail data kondisi kendaraan berdasarkan id yang diminta</returns>
        /// <remarks>method yang digunakan untuk menampilakn detail data kondisi kendaraan berdasarkan data yang dipilih</remarks>
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var kondisiKendaraan = await _context.KondisiKendaraans
                .FirstOrDefaultAsync(m => m.IdKondisi == id);
            if (kondisiKendaraan == null)
            {
                return NotFound();
            }

            return View(kondisiKendaraan);
        }

        /// <summary>
        /// GET: KondisiKendaraans/Create
        /// </summary>
        /// <returns>menampilkan data</returns>
        public IActionResult Create()
        {
            return View();
        }

        /// <summary>
        /// POST: KondisiKendaraans/Create
        /// </summary>
        /// <param name="kondisiKendaraan">berguna untuk binding data dari database</param>
        /// <returns>menampilkan data hasil input data</returns>
        /// <remarks>method yang di gunakan untuk add/menambahkan data baru</remarks>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("IdKondisi,NamaKondisi")] KondisiKendaraan kondisiKendaraan)
        {
            if (ModelState.IsValid)
            {
                _context.Add(kondisiKendaraan);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(kondisiKendaraan);
        }

        /// <summary>
        /// GET: KondisiKendaraans/Edit/5
        /// </summary>
        /// <param name="id">parameter ini digunakan untuk menangkap id yang di kirim dari http request</param>
        /// <returns>menampilkan data kendaraan berdasarkan id yang diminta</returns>
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var kondisiKendaraan = await _context.KondisiKendaraans.FindAsync(id);
            if (kondisiKendaraan == null)
            {
                return NotFound();
            }
            return View(kondisiKendaraan);
        }

        /// <summary>
        /// POST: KondisiKendaraans/Edit/5
        /// </summary>
        /// <param name="id">parameter ini digunakan untuk menangkap id yang di kirim dari http request</param>
        /// <param name="kondisiKendaraan">digunakan sebagai binding data dari database ke form</param>
        /// <returns>menampilkan hasil editing data jamianan</returns>
        /// <remarks>method yang di gunakan untuk edit/mengubah data sesuai id/data yang di pilih</remarks>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("IdKondisi,NamaKondisi")] KondisiKendaraan kondisiKendaraan)
        {
            if (id != kondisiKendaraan.IdKondisi)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(kondisiKendaraan);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!KondisiKendaraanExists(kondisiKendaraan.IdKondisi))
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
            return View(kondisiKendaraan);
        }

        /// <summary>
        /// GET: KondisiKendaraans/Delete/5
        /// </summary>
        /// <param name="id">parameter ini digunakan untuk menangkap id yang di kirim dari http request</param>
        /// <returns>menampilkan hasil data kondisi kendaraan berdasarkan id yang diminta</returns>
        //
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var kondisiKendaraan = await _context.KondisiKendaraans
                .FirstOrDefaultAsync(m => m.IdKondisi == id);
            if (kondisiKendaraan == null)
            {
                return NotFound();
            }

            return View(kondisiKendaraan);
        }

        /// <summary>
        /// kondisi kendaraan konfirmasi menghapus
        /// </summary>
        /// <param name="id">parameter ini digunakan untuk menangkap id yang di kirim dari http request</param>
        /// <returns>mengembalikan ke halaman index</returns>
        /// <remarks>method yang di gunakan untuk menghapus/delete data sesuai id/data yang di pilih</remarks>
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var kondisiKendaraan = await _context.KondisiKendaraans.FindAsync(id);
            _context.KondisiKendaraans.Remove(kondisiKendaraan);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        private bool KondisiKendaraanExists(int id)
        {
            return _context.KondisiKendaraans.Any(e => e.IdKondisi == id);
        }
    }
}
