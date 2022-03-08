using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

#nullable disable

namespace RentalKendaraan.Models
{
    /// <summary>
    /// Class Partial Kondisi Kendaraan
    /// </summary>
    /// <remarks>implementasi fungsionalitas satu class ke semua file</remarks>
    public partial class KondisiKendaraan
    {
        /// <summary>
        /// Method Kondisi Kendaraan
        /// </summary>
        /// <remarks>untuk mendukung implementasi set dan menggunakan table hash milik class pengembalian</remarks>
        public KondisiKendaraan()
        {
            Pengembalians = new HashSet<Pengembalian>();
        }

        /// <summary>
        /// Auto-Property Id Kondisi
        /// </summary>
        /// <remarks>dapat mengakses data dan info di field private dan biasanya melakukannya dari property public</remarks>
        public int IdKondisi { get; set; }

        /// <summary>
        /// auto property Nama Kondisi bertipe String
        /// </summary>
        /// <remarks>agar dapat mengakses data dan info di field private dan biasanya melakukannya dari property public</remarks>
        [Required(ErrorMessage = "Kondisi Kendaraan tidak boleh kosong")]
        public string NamaKondisi { get; set; }

        /// <summary>
        /// Method virtual Pengembalian
        /// </summary>
        /// <remarks>supaya dapat di akses oleh class lain sehingga dibuat auto property</remarks>
        public virtual ICollection<Pengembalian> Pengembalians { get; set; }
    }
}
