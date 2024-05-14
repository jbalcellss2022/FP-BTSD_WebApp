using Entities.Models;

namespace BusinessLogicLayer.Interfaces
{
    public interface IBarcodeService
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

        /// <summary>
        /// Get a dynamic barcode by its code
        /// </summary>
        /// <param name="code"></param>
        /// <returns></returns>
        Task<AppCBDynamic> GetCBDynamicByCode(string code);

        /// <summary>
        /// Get a static barcode by its code
        /// </summary>
        /// <param name="code"></param>
        /// <returns></returns>
        Task<AppCBStatic> GetCBStaticByCode(string code);

        /// <summary>
        /// Get a dynamic barcode by its id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<AppCBDynamic> GetCBDynamicById(int id);

        /// <summary>
        /// Get a static barcode by its id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<AppCBStatic> GetCBStaticById(int id);

        /// <summary>
        /// Delete a dynamic barcode by its id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<bool> DeleteCBDynamicById(int id);

        /// <summary>
        /// Delete a static barcode by its id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<bool> DeleteCBStaticById(int id);

        /// <summary>
        /// Delete a dynamic barcode
        /// </summary>
        /// <param name="cbDynamic"></param>
        /// <returns></returns>
        Task<bool> DeleteCBDynamic(AppCBDynamic cbDynamic);

        /// <summary>
        /// Delete a static barcode
        /// </summary>
        /// <param name="cbStatic"></param>
        /// <returns></returns>
        Task<bool> DeleteCBStatic(AppCBStatic cbStatic);

        /// <summary>
        /// Update a dynamic barcode
        /// </summary>
        /// <param name="cbDynamic"></param>
        /// <returns></returns>
        Task<bool> UpdateCBDynamic(AppCBDynamic cbDynamic);

        /// <summary>
        /// Update a static barcode    
        /// </summary>
        /// <param name="cbStatic"></param>
        /// <returns></returns>
        Task<bool> UpdateCBStatic(AppCBStatic cbStatic);

        /// <summary>
        /// Add a dynamic barcode
        /// </summary>
        /// <param name="cbDynamic"></param>
        /// <returns></returns>
        Task<bool> AddCBDynamic(AppCBDynamic cbDynamic);

        /// <summary>
        /// Add a static barcode
        /// </summary>
        /// <param name="cbStatic"></param>
        /// <returns></returns>
        Task<bool> AddCBStatic(AppCBStatic cbStatic);
    }
}
