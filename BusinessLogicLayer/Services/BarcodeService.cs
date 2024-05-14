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
    }
}
