using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using RentalKendaraan.Models;
using Microsoft.AspNetCore.Authorization;

namespace RentalKendaraan.Controllers

{   /// <summary>
    /// Main class controller peminjaman
    /// </summary>
    /// <remarks>Controller di gunakan untuk menerima memproses dan mengirim Http request maupun response</remarks>
    public class PeminjamansController : Controller
    {
        /// <summary>
        /// class level variable hanya dapat diatur dalam konstruktor dan tidak dapat diubah
        /// </summary>
        private readonly RentKendaraanContext _context;

        /// <summary>
        /// menerapkan akses readonly dalam method peminjaman
        /// </summary>
        /// <param name="context">menginisiaisi variable readonly</param>
        public PeminjamansController(RentKendaraanContext context)
        {
            _context = context;
        }

        /// <summary>
        /// GET: Peminjaman 
        /// </summary>
        /// <param name="ktsd">ketersediaan data jenis peminjaman</param>
        /// <param name="searchString">Membuat metode Search data</param>
        /// <param name="sortOrder">Membuat metode pengurutan data</param>
        /// <param name="currentFilter">Membuat filterisasi pada data</param>
        /// <param name="pageNumber">Membuat Pagination pada data yang ditampilkan</param>
        /// <returns>menampilkan semua data kondisi kendaraan</returns>
        [Authorize(Policy ="readonlypolicy")] //tambahan
        public async Task<IActionResult> Index(string ktsd, string searchString, string sortOrder, string currentFilter, int? pageNumber)
        {
            var ktsdList = new List<string>();
            var ktsdQuery = from d in _context.Peminjamen orderby d.IdCustomerNavigation.NamaCustomer select d.IdCustomerNavigation.NamaCustomer;
            ktsdList.AddRange(ktsdQuery.Distinct());
            ViewBag.ktsd = new SelectList(ktsdList);

            var menu = from m in _context.Peminjamen.Include(k => k.IdKendaraanNavigation).Include(k => k.IdCustomerNavigation).Include(k => k.IdJaminanNavigation) select m;

            if (!string.IsNullOrEmpty(ktsd))
            {
                menu = menu.Where(x => x.IdCustomerNavigation.NamaCustomer == ktsd);
            }

            if (!string.IsNullOrEmpty(searchString))
            {
                menu = menu.Where(s => s.Biaya.ToString().Contains(searchString) || s.TglPeminjaman.ToString().Contains(searchString) || s.IdCustomerNavigation.NamaCustomer.Contains(searchString));
            }

            //
            ViewData["CurrentFilter"] = searchString;
            ViewData["CurrentSort"] = sortOrder;
            //definiosi jumlah maksimum data yang akan di tampilkan
            int pageSize = 5;

            if(searchString != null)
            {
                pageNumber = 1;
            }
            else
            {
                searchString = currentFilter;
            }

            //untuk sorting
            ViewData["NameSortParm"] = String.IsNullOrEmpty(sortOrder) ? "name_desc" : "";
            ViewData["DateSortParm"] = sortOrder == "Date" ? "date_desc" : "Date";

            switch (sortOrder)
            {
                case "name_desc":
                    menu = menu.OrderByDescending(s => s.IdCustomerNavigation.NamaCustomer);
                    break;
                case "Date":
                    menu = menu.OrderBy(s => s.TglPeminjaman);
                    break;
                case "date_desc":
                    menu = menu.OrderByDescending(s => s.TglPeminjaman);
                    break;
                default:
                    menu = menu.OrderBy(s => s.IdCustomerNavigation.NamaCustomer);
                    break;
            }

            return View(await PaginatedList<Peminjaman>.CreateAsync(menu.AsNoTracking(), pageNumber ?? 1, pageSize));
            
        }

        /// <summary>
        /// GET: Peminjamen/Details/5
        /// </summary>
        /// <param name="id">parameter ini digunakan untuk menangkap id yang di kirim dari http request</param>
        /// <returns>menampilkan hasil request detail data peminjaman berdasarkan id yang diminta</returns>
        /// <remarks>method yang digunakan untuk menampilakn detail data peminjaman berdasarkan data yang dipilih</remarks>
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var peminjaman = await _context.Peminjamen
                .Include(p => p.IdCustomerNavigation)
                .Include(p => p.IdJaminanNavigation)
                .Include(p => p.IdKendaraanNavigation)
                .FirstOrDefaultAsync(m => m.IdPeminjaman == id);
            if (peminjaman == null)
            {
                return NotFound();
            }

            return View(peminjaman);
        }

        /// <summary>
        /// GET: Peminjaman/Create
        /// </summary>
        /// <returns>menampilkan data customer, jaminan dan kendaraan</returns>
        [Authorize(Policy = "writepolicy")] //tambahan
        public IActionResult Create()
        {
            ViewData["IdCustomer"] = new SelectList(_context.Customers, "IdCustomer", "IdCustomer");
            ViewData["IdJaminan"] = new SelectList(_context.Jaminans, "IdJaminan", "IdJaminan");
            ViewData["IdKendaraan"] = new SelectList(_context.Kendaraans, "IdKendaraan", "IdKendaraan");
            return View();
        }

