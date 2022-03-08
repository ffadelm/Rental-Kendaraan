using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RentalKendaraan.Models
{
    /// <summary>
    /// main class Paginated List
    /// </summary>
    /// <typeparam name="T">parameter T sebagai List</typeparam>
    /// <remarks>digunakan sebagai class yang membuat pagination atau membuat halaman</remarks>
    public class PaginatedList<T> : List<T>
    {
        /// <summary>
        /// method bertipe int pageIndex
        /// </summary>
        /// <remarks>method yang di gunakan sebagai penomoran awal dari halaman</remarks>
        public int PageIndex { get; private set; }
        
        /// <summary>
        /// method bertipe int Total page
        /// </summary>
        /// <remarks>digunakan untuk mengetahui total dari halaman</remarks>
        public int TotalPages { get; private set; }

        /// <summary>
        /// Method Paginated list
        /// </summary>
        /// <param name="items">list item</param>
        /// <param name="count">angka yng menunjukan jumlah data</param>
        /// <param name="pageIndex">angka awal halaman</param>
        /// <param name="pageSize">angka mengetahui jumlah data perhalaman</param>
        /// <remarks>method yang digunakan untuk menghitung pagination atau membuat batas halamn di setiap data</remarks>
        public PaginatedList(List<T> items, int count, int pageIndex, int pageSize)
        {
            PageIndex = pageIndex;
            TotalPages = (int)Math.Ceiling(count / (double)pageSize);

            this.AddRange(items);
        }

        /// <summary>
        /// method previous page
        /// </summary>
        /// <remarks>digunakan untuk memberikan perhitungan kembali ke halaman sebelumnya</remarks>
        public bool HasPreviousPage
        {
            get
            {
                return (PageIndex > 1);
            }
        }

        /// <summary>
        /// method next page
        /// </summary>
        /// <remarks>digunakan untuk memberikan perhitungan next atau ke halaman sebelanjutnya</remarks>
        public bool HasNextPage
        {
            get
            {
                return (PageIndex < TotalPages);
            }
        }

        /// <summary>
        /// method awal data
        /// </summary>
        /// <param name="source">jumlah data</param>
        /// <param name="pageIndex">angka awal halaman</param>
        /// <param name="pageSize">angka mengetahui jumlah data perhalaman</param>
        /// <returns>menampilkan hasil perhitungan items</returns>
        /// <remarks>method yang digunakan untuk mengetahui awal data</remarks>
        public static async Task<PaginatedList<T>> CreateAsync(IQueryable<T> source, int pageIndex, int pageSize)
        {
            var count = await source.CountAsync();
            var items = await source.Skip((pageIndex - 1) * pageSize).Take(pageSize).ToListAsync();
            return new PaginatedList<T>(items, count, pageIndex, pageSize);
        }
    }
}
