using System.Threading.Tasks;
using Xunit;
using Microsoft.EntityFrameworkCore;
using EnpalSharpTemplate.Model;
using System;
using EnpalSharpTemplate.Services;
using System.Linq;

namespace EnpalSharpTemplate.Test
{
    public class MySimpleTests
    {
        [Fact]
        public async void Add_New_Version()
        {
            //arrange 
            var record = new PayloadModel
            {
                Key = "customer.1",
                FirstName = "saif",
                LastName = "manea",
                Address = "earth"
            };
            var options = new DbContextOptionsBuilder<HistoryContext>()
            .UseInMemoryDatabase(databaseName: "MyData1").Options;

            // Act
            using (var context = new HistoryContext(options))
            {
                RegistrationHistoryService historyRepository = new RegistrationHistoryService(context);
                bool result = await historyRepository.Create(record);

                // Assert
                Assert.True(result);
            }
        }

        [Fact]
        public async void Update_Exist_Version()
        {
            //arrange 
            var record = new PayloadModel
            {
                Key = "customer.1",
                FirstName = "new name",
                LastName = "manea",
                Address = "earth"
            };
            var options = new DbContextOptionsBuilder<HistoryContext>()
            .UseInMemoryDatabase(databaseName: "MyData2").Options;

            // Seed Data
            using (var context = new HistoryContext(options))
            {
                context.HistoryData.Add(new PayloadModel { Key = "customer.1", FirstName = "siaf", LastName = "manea", Address = "berlin", CreatedDate = DateTime.UtcNow });
                _ = context.AddAsync(record);
                _ = context.SaveChangesAsync();
            }

            // Act
            using (var context = new HistoryContext(options))
            {
                RegistrationHistoryService historyRepository = new RegistrationHistoryService(context);
                // Assert
                Assert.True(await historyRepository.Create(record));
            }
        }

        [Fact]
        public async Task Get_Version_ByKeyAsync()
        {
            //arrange 
            string Key = "customer.3";
               
            var options = new DbContextOptionsBuilder<HistoryContext>()
            .UseInMemoryDatabase(databaseName: "MyData3").Options;

            // Seed Data
            using (var context = new HistoryContext(options))
            {
                context.HistoryData.Add(new PayloadModel { Key = "customer.3", FirstName = "siaf", LastName = "manea", Address = "berlin", CreatedDate = DateTime.UtcNow });
                context.SaveChanges();
            }

            // Act
            using (var context = new HistoryContext(options))
            {
                SearchHistoryService historyRepository = new SearchHistoryService(context);
                var result = await historyRepository.GetSingleVersion(Key);

                // Assert
                Assert.NotNull(result);
            }
        }

        [Fact]
        public async Task Version_Not_Exist()
        {
            //arrange 
            string Key = "customer.3";

            var options = new DbContextOptionsBuilder<HistoryContext>()
            .UseInMemoryDatabase(databaseName: "MyData4").Options;

            // Act
            using (var context = new HistoryContext(options))
            {
                SearchHistoryService historyRepository = new SearchHistoryService(context);
                var result = await historyRepository.GetSingleVersion(Key);

                // Assert
                Assert.Null(result);
            }
        }

        [Fact]
        public async Task Get_All_versionsAsync()
        {
            //arrange 
            var options = new DbContextOptionsBuilder<HistoryContext>()
            .UseInMemoryDatabase(databaseName: "MyData5").Options;

            // Seed Data
            using (var context = new HistoryContext(options))
            {
                context.HistoryData.Add(new PayloadModel { Id = Guid.NewGuid(), Key = "customer.2", FirstName = "siaf", LastName = "manea", Address = "berlin", CreatedDate = DateTime.UtcNow });
                context.HistoryData.Add(new PayloadModel { Id = Guid.NewGuid(), Key = "customer.1", FirstName = "siaf", LastName = "manea", Address = "berlin", CreatedDate = DateTime.UtcNow });
                context.SaveChanges();
            }
            // Act
            using (var context = new HistoryContext(options))
            {
                SearchHistoryService historyRepository = new SearchHistoryService(context);
                var result = await historyRepository.GetAll();

                // Assert
                Assert.True(result.Count == 2);
            }
        }

        [Fact]
        public async Task No_Version_Exist()
        {
            //arrange 
            var options = new DbContextOptionsBuilder<HistoryContext>()
            .UseInMemoryDatabase(databaseName: "MyData6").Options;

            // Act
            using (var context = new HistoryContext(options))
            {
                SearchHistoryService historyRepository = new SearchHistoryService(context);
                var result = await historyRepository.GetAll();

                // Assert
                Assert.True(result.Count().Equals(0));
            }
        }

    }
}