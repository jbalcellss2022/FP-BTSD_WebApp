using DataAccessLayer.Interfaces;
using Entities.Data;
using Entities.Models;

namespace DataAccessLayer.Repositories
{
    public class BarcodeRepository(BBDDContext bbddcontext): IBarcodeRepository
    {
        public List<AppCBStatic> GetAllCBStatic()
        {
            var cbStatic = bbddcontext.AppCBStatics
                .ToList();
            return cbStatic;
        }

        public List<AppCBDynamic> GetAllCBDynamic()
        {
            var cbStatic = bbddcontext.AppCBDynamics
                .ToList();
            return cbStatic;
        }
    }
}
