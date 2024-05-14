using Entities.Models;

namespace BusinessLogicLayer.Interfaces
{
    public interface IBarcodeService
    {
        /// <summary>
        /// Get all static barcodes
        /// </summary>
        /// <returns></returns>
        public List<AppCBStatic> GetAllCBStatic();

        /// <summary>
        /// Get all dynamic barcodes
        /// </summary>
        /// <returns></returns>
        public List<AppCBDynamic> GetAllCBDynamic();
    }
}
