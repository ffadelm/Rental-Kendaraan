using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

#nullable disable

namespace RentalKendaraan.Models
{
    /// <summary>
    /// 
    /// </summary>
    public partial class Gender
    {
        /// <summary>
        /// 
        /// </summary>
        public Gender()
        {
            Customers = new HashSet<Customer>();
        }

        /// <summary>
        /// 
        /// </summary>
        public int IdGender { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [MaxLength(1, ErrorMessage = "Isi dengan L: Laki-Laki & P: Perempuan")]
        [Required(ErrorMessage = "Jenis kelamin tidak boleh kosong")]
        public string NamaGender { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public virtual ICollection<Customer> Customers { get; set; }
    }
}
