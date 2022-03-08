using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

#nullable disable

namespace RentalKendaraan.Models
{
    /// <summary>
    /// Class Partial Jaminan
    /// </summary>
    /// <remarks>implementasi fungsionalitas satu class ke semua file</remarks>
    public partial class Jaminan
    {
        /// <summary>
        /// Method Jaminan
        /// </summary>
        /// <remarks>untuk mendukung implementasi set dan menggunakan table hash milik class peminjaman</remarks>
        public Jaminan()
        {
            Peminjamen = new HashSet<Peminjaman>();
        }

        /// <summary>
        /// Auto-Property IdJaminan
        /// </summary>
        /// <remarks>dapat mengakses data dan info di field private dan biasanya melakukannya dari property public</remarks>
        public int IdJaminan { get; set; }

        /// <summary>
        /// auto property NamaJaminan bertipe String
        /// </summary>
        /// <remarks>agar dapat mengakses data dan info di field private dan biasanya melakukannya dari property public</remarks>
        [Required(ErrorMessage = "Jaminan tidak boleh kosong")]
        public string NamaJaminan { get; set; }

        /// <summary>
        /// method virtual Peminjaman
        /// </summary>
        /// <remarks>supaya dapat di akses oleh class lain sehingga dibuat auto property</remarks>
        public virtual ICollection<Peminjaman> Peminjamen { get; set; }
    }
}
