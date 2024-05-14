using Entities.Models;

namespace DataAccessLayer.Interfaces
{
    public interface IBarcodeRepository
    {
        /// <summary>
        /// Get all static barcodes from database
        /// </summary>
        /// <returns></returns>
        public List<AppCBStatic> GetAllCBStatic();

        /// <summary>
        /// Get all dynamic barcodes from database
        /// </summary>
        /// <returns></returns>
        public List<AppCBDynamic> GetAllCBDynamic();

    }
}
