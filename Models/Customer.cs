using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

#nullable disable

namespace RentalKendaraan.Models
{
    /// <summary>
    /// 
    /// </summary>
    public partial class Customer
    {
        /// <summary>
        /// 
        /// </summary>
        public Customer()
        {
            Peminjamen = new HashSet<Peminjaman>();

        }

        /// <summary>
        /// 
        /// </summary>
        public int IdCustomer { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [Required(ErrorMessage = "Nama Customer tidak boleh kosong")]
        public string NamaCustomer { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [RegularExpression("^[0-9]*$", ErrorMessage ="Hanya boleh diisi dengan angka")]
        public string Nik { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string Alamat { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [MinLength(10, ErrorMessage ="Isi minimal dengan 10 angka")]
        [MaxLength(13, ErrorMessage = "Maksimal hanya 10 angka")]
        public string NoHp { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public int? IdGender { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public virtual Gender IdGenderNavigation { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public virtual ICollection<Peminjaman> Peminjamen { get; set; }

    }
}
