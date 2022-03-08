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
    /// main class Gender Controller
    /// </summary>
    /// <remarks>Controller di gunakan untuk menerima memproses dan mengirim Http request maupun response</remarks>
    public class GendersController : Controller
    {
        /// <summary>
        /// class level variable hanya dapat diatur dalam konstruktor dan tidak dapat diubah
        /// </summary>
        private readonly RentKendaraanContext _context;

        /// <summary>
        /// menerapkan akses readonly dalam method GendersContoller
        /// </summary>
        /// <param name="context">menginisiaisi variable readonly</param>
        public GendersController(RentKendaraanContext context)
        {
            _context = context;
        }

        /// <summary>
        /// GET: Genders
        /// </summary>
        /// <param name="ktsd">ketersediaan data genders</param>
        /// <param name="searchString">Membuat metode Search data</param>
        /// <param name="sortOrder">Membuat metode pengurutan data</param>
        /// <param name="currentFilter">Membuat filterisasi pada data</param>
        /// <param name="pageNumber">Membuat Pagination pada data yang ditampilkan</param>
        /// <returns>menampilkan semua data dari genders</returns>
        public async Task<IActionResult> Index(string ktsd, string searchString, string sortOrder, string currentFilter, int? pageNumber)
        {
            var ktsdList = new List<string>();
            var ktsdQuery = from d in _context.Genders orderby d.NamaGender select d.NamaGender;

            ktsdList.AddRange(ktsdQuery.Distinct());
            ViewBag.ktsd = new SelectList(ktsdList);

            var menu = from m in _context.Genders select m;

            if (!string.IsNullOrEmpty(ktsd))
            {
                menu = menu.Where(x => x.NamaGender == ktsd);
            }

            if (!string.IsNullOrEmpty(searchString))
            {
                menu = menu.Where(s => s.NamaGender.Contains(searchString));
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
                    menu = menu.OrderByDescending(s => s.NamaGender);
                    break;
                default:
                    menu = menu.OrderBy(s => s.NamaGender);
                    break;
            }

            return View(await PaginatedList<Gender>.CreateAsync(menu.AsNoTracking(), pageNumber ?? 1, pageSize));
        }

        /// <summary>
        /// GET: Genders/Details/5
        /// </summary>
        /// <param name="id">parameter ini digunakan untuk menangkap id yang di kirim dari http request</param>
        /// <returns>method yang digunakan untuk menampilakn detail data genders berdasarkan data yang dipilih</returns>
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var gender = await _context.Genders
                .FirstOrDefaultAsync(m => m.IdGender == id);
            if (gender == null)
            {
                return NotFound();
            }

            return View(gender);
        }

        /// <summary>
        /// GET: Genders/Create
        /// </summary>
        /// <returns>menampilkan data</returns>
        public IActionResult Create()
        {
            return View();
        }

        /// <summary>
        /// POST: Genders/Create
        /// </summary>
        /// <param name="gender">berguna untuk binding data dari database</param>
        /// <returns></returns>
        /// <remarks></remarks>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("IdGender,NamaGender")] Gender gender)
        {
            if (ModelState.IsValid)
            {
                _context.Add(gender);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(gender);
        }

        /// <summary>
        /// GET: Genders/Edit/5
        /// </summary>
        /// <param name="id"></param>
        /// <returns>menampilkan data genders berdasarkan id yang diminta</returns>
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var gender = await _context.Genders.FindAsync(id);
            if (gender == null)
            {
                return NotFound();
            }
            return View(gender);
        }

        /// <summary>
        /// POST: Genders/Edit/5
        /// </summary>
        /// <param name="id">parameter ini digunakan untuk menangkap id yang di kirim dari http request</param>
        /// <param name="gender">berguna untuk binding data dari database ke form</param>
        /// <returns>menampilkan hasil editing data genders</returns>
        /// <remarks>method yang di gunakan untuk edit/mengubah data sesuai id/data yang di pilih</remarks>
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("IdGender,NamaGender")] Gender gender)
        {
            if (id != gender.IdGender)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(gender);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!GenderExists(gender.IdGender))
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
            return View(gender);
        }

        /// <summary>
        /// GET: Genders/Delete/5
        /// </summary>
        /// <param name="id">parameter ini digunakan untuk menangkap id yang di kirim dari http request</param>
        /// <returns>menampilkan data customer berdasarkan id yang diminta</returns>
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var gender = await _context.Genders
                .FirstOrDefaultAsync(m => m.IdGender == id);
            if (gender == null)
            {
                return NotFound();
            }

            return View(gender);
        }

        /// <summary>
        /// Genders Delete Confirm
        /// </summary>
        /// <param name="id">parameter ini digunakan untuk menangkap id yang di kirim dari http request</param>
        /// <returns>mengembalikan ke halaman index</returns>
        /// <remarks>method yang di gunakan untuk menghapus/delete data sesuai id/data yang di pilih</remarks>
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var gender = await _context.Genders.FindAsync(id);
            _context.Genders.Remove(gender);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        /// <summary>
        /// cek ketersediaaan data Gender
        /// </summary>
        /// <param name="id">membuat variable id</param>
        /// <returns>menampilkan hasil pengecekan apakah tersedia id Gender</returns>
        /// <remarks>method ini digunakan untuk mengcek ketersediaan data gender berdasarkan id yang di tangkap dari database</remarks>
        private bool GenderExists(int id)
        {
            return _context.Genders.Any(e => e.IdGender == id);
        }
    }
}
