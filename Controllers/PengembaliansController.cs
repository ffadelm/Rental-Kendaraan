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
    /// Main class controller pengembalian
    /// </summary>
    /// <remarks>Controller di gunakan untuk menerima memproses dan mengirim Http request maupun response</remarks>
    public class PengembaliansController : Controller
    {
        /// <summary>
        /// class level variable hanya dapat diatur dalam konstruktor dan tidak dapat diubah
        /// </summary>
        private readonly RentKendaraanContext _context;

        /// <summary>
        /// menerapkan akses readonly dalam method pengembalian
        /// </summary>
        /// <param name="context">menginisiaisi variable readonly</param>
        public PengembaliansController(RentKendaraanContext context)
        {
            _context = context;
        }

        /// <summary>
        /// GET: Pengembalian
        /// </summary>
        /// <param name="ktsd">ketersediaan data jenis pengembalian</param>
        /// <param name="searchString">Membuat metode Search data</param>
        /// <param name="sortOrder">Membuat metode pengurutan data</param>
        /// <param name="currentFilter">Membuat filterisasi pada data</param>
        /// <param name="pageNumber">Membuat Pagination pada data yang ditampilkan</param>
        /// <returns>menampilkan semua data pengembalian </returns>
        public async Task<IActionResult> Index(string ktsd, string searchString, string sortOrder, string currentFilter, int? pageNumber)
        {
            var ktsdList = new List<string>();
            var ktsdQuery = from d in _context.Pengembalians orderby d.IdKondisiNavigation.NamaKondisi select d.IdKondisiNavigation.NamaKondisi;

            ktsdList.AddRange(ktsdQuery.Distinct());
            ViewBag.ktsd = new SelectList(ktsdList);

            var menu = from m in _context.Pengembalians.Include(p => p.IdKondisiNavigation).Include(p => p.IdPeminjamanNavigation) select m;


            if (!string.IsNullOrEmpty(ktsd))
            {
                menu = menu.Where(x => x.IdKondisiNavigation.NamaKondisi == ktsd);
            }

            if (!string.IsNullOrEmpty(searchString))
            {
                menu = menu.Where(s => s.TglPengembalian.ToString().Contains(searchString)||s.IdKondisiNavigation.NamaKondisi.Contains(searchString));
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
            ViewData["DendaSortParm"] = String.IsNullOrEmpty(sortOrder) ? "denda_desc" : "";
            ViewData["DateSortParm"] = sortOrder == "Date" ? "date_desc" : "Date";

            switch (sortOrder)
            {
                case "denda_desc":
                    menu = menu.OrderByDescending(s => s.Denda.ToString());
                    break;
                case "Date":
                    menu = menu.OrderBy(s => s.TglPengembalian);
                    break;
                case "date_desc":
                    menu = menu.OrderByDescending(s => s.TglPengembalian);
                    break;
                default:
                    menu = menu.OrderBy(s => s.TglPengembalian);
                    break;
            }

            return View(await PaginatedList<Pengembalian>.CreateAsync(menu.AsNoTracking(), pageNumber ?? 1, pageSize));
        }

        /// <summary>
        /// GET: Pengembalian/Details/5
        /// </summary>
        /// <param name="id">parameter ini digunakan untuk menangkap id yang di kirim dari http request</param>
        /// <returns>menampilkan hasil request detail data pengembalian berdasarkan id yang diminta</returns>
        /// <remarks>method yang digunakan untuk menampilakn detail data pengembalian berdasarkan data yang dipilih</remarks>
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var pengembalian = await _context.Pengembalians
                .Include(p => p.IdKondisiNavigation)
                .Include(p => p.IdPeminjamanNavigation)
                .FirstOrDefaultAsync(m => m.IdPengembalian == id);
            if (pengembalian == null)
            {
                return NotFound();
            }

            return View(pengembalian);
        }

        /// <summary>
        /// GET: Pengembalian/Create
        /// </summary>
        /// <returns>menampilkan data Kondisi dan peminjaman</returns>
        public IActionResult Create()
        {
            ViewData["IdKondisi"] = new SelectList(_context.KondisiKendaraans, "IdKondisi", "IdKondisi");
            ViewData["IdPeminjaman"] = new SelectList(_context.Peminjamen, "IdPeminjaman", "IdPeminjaman");
            return View();
        }

        /// <summary>
        /// POST: Pengembalian/Create
        /// </summary>
        /// <param name="pengembalian">berguna untuk binding data dari database</param>
        /// <returns>menampilkan data hasil input data</returns>
        /// <remarks>method yang di gunakan untuk add/menambahkan data baru</remarks>
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("IdPengembalian,TglPengembalian,IdPeminjaman,IdKondisi,Denda")] Pengembalian pengembalian)
        {
            if (ModelState.IsValid)
            {
                _context.Add(pengembalian);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["IdKondisi"] = new SelectList(_context.KondisiKendaraans, "IdKondisi", "IdKondisi", pengembalian.IdKondisi);
            ViewData["IdPeminjaman"] = new SelectList(_context.Peminjamen, "IdPeminjaman", "IdPeminjaman", pengembalian.IdPeminjaman);
            return View(pengembalian);
        }

        /// <summary>
        /// GET: Pengembalians/Edit/5
        /// </summary>
        /// <param name="id">parameter ini digunakan untuk menangkap id yang di kirim dari http request</param>
        /// <returns>menampilkan data pengembalian serta data kondisi dan peminjaman berdasarkan id yang diminta</returns>
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var pengembalian = await _context.Pengembalians.FindAsync(id);
            if (pengembalian == null)
            {
                return NotFound();
            }
            ViewData["IdKondisi"] = new SelectList(_context.KondisiKendaraans, "IdKondisi", "IdKondisi", pengembalian.IdKondisi);
            ViewData["IdPeminjaman"] = new SelectList(_context.Peminjamen, "IdPeminjaman", "IdPeminjaman", pengembalian.IdPeminjaman);
            return View(pengembalian);
        }

        /// <summary>
        /// POST: Pengembalian/Edit/5
        /// </summary>
        /// <param name="id"></param>
        /// <param name="pengembalian">berguna untuk binding data dari database ke form</param>
        /// <returns>menampilkan hasil editing data pengembalian</returns>
        /// <remarks>method yang di gunakan untuk edit/mengubah data sesuai id/data yang di pilih</remarks>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("IdPengembalian,TglPengembalian,IdPeminjaman,IdKondisi,Denda")] Pengembalian pengembalian)
        {
            if (id != pengembalian.IdPengembalian)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(pengembalian);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!PengembalianExists(pengembalian.IdPengembalian))
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
            ViewData["IdKondisi"] = new SelectList(_context.KondisiKendaraans, "IdKondisi", "IdKondisi", pengembalian.IdKondisi);
            ViewData["IdPeminjaman"] = new SelectList(_context.Peminjamen, "IdPeminjaman", "IdPeminjaman", pengembalian.IdPeminjaman);
            return View(pengembalian);
        }

        /// <summary>
        /// GET: Pengembalian/Delete/5
        /// </summary>
        /// <param name="id">parameter ini digunakan untuk menangkap id yang di kirim dari http request</param>
        /// <returns>menampilkan data peengambalian berdasarkan id yang diminta</returns>
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var pengembalian = await _context.Pengembalians
                .Include(p => p.IdKondisiNavigation)
                .Include(p => p.IdPeminjamanNavigation)
                .FirstOrDefaultAsync(m => m.IdPengembalian == id);
            if (pengembalian == null)
            {
                return NotFound();
            }

            return View(pengembalian);
        }

        /// <summary>
        /// konfirmasi delete data pengembalian
        /// </summary>
        /// <param name="id">parameter ini digunakan untuk menangkap id yang di kirim dari http request</param>
        /// <returns>mengembalikan ke halaman index</returns>
        /// <remarks>method yang di gunakan untuk menghapus/delete data sesuai id/data yang di pilih</remarks>
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var pengembalian = await _context.Pengembalians.FindAsync(id);
            _context.Pengembalians.Remove(pengembalian);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        private bool PengembalianExists(int id)
        {
            return _context.Pengembalians.Any(e => e.IdPengembalian == id);
        }
    }
}
