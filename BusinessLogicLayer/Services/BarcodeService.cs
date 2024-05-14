using BusinessLogicLayer.Interfaces;
using DataAccessLayer.Interfaces;
using Entities.Models;

namespace BusinessLogicLayer.Services
{
    public class BarcodeService(IBarcodeRepository BarcodeRepository) : IBarcodeService
    {
        public List<AppCBStatic> GetAllCBStatic()
        {
            return BarcodeRepository.GetAllCBStatic();
        }

        public List<AppCBDynamic> GetAllCBDynamic()
        {
            return BarcodeRepository.GetAllCBDynamic();
        }

        public async Task<AppCBStatic> GetCBStaticByCode(string code)
        {
            return await BarcodeRepository.GetCBStaticByCode(code);
        }

        public async Task<AppCBDynamic> GetCBDynamicByCode(string code)
        {
            return await BarcodeRepository.GetCBDynamicByCode(code);
        }

        public async Task<AppCBStatic> GetCBStaticById(int id)
        {
            return await BarcodeRepository.GetCBStaticById(id);
        }

        public async Task<AppCBDynamic> GetCBDynamicById(int id)
        {
            return await BarcodeRepository.GetCBDynamicById(id);
        }

        public async Task<bool> DeleteCBStaticById(int id)
        {
            return await BarcodeRepository.DeleteCBStaticById(id);
        }

        public async Task<bool> DeleteCBDynamicById(int id)
        {
            return await BarcodeRepository.DeleteCBDynamicById(id);
        }

        public async Task<bool> AddCBStatic(AppCBStatic cbStatic)
        {
            return await BarcodeRepository.AddCBStatic(cbStatic);
        }

        public async Task<bool> AddCBDynamic(AppCBDynamic cbDynamic)
        {
            return await BarcodeRepository.AddCBDynamic(cbDynamic);
        }

        public async Task<bool> UpdateCBStatic(AppCBStatic cbStatic)
        {
            return await BarcodeRepository.UpdateCBStatic(cbStatic);
        }

        public async Task<bool> UpdateCBDynamic(AppCBDynamic cbDynamic)
        {
            return await BarcodeRepository.UpdateCBDynamic(cbDynamic);
        }

        public async Task<bool> DeleteCBDynamic(AppCBDynamic cbDynamic)
        {
            return await BarcodeRepository.DeleteCBDynamic(cbDynamic);
        }

        public async Task<bool> DeleteCBStatic(AppCBStatic cbStatic)
        {
            return await BarcodeRepository.DeleteCBStatic(cbStatic);
        }
    }
}
