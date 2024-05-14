using DataAccessLayer.Interfaces;
using Entities.Data;
using Entities.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System.Diagnostics;

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

        public async Task<bool> AddCBStatic(AppCBStatic cbStatic)
        {
            bool result = false;
            try
            {
                bbddcontext.AppCBStatics.Add(cbStatic);
                await bbddcontext.SaveChangesAsync();
                result = true;
            }
            catch (Exception)
            {
            }

            return result;
        }

        public async Task<bool> AddCBDynamic(AppCBDynamic cbDynamic)
        {
            bool result = false;
            try
            {
                bbddcontext.AppCBDynamics.Add(cbDynamic);
                await bbddcontext.SaveChangesAsync();
                result = true;
            }
            catch (Exception)
            {
            }

            return result;
        }

        public async Task<bool> UpdateCBStatic(AppCBStatic cbStatic)
        {
            bool result = false;
            try
            {
                bbddcontext.AppCBStatics.Update(cbStatic);
                await bbddcontext.SaveChangesAsync();
                result = true;
            }
            catch (Exception)
            {
            }

            return result;
        }

        public async Task<bool> UpdateCBDynamic(AppCBDynamic cbDynamic)
        {
            bool result = false;
            try
            {
                bbddcontext.AppCBDynamics.Update(cbDynamic);
                await bbddcontext.SaveChangesAsync();
                result = true;
            }
            catch (Exception)
            {
            }

            return result;
        }

        public async Task<bool> DeleteCBStatic(AppCBStatic cbStatic)
        {
            bool result = false;
            try
            {
                bbddcontext.AppCBStatics.Remove(cbStatic);
                await bbddcontext.SaveChangesAsync();
                result = true;
            }
            catch (Exception)
            {
            }

            return result;
        }

        public async Task<bool> DeleteCBDynamic(AppCBDynamic cbDynamic)
        {
            bool result = false;
            try
            {
                bbddcontext.AppCBDynamics.Remove(cbDynamic);
                await bbddcontext.SaveChangesAsync();
                result = true;
            }
            catch (Exception)
            {
            }

            return result;
        }

        public async Task<bool> DeleteCBStaticById(int id)
        {
            bool result = false;
            try
            {
                var cbStatic = bbddcontext.AppCBStatics
                    .Where(x => x.Id == id)
                    .FirstOrDefault();
                if (cbStatic != null)
                {
                    bbddcontext.AppCBStatics.Remove(cbStatic);
                    await bbddcontext.SaveChangesAsync();
                    result = true;
                }
            }
            catch (Exception)
            {
            }

            return result;
        }

        public async Task<bool> DeleteCBDynamicById(int id)
        {
            bool result = false;
            try
            {
                var cbDynamic = bbddcontext.AppCBDynamics
                    .Where(x => x.Id == id)
                    .FirstOrDefault();
                if (cbDynamic != null)
                {
                    bbddcontext.AppCBDynamics.Remove(cbDynamic);
                    await bbddcontext.SaveChangesAsync();
                    result = true;
                }
            }
            catch (Exception)
            {
            }

            return result;
        }

        public async Task<AppCBStatic> GetCBStaticById(int id)
        {
            var cbStatic = await bbddcontext.AppCBStatics
                .Where(x => x.Id == id)
                .FirstOrDefaultAsync();
            return cbStatic!;
        }

        public async Task<AppCBDynamic> GetCBDynamicById(int id)
        {
            var cbDynamic = await bbddcontext.AppCBDynamics
                .Where(x => x.Id == id)
                .FirstOrDefaultAsync();
            return cbDynamic!;
        }

        public async Task<AppCBStatic> GetCBStaticByCode(string code)
        {
            var cbStatic = await bbddcontext.AppCBStatics
                .Where(x => x.CBValue == code)
                .FirstOrDefaultAsync();
            return cbStatic!;
        }

        public async Task<AppCBDynamic> GetCBDynamicByCode(string code)
        {
            var cbDynamic = await bbddcontext.AppCBDynamics
                .Where(x => x.CBValue == code)
                .FirstOrDefaultAsync();
            return cbDynamic!;
        }

    }
}
