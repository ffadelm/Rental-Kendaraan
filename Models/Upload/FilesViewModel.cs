using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RentalKendaraan.Models.Upload
{
    /// <summary>
    /// class file details
    /// </summary>
    /// <remarks>digunakan untuk membuat directory path file yang di upload</remarks>
    public class FileDetails
    {
        /// <summary>
        /// auto property Name bertipe String
        /// </summary>
        /// <remarks>Name untuk di file agar dapat mengakses data dan info di field private dan biasanya melakukannya dari property public</remarks>
        public string Name { get; set; }

        /// <summary>
        /// auto property path bertipe String
        /// </summary>
        /// <remarks>path directory untuk di file agar dapat mengakses data dan info di field private dan biasanya melakukannya dari property public</remarks>
        public string Path { get; set; }
    }

    /// <summary>
    /// class files view model
    /// </summary>
    /// <remarks>instansiasi property Files</remarks>
    public class FilesViewModel
    {
        public List<FileDetails> Files { get; set; }
            = new List<FileDetails>();
    }
}