        /// <summary>
        /// POST: Peminjaman/Create
        /// </summary>
        /// <param name="peminjaman">berguna untuk binding data dari database</param>
        /// <returns>menampilkan data hasil input data</returns>
        /// <remarks>method yang di gunakan untuk add/menambahkan data baru</remarks>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("IdPeminjaman,TglPeminjaman,IdKendaraan,IdCustomer,IdJaminan,Biaya")] Peminjaman peminjaman)
        {
            if (ModelState.IsValid)
            {
                _context.Add(peminjaman);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["IdCustomer"] = new SelectList(_context.Customers, "IdCustomer", "IdCustomer", peminjaman.IdCustomer);
            ViewData["IdJaminan"] = new SelectList(_context.Jaminans, "IdJaminan", "IdJaminan", peminjaman.IdJaminan);
            ViewData["IdKendaraan"] = new SelectList(_context.Kendaraans, "IdKendaraan", "IdKendaraan", peminjaman.IdKendaraan);
            return View(peminjaman);
        }

        /// <summary>
        /// GET: Peminjaman/Edit/5
        /// </summary>
        /// <param name="id">parameter ini digunakan untuk menangkap id yang di kirim dari http request</param>
        /// <returns>menampilkan data serta data customer, jaminana, dan kendaraan berdasarkan id yang diminta</returns>
        [Authorize(Policy = "editpolicy")]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var peminjaman = await _context.Peminjamen.FindAsync(id);
            if (peminjaman == null)
            {
                return NotFound();
            }
            ViewData["IdCustomer"] = new SelectList(_context.Customers, "IdCustomer", "IdCustomer", peminjaman.IdCustomer);
            ViewData["IdJaminan"] = new SelectList(_context.Jaminans, "IdJaminan", "IdJaminan", peminjaman.IdJaminan);
            ViewData["IdKendaraan"] = new SelectList(_context.Kendaraans, "IdKendaraan", "IdKendaraan", peminjaman.IdKendaraan);
            return View(peminjaman);
        }

        /// <summary>
        /// POST: Peminjaman/Edit/5 
        /// </summary>
        /// <param name="id">parameter ini digunakan untuk menangkap id yang di kirim dari http request</param>
        /// <param name="peminjaman">berguna untuk binding data dari database ke form</param>
        /// <returns>menampilkan hasil editing data peminjaman</returns>
        /// <remarks>method yang di gunakan untuk edit/mengubah data sesuai id/data yang di pilih</remarks>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("IdPeminjaman,TglPeminjaman,IdKendaraan,IdCustomer,IdJaminan,Biaya")] Peminjaman peminjaman)
        {
            if (id != peminjaman.IdPeminjaman)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(peminjaman);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!PeminjamanExists(peminjaman.IdPeminjaman))
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
            ViewData["IdCustomer"] = new SelectList(_context.Customers, "IdCustomer", "IdCustomer", peminjaman.IdCustomer);
            ViewData["IdJaminan"] = new SelectList(_context.Jaminans, "IdJaminan", "IdJaminan", peminjaman.IdJaminan);
            ViewData["IdKendaraan"] = new SelectList(_context.Kendaraans, "IdKendaraan", "IdKendaraan", peminjaman.IdKendaraan);
            return View(peminjaman);
        }

        /// <summary>
        /// GET: Peminjamen/Delete/5
        /// </summary>
        /// <param name="id">parameter ini digunakan untuk menangkap id yang di kirim dari http request</param>
        /// <returns>menampilkan data peminjaman berdasarkan id yang diminta</returns>
        [Authorize(Policy = "deletepolicy")]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var peminjaman = await _context.Peminjamen
                .Include(p => p.IdCustomerNavigation)
                .Include(p => p.IdJaminanNavigation)
                .Include(p => p.IdKendaraanNavigation)
                .FirstOrDefaultAsync(m => m.IdPeminjaman == id);
            if (peminjaman == null)
            {
                return NotFound();
            }

            return View(peminjaman);
        }

        /// <summary>
        /// konfirmasi delete data peminjaman 
        /// </summary>
        /// <param name="id">parameter ini digunakan untuk menangkap id yang di kirim dari http request</param>
        /// <returns>mengembalikan ke halaman index</returns>
        /// <remarks>method yang di gunakan untuk menghapus/delete data sesuai id/data yang di pilih</remarks>
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var peminjaman = await _context.Peminjamen.FindAsync(id);
            _context.Peminjamen.Remove(peminjaman);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        private bool PeminjamanExists(int id)
        {
            return _context.Peminjamen.Any(e => e.IdPeminjaman == id);
        }
    }
}
