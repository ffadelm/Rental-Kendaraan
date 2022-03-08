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
    /// main class Customer Controller
    /// </summary>
    /// <remarks>Controller di gunakan untuk menerima memproses dan mengirim Http request maupun response</remarks>
    public class CustomersController : Controller
    {
        /// <summary>
        /// class level variable hanya dapat diatur dalam konstruktor dan tidak dapat diubah
        /// </summary>
        private readonly RentKendaraanContext _context;

        /// <summary>
        /// menerapkan akses readonly dalam method CustomersContoller
        /// </summary>
        /// <param name="context">menginisiaisi variable readonly</param>
        public CustomersController(RentKendaraanContext context)
        {
            _context = context;
        }


        /// <summary>
        /// GET: Customers
        /// </summary>
        /// <param name="ktsd">ketersediaan data customers</param>
        /// <param name="searchString">Membuat metode Search data</param>
        /// <param name="sortOrder">Membuat metode pengurutan data</param>
        /// <param name="currentFilter">Membuat filterisasi pada data</param>
        /// <param name="pageNumber">Membuat Pagination pada data yang ditampilkan</param>
        /// <returns>menampilkan semua data dari customers</returns>
        public async Task<IActionResult> Index(string ktsd, string searchString, string sortOrder, string currentFilter, int? pageNumber)
        {
            var ktsdList = new List<string>();
            var ktsdQuery = from d in _context.Customers orderby d.NamaCustomer select d.NamaCustomer;

            ktsdList.AddRange(ktsdQuery.Distinct());
            ViewBag.ktsd = new SelectList(ktsdList);

            var menu = from m in _context.Customers.Include(k => k.IdGenderNavigation) select m;

            if (!string.IsNullOrEmpty(ktsd))
            {
                menu = menu.Where(x => x.NamaCustomer == ktsd);
            }

            if (!string.IsNullOrEmpty(searchString))
            {
                menu = menu.Where(s => s.NamaCustomer.Contains(searchString) || s.Nik.Contains(searchString) || s.Alamat.Contains(searchString) || 
                s.NoHp.Contains(searchString) || s.IdGenderNavigation.NamaGender.Contains(searchString));
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
            ViewData["GenderSortParm"] = sortOrder == "Gender" ? "gender_desc" : "Gender";

            switch (sortOrder)
            {
                case "name_desc":
                    menu = menu.OrderByDescending(s => s.NamaCustomer);
                    break;
                case "Gender":
                    menu = menu.OrderBy(s => s.IdGenderNavigation.NamaGender);
                    break;
                case "gender_desc":
                    menu = menu.OrderByDescending(s => s.IdGenderNavigation.NamaGender);
                    break;
                default:
                    menu = menu.OrderBy(s => s.NamaCustomer);
                    break;
            }

            return View(await PaginatedList<Customer>.CreateAsync(menu.AsNoTracking(), pageNumber ?? 1, pageSize));
        }

        /// <summary>
        /// GET: Customers/Details/5
        /// </summary>
        /// <param name="id">parameter ini digunakan untuk menangkap id yang di kirim dari http request</param>
        /// <returns>menampilkan detail data customer berdasarkan id yang diminta</returns>
        /// <remarks>method yang digunakan untuk menampilakn detail data customer berdasarkan data yang dipilih</remarks>
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var customer = await _context.Customers
                .Include(c => c.IdGenderNavigation)
                .FirstOrDefaultAsync(m => m.IdCustomer == id);
            if (customer == null)
            {
                return NotFound();
            }

            return View(customer);
        }

        /// <summary>
        /// GET: Customers/Create
        /// </summary>
        /// <returns>menampilkan data id Gender</returns>
        public IActionResult Create()
        {
            ViewData["IdGender"] = new SelectList(_context.Genders, "IdGender", "IdGender");
            return View();
        }

        /// <summary>
        /// POST: Customers/Create
        /// </summary>
        /// <param name="customer">berguna untuk binding data dari database</param>
        /// <returns>menampilkan data hasil input data</returns>
        /// <remarks>method yang di gunakan untuk add/menambahkan data baru</remarks>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("IdCustomer,NamaCustomer,Nik,Alamat,NoHp,IdGender")] Customer customer)
        {
            if (ModelState.IsValid)
            {
                _context.Add(customer);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["IdGender"] = new SelectList(_context.Genders, "IdGender", "IdGender", customer.IdGender);
            return View(customer);
        }

        /// <summary>
        /// GET: Customers/Edit/5
        /// </summary>
        /// <param name="id">parameter ini digunakan untuk menangkap id yang di kirim dari http request</param>
        /// <returns>menampilkan data serta id gender berdasarkan id yang diminta</returns>
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var customer = await _context.Customers.FindAsync(id);
            if (customer == null)
            {
                return NotFound();
            }
            ViewData["IdGender"] = new SelectList(_context.Genders, "IdGender", "IdGender", customer.IdGender);
            return View(customer);
        }

        /// <summary>
        /// POST: Customers/Edit/5
        /// </summary>
        /// <param name="id">parameter ini digunakan untuk menangkap id yang di kirim dari http request</param>
        /// <param name="customer">berguna untuk binding data dari database ke form</param>
        /// <returns>menampilkan hasil editing data customer</returns>
        /// <remarks>method yang di gunakan untuk edit/mengubah data sesuai id/data yang di pilih</remarks>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("IdCustomer,NamaCustomer,Nik,Alamat,NoHp,IdGender")] Customer customer)
        {
            if (id != customer.IdCustomer)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(customer);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!CustomerExists(customer.IdCustomer))
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
            ViewData["IdGender"] = new SelectList(_context.Genders, "IdGender", "IdGender", customer.IdGender);
            return View(customer);
        }

        /// <summary>
        /// GET: Customers/Delete/5
        /// </summary>
        /// <param name="id">parameter ini digunakan untuk menangkap id yang di kirim dari http request</param>
        /// <returns>menampilkan data customer berdasarkan id yang diminta</returns>
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var customer = await _context.Customers
                .Include(c => c.IdGenderNavigation)
                .FirstOrDefaultAsync(m => m.IdCustomer == id);
            if (customer == null)
            {
                return NotFound();
            }

            return View(customer);
        }

        /// <summary>
        /// Customers Delete Confirm
        /// </summary>
        /// <param name="id">parameter ini digunakan untuk menangkap id yang di kirim dari http request</param>
        /// <returns>mengembalikan ke halaman index</returns>
        /// <remarks>method yang di gunakan untuk menghapus/delete data sesuai id/data yang di pilih</remarks>
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var customer = await _context.Customers.FindAsync(id);
            _context.Customers.Remove(customer);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        /// <summary>
        /// cek ketersediaaan data customer
        /// </summary>
        /// <param name="id">membuat variable id</param>
        /// <returns>menampilkan hasil pengecekan apakah tersedia id customer</returns>
        /// <remarks>method ini digunakan untuk mengcek ketersediaan data customer berdasarkan id yang di tangkap dari database</remarks>
        private bool CustomerExists(int id)
        {
            return _context.Customers.Any(e => e.IdCustomer == id);
        }
    }
}
