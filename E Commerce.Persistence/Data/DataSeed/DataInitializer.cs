using E_Commerce.Domain.Contracts;
using E_Commerce.Domain.Entities;
using E_Commerce.Domain.Entities.ProductModule;
using E_Commerce.Persistence.Data.DbContexts;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace E_Commerce.Persistence.Data.DataSeed
{
    public class DataInitializer : IDataInitializer
    {
        private readonly StoreDbContext _dbContext;

        public DataInitializer(StoreDbContext dbContext)
        {
            _dbContext = dbContext;
        }
        public async Task InitializeAsync()
        {
            try
            {
                var HasProducts = await _dbContext.Products.AnyAsync();
                var HasBrands = await _dbContext.ProductBrands.AnyAsync();
                var HasTypes = await _dbContext.ProductTypes.AnyAsync();

                if (HasProducts && HasBrands && HasTypes)
                {
                    // Data already exists, no need to seed
                    return;
                }
                if (!HasBrands)
                    await SeedDataFromJSONAsync<ProductBrand, int>("brands.json", _dbContext.ProductBrands);

                if (!HasTypes)
                    await SeedDataFromJSONAsync<ProductType, int>("types.json", _dbContext.ProductTypes);

                await _dbContext.SaveChangesAsync(); // Save changes after seeding brands and types to ensure foreign key constraints are met when seeding products

                if (!HasProducts)
                    await SeedDataFromJSONAsync<Product, int>("products.json", _dbContext.Products);
                await _dbContext.SaveChangesAsync(); // Save changes after seeding products

            }
            catch (Exception ex)
            {
                // Log the exception or handle it as needed
                Console.WriteLine($"Data Seeding Failed: {ex}");
            }
        }

        private async Task SeedDataFromJSONAsync<T, TKey>(string fileName, DbSet<T> dbset) where T : BaseEntity<TKey>
        {
            //D:\Route_2025\09 ASP.NET Core Web APIs\ECommerce.WebAPI\ECommerce.Web Solution\E Commerce.Persistence\Data\DataSeed\JSONFiles\

            var FilePath = @"..\E Commerce.Persistence\Data\DataSeed\JSONFiles\" + fileName;

            if (!File.Exists(FilePath)) throw new FileNotFoundException($"The file {fileName} was not found at path {FilePath}.");


            try
            {
                using var dataStream = File.OpenRead(FilePath);
                var data = await JsonSerializer.DeserializeAsync<List<T>>(dataStream, new JsonSerializerOptions()
                {
                    PropertyNameCaseInsensitive = true
                });

                if (data is not null)
                {
                    await dbset.AddRangeAsync(data);
                }
            }
            catch (Exception ex)
            {
                // Log the exception or handle it as needed
                Console.WriteLine($"Error While Reading JSON File : {ex}");
                return; // Exit the method if there's an error reading the file not needed here but just to be safe
            }
        }
    }
}
