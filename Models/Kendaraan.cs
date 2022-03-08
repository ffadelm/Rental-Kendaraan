using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

#nullable disable

namespace RentalKendaraan.Models
{
    /// <summary>
    /// class partial Kendaraan
    /// </summary>
    /// <remarks>implementasi fungsionalitas satu class ke semua file</remarks>
    public partial class Kendaraan
    {
        /// <summary>
        /// method Kendaraan
        /// </summary>
        /// <remarks>untuk mendukung implementasi set dan menggunakan table hash milik class Peminjaman</remarks>
        public Kendaraan()
        {
            Peminjamen = new HashSet<Peminjaman>();
        }

        /// <summary>
        /// auto property ID Kendaraan
        /// </summary>
        /// <remarks>dapat mengakses data dan info di field private dan biasanya melakukannya dari property public</remarks>
        public int IdKendaraan { get; set; }

        /// <summary>
        /// auto property Nama Kendaraan bertipe String
        /// </summary>
        /// <remarks>dapat mengakses data dan info di field private dan biasanya melakukannya dari property public</remarks>
        [Required(ErrorMessage = "Nama Kendaraan tidak boleh kosong")]
        public string NamaKendaraan { get; set; }

        /// <summary>
        /// auto property Nomor Polisi bertipe String
        /// </summary>
        /// <remarks>agar dapat mengakses data dan info di field private dan biasanya melakukannya dari property public</remarks>
        public string NoPolisi { get; set; }

        /// <summary>
        /// auto property Nomor STNK bertipe String
        /// </summary>
        /// <remarks>supaya dapat mengakses data dan info di field private dan biasanya melakukannya dari property public</remarks>
        [RegularExpression("^[0-9]*$", ErrorMessage = "Hanya boleh diisi dengan angka")]
        public string NoStnk { get; set; }

        /// <summary>
        /// nullable int Id Jenis Kendaraan auto property 
        /// </summary>
        /// <remarks>secara eksplisit memasukkan tipe nilai nullable ke tipe nilai none-nullable</remarks>
        public int? IdJenisKendaraan { get; set; }

        /// <summary>
        /// auto property Ketersediaan bertipe String
        /// </summary>
        /// <remarks>untuk dapat mengakses data dan info di field private dan biasanya melakukannya dari property public</remarks>
        public string Ketersediaan { get; set; }

        /// <summary>
        /// method virtual Id Jenis Kendaraan Navigation
        /// </summary>
        /// <remarks>metode yang dapat didefinisikan ulang di kelas turunan, memiliki implementasi di kelas dasar serta menurunkan kelas</remarks>
        public virtual JenisKendaraan IdJenisKendaraanNavigation { get; set; }

        /// <summary>
        /// method virtual peminjaman
        /// </summary>
        /// <remarks>agar dapat di akses oleh class lain sehingga dibuat auto property</remarks>
        public virtual ICollection<Peminjaman> Peminjamen { get; set; }
    }
}
