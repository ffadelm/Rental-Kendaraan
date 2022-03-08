using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

#nullable disable

namespace RentalKendaraan.Models
{
    /// <summary>
    /// class partial Customer
    /// </summary>
    /// <remarks>implementasi fungsionalitas satu class ke semua file</remarks>
    public partial class Customer
    {
        /// <summary>
        /// method Customer
        /// </summary>
        /// <remarks>untuk mendukung implementasi set dan menggunakan table hash milik class Peminjaman</remarks>
        public Customer()
        {
            Peminjamen = new HashSet<Peminjaman>();
        }

        /// <summary>
        /// auto property ID Customer
        /// </summary>
        /// <remarks>dapat mengakses data dan info di field private dan biasanya melakukannya dari property public</remarks>
        public int IdCustomer { get; set; }

        /// <summary>
        /// auto property NamaCustomer bertipe String
        /// </summary>
        /// <remarks>dapat mengakses data dan info di field private dan biasanya melakukannya dari property public</remarks>
        [Required(ErrorMessage = "Nama Customer tidak boleh kosong")]
        public string NamaCustomer { get; set; }

        /// <summary>
        /// auto property Nik bertipe String
        /// </summary>
        /// <remarks>dapat mengakses data dan info di field private dan biasanya melakukannya dari property public</remarks>
        [RegularExpression("^[0-9]*$", ErrorMessage ="Hanya boleh diisi dengan angka")]
        public string Nik { get; set; }

        /// <summary>
        /// auto property Alamat bertipe String
        /// </summary>
        /// <remarks>supaya dapat mengakses data dan info di field private dan biasanya melakukannya dari property public</remarks>
        public string Alamat { get; set; }

        /// <summary>
        /// auto property NoHP bertipe String
        /// </summary>
        /// <remarks>agar dapat mengakses data dan info di field private dan biasanya melakukannya dari property public</remarks>
        [MinLength(10, ErrorMessage ="Isi minimal dengan 10 angka")]
        [MaxLength(13, ErrorMessage = "Maksimal hanya 10 angka")]
        public string NoHp { get; set; }

        /// <summary>
        /// nullable int IdGender auto property 
        /// </summary>
        /// <remarks>secara eksplisit memasukkan tipe nilai nullable ke tipe nilai none-nullable</remarks>
        public int? IdGender { get; set; }

        /// <summary>
        /// method virtual IdGenderNavigation
        /// </summary>
        /// <remarks>metode yang dapat didefinisikan ulang di kelas turunan, memiliki implementasi di kelas dasar serta menurunkan kelas</remarks>
        public virtual Gender IdGenderNavigation { get; set; }

        /// <summary>
        /// method virtual peminjaman
        /// </summary>
        /// <remarks>agar dapat di akses oleh class lain sehingga dibuat auto property</remarks>
        public virtual ICollection<Peminjaman> Peminjamen { get; set; }

    }
}
