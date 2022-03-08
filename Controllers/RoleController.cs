using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;

namespace RentalKendaraan.Controllers
{
    /// <summary>
    /// Main class Controller Role 
    /// </summary>
    /// <remarks>class yang digunakan untuk membuat role</remarks>
    public class RoleController : Controller
    {
        /// <summary>
        /// Membangun instance baru dari RoleManager roleManager .
        /// </summary>
        RoleManager<IdentityRole> roleManager;

        /// <summary>
        /// inject rolemanager
        /// </summary>
        /// <param name="roleManager">instansiasi variable rolemanager </param>
        public RoleController(RoleManager<IdentityRole> roleManager)
        {
            this.roleManager = roleManager;
        }

        /// <summary>
        /// method index
        /// </summary>
        /// <returns>menampilkan semua hasil role yang ada</returns>
        /// <remarks>method yang menampilkan seluruh data role</remarks>
        public IActionResult Index()
        {
            var roles = roleManager.Roles.ToList();
            return View(roles);
        }

        /// <summary>
        /// method add/create
        /// </summary>
        /// <returns>menampilkan identity role</returns>
        public IActionResult Create()
        {
            return View(new IdentityRole());
        }

        /// <summary>
        /// post method add/create
        /// </summary>
        /// <param name="role">instansiasi dari method create</param>
        /// <returns>setelah di proses tambah data akan di kembalikan ke halaman index.</returns>
        /// <remarks>method yang berguna untuk add/tambah data role</remarks>
        [HttpPost]
        public async Task<IActionResult> Create(IdentityRole role)
        {
            await roleManager.CreateAsync(role);
            return RedirectToAction("Index");
        }
    }
}
