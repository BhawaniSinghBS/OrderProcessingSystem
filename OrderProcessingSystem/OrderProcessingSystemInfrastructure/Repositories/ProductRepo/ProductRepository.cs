using Microsoft.EntityFrameworkCore;
using OrderProcessingSystem.Shared.Constants;
using OrderProcessingSystemInfrastructure.DataBase;
using OrderProcessingSystemInfrastructure.DataBase.Entities;
using Serilog;
using System.Reflection;

namespace OrderProcessingSystemInfrastructure.Repositories.ProductRepo
{
    public class ProductRepository : IProductRepository
    {
        private readonly DataBaseContext _context;

        public ProductRepository(DataBaseContext context)
        {
            _context = context;
        }

        public async Task<List<ProductEntity>> GetAllAsync()
        {
            try
            {
                return await _context.Set<ProductEntity>().ToListAsync();
            }
            catch (Exception ex)
            {
                string className = MethodBase.GetCurrentMethod()?.DeclaringType?.Name ?? $"{TextMessages.UnknownClassText}";
                string methodName = MethodBase.GetCurrentMethod()?.Name ?? $"{TextMessages.UnknownMethodText}";
                string exLocationAndMessage = $"{TextMessages.ClassNameText} : {className}  -- {TextMessages.FunctionNameText} : {methodName} ---- {ex.Message} ------";
                Log.Error(ex, exLocationAndMessage);
                return new();
            }
        }

        public async Task<ProductEntity> GetByIdAsync(int id)
        {
            try
            {
                return await _context.Set<ProductEntity>().FindAsync(id);
            }
            catch (Exception ex)
            {
                string className = MethodBase.GetCurrentMethod()?.DeclaringType?.Name ?? $"{TextMessages.UnknownClassText}";
                string methodName = MethodBase.GetCurrentMethod()?.Name ?? $"{TextMessages.UnknownMethodText}";
                string exLocationAndMessage = $"{TextMessages.ClassNameText} : {className}  -- {TextMessages.FunctionNameText} : {methodName} ---- {ex.Message} ------";
                Log.Error(ex, exLocationAndMessage);
                return new();
            }
        }

        public async Task<bool> AddAsync(ProductEntity product)
        {
            try
            {
                await _context.Set<ProductEntity>().AddAsync(product);
                return await _context.SaveChangesAsync() > 0;
            }
            catch (Exception ex)
            {
                string className = MethodBase.GetCurrentMethod()?.DeclaringType?.Name ?? $"{TextMessages.UnknownClassText}";
                string methodName = MethodBase.GetCurrentMethod()?.Name ?? $"{TextMessages.UnknownMethodText}";
                string exLocationAndMessage = $"{TextMessages.ClassNameText} : {className}  -- {TextMessages.FunctionNameText} : {methodName} ---- {ex.Message} ------";
                Log.Error(ex, exLocationAndMessage);
                return false;
            }
        }

        public async Task<bool> UpdateAsync(ProductEntity product)
        {
            try
            {
                _context.Set<ProductEntity>().Update(product);
                return await _context.SaveChangesAsync() > 0;
            }
            catch (Exception ex)
            {
                string className = MethodBase.GetCurrentMethod()?.DeclaringType?.Name ?? $"{TextMessages.UnknownClassText}";
                string methodName = MethodBase.GetCurrentMethod()?.Name ?? $"{TextMessages.UnknownMethodText}";
                string exLocationAndMessage = $"{TextMessages.ClassNameText} : {className}  -- {TextMessages.FunctionNameText} : {methodName} ---- {ex.Message} ------";
                Log.Error(ex, exLocationAndMessage);
                return false;
            }
        }

        public async Task<bool> DeleteAsync(int id)
        {
            try
            {
                var product = await GetByIdAsync(id);
                if (product != null)
                {
                    _context.Set<ProductEntity>().Remove(product);
                    return await _context.SaveChangesAsync() > 0;
                }
                return false;
            }
            catch (Exception ex)
            {
                string className = MethodBase.GetCurrentMethod()?.DeclaringType?.Name ?? $"{TextMessages.UnknownClassText}";
                string methodName = MethodBase.GetCurrentMethod()?.Name ?? $"{TextMessages.UnknownMethodText}";
                string exLocationAndMessage = $"{TextMessages.ClassNameText} : {className}  -- {TextMessages.FunctionNameText} : {methodName} ---- {ex.Message} ------";
                Log.Error(ex, exLocationAndMessage);
                return false;

            }
        }
    }
}
