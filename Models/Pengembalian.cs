using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

#nullable disable

namespace RentalKendaraan.Models
{
    /// <summary>
    /// class partial Pengembalian
    /// </summary>
    /// <remarks>implementasi fungsionalitas satu class ke semua file</remarks>
    public partial class Pengembalian
    {
        /// <summary>
        /// auto property ID Pengembalian
        /// </summary>
        /// <remarks>dapat mengakses data dan info di field private dan biasanya melakukannya dari property public</remarks>
        public int IdPengembalian { get; set; }

        /// <summary>
        /// nullable Date Time Id tanggal pengembalian auto property 
        /// </summary>
        /// <remarks>secara eksplisit memasukkan tipe nilai nullable ke tipe nilai none-nullable dalam bentuk date atau tanggal</remarks>
        public DateTime? TglPengembalian { get; set; }

        /// <summary>
        /// nullable int Id Peminjaman auto property 
        /// </summary>
        /// <remarks>secara eksplisit memasukkan tipe nilai nullable ke tipe nilai none-nullable</remarks>
        public int? IdPeminjaman { get; set; }

        /// <summary>
        /// nullable int Id Kondisi auto property 
        /// </summary>
        /// <remarks>secara eksplisit memasukkan tipe nilai nullable ke tipe nilai none-nullable</remarks>
        public int? IdKondisi { get; set; }

        /// <summary>
        /// nullable int Denda auto property 
        /// </summary>
        /// <remarks>secara eksplisit memasukkan tipe nilai nullable ke tipe nilai none-nullable</remarks>
        [Required(ErrorMessage = "Denda tidak boleh kosong")]
        public int? Denda { get; set; }

        /// <summary>
        /// method virtual Id Kondisi Kendaraan Navigation
        /// </summary>
        /// <remarks>method yang dapat didefinisikan ulang di kelas turunan, memiliki implementasi di kelas dasar serta menurunkan kelas</remarks>
        public virtual KondisiKendaraan IdKondisiNavigation { get; set; }

        /// <summary>
        /// method virtual Id Peminjaman Navigation
        /// </summary>
        /// <remarks>method yang dapat didefinisikan ulang di kelas turunan, memiliki implementasi di kelas dasar serta menurunkan kelas</remarks>
        public virtual Peminjaman IdPeminjamanNavigation { get; set; }
    }
}
