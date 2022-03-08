using System;

namespace RentalKendaraan.Models
{
    /// <summary>
    /// main class ErrorViewModel
    /// </summary>
    /// <remarks>membuat class Handling Error </remarks>
    public class ErrorViewModel
    {
        /// <summary>
        /// method auto-property  Request Id
        /// </summary>
        /// <remarks>sebagai method untuk dapat di akses oleh field lain</remarks>
        public string RequestId { get; set; }

        /// <summary>
        /// method showRequestID
        /// </summary>
        /// <remarks>digunakan sebagai pengecekan pengkondisian apakah ad id yang Request atau tidak</remarks>
        public bool ShowRequestId => !string.IsNullOrEmpty(RequestId);
    }
}
