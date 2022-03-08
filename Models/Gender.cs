using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

#nullable disable

namespace RentalKendaraan.Models
{
    /// <summary>
    /// class partial Gender
    /// </summary>
    /// <remarks>implementasi fungsionalitas satu class ke semua file</remarks>
    public partial class Gender
    {
        /// <summary>
        /// method Gender
        /// </summary>
        /// <remarks>untuk mendukung implementasi set dan menggunakan table hash milik class Customer</remarks>
        public Gender()
        {
            Customers = new HashSet<Customer>();
        }

        /// <summary>
        /// Auto-Property IdGender
        /// </summary>
        /// <remarks>dapat mengakses data dan info di field private dan biasanya melakukannya dari property public</remarks>
        public int IdGender { get; set; }

        /// <summary>
        /// auto property NamaGender bertipe String
        /// </summary>
        /// <remarks>agar dapat mengakses data dan info di field private dan biasanya melakukannya dari property public</remarks>
        [MaxLength(1, ErrorMessage = "Isi dengan L: Laki-Laki & P: Perempuan")]
        [Required(ErrorMessage = "Jenis kelamin tidak boleh kosong")]
        public string NamaGender { get; set; }

        /// <summary>
        /// method virtual Customer
        /// </summary>
        /// <remarks>agar dapat di akses oleh class lain sehingga dibuat auto property</remarks>
        public virtual ICollection<Customer> Customers { get; set; }
    }
}
