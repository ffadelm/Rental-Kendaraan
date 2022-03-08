using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

#nullable disable

namespace RentalKendaraan.Models
{
    /// <summary>
    /// Class Partial Jenis Kendaraan
    /// </summary>
    /// <remarks>implementasi fungsionalitas satu class ke semua file</remarks>
    public partial class JenisKendaraan
    {
        /// <summary>
        /// Method Jenis Kendaraan
        /// </summary>
        /// <remarks>untuk mendukung implementasi set dan menggunakan table hash milik class Kendaraan</remarks>
        public JenisKendaraan()
        {
            Kendaraans = new HashSet<Kendaraan>();
        }

        /// <summary>
        /// Auto-Property Id Jenis kendaraan
        /// </summary>
        /// <remarks>dapat mengakses data dan info di field private dan biasanya melakukannya dari property public</remarks>
        public int IdJenisKendaraan { get; set; }

        /// <summary>
        /// auto property Nama Jenis Kendaraan bertipe String
        /// </summary>
        /// <remarks>agar dapat mengakses data dan info di field private dan biasanya melakukannya dari property public</remarks>
        [Required(ErrorMessage = "Jenis Kendaraan tidak boleh kosong")]
        public string NamaJenisKendaraan { get; set; }

        /// <summary>
        /// Method virtual Kendaraan
        /// </summary>
        /// <remarks>supaya dapat di akses oleh class lain sehingga dibuat auto property</remarks>
        public virtual ICollection<Kendaraan> Kendaraans { get; set; }
    }
}
