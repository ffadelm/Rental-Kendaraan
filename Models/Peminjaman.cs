using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

#nullable disable

namespace RentalKendaraan.Models
{
    /// <summary>
    /// class partial Peminjaman
    /// </summary>
    /// <remarks>implementasi fungsionalitas satu class ke semua file</remarks>
    public partial class Peminjaman
    {
        /// <summary>
        /// method Peminjaman
        /// </summary>
        /// <remarks>untuk mendukung implementasi set dan menggunakan table hash milik class Pengembalian</remarks>
        public Peminjaman()
        {
            Pengembalians = new HashSet<Pengembalian>();
        }

        /// <summary>
        /// auto property ID Peminjaman
        /// </summary>
        /// <remarks>dapat mengakses data dan info di field private dan biasanya melakukannya dari property public</remarks>
        public int IdPeminjaman { get; set; }

        /// <summary>
        /// nullable Date Time Id tanggal peminjaman auto property 
        /// </summary>
        /// <remarks>secara eksplisit memasukkan tipe nilai nullable ke tipe nilai none-nullable dalam bentuk date atau tanggal</remarks>
        public DateTime? TglPeminjaman { get; set; }

        /// <summary>
        /// nullable int Id Kendaraan auto property 
        /// </summary>
        /// <remarks>secara eksplisit memasukkan tipe nilai nullable ke tipe nilai none-nullable</remarks>
        public int? IdKendaraan { get; set; }

        /// <summary>
        /// nullable int Id Customer auto property 
        /// </summary>
        /// <remarks>secara eksplisit memasukkan tipe nilai nullable ke tipe nilai none-nullable</remarks>
        public int? IdCustomer { get; set; }

        /// <summary>
        /// nullable int Id Jaminan auto property 
        /// </summary>
        /// <remarks>secara eksplisit memasukkan tipe nilai nullable ke tipe nilai none-nullable</remarks>
        public int? IdJaminan { get; set; }

        /// <summary>
        /// nullable int Id Biaya auto property 
        /// </summary>
        /// <remarks>secara eksplisit memasukkan tipe nilai nullable ke tipe nilai none-nullable</remarks>
        [Required(ErrorMessage = "Biaya tidak boleh kosong")]
        public int? Biaya { get; set; }

        /// <summary>
        /// method virtual Id Customer Navigation
        /// </summary>
        /// <remarks>method yang dapat didefinisikan ulang di kelas turunan, memiliki implementasi di kelas dasar serta menurunkan kelas</remarks>
        public virtual Customer IdCustomerNavigation { get; set; }

        /// <summary>
        /// method virtual Id Jaminan Navigation
        /// </summary>
        /// <remarks>method yang dapat didefinisikan ulang di kelas turunan, memiliki implementasi di kelas dasar serta menurunkan kelas</remarks>
        public virtual Jaminan IdJaminanNavigation { get; set; }

        /// <summary>
        /// method virtual Id Kendaraan Navigation
        /// </summary>
        /// <remarks>method yang dapat didefinisikan ulang di kelas turunan, memiliki implementasi di kelas dasar serta menurunkan kelas</remarks>
        public virtual Kendaraan IdKendaraanNavigation { get; set; }

        /// <summary>
        /// method virtual pengembalian
        /// </summary>
        /// <remarks>agar dapat di akses oleh class lain sehingga dibuat auto property</remarks>
        public virtual ICollection<Pengembalian> Pengembalians { get; set; }
    }
}
