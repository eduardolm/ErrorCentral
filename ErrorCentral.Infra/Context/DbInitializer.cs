using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using ErrorCentral.Domain.Models;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using Environment = ErrorCentral.Domain.Models.Environment;

namespace ErrorCentral.Infra.Context
{
    public class DbInitializer
    {
        public static DbContextOptions<MainContext> DbOptions { get; set; }
        
        private static Dictionary<Type, string> DataFileNames { get; } = new Dictionary<Type, string>();
        
        private static string FileName<T>()
        {
            return DataFileNames[typeof(T)];
        }
        public static void Initialize(MainContext context)
        {
            DbOptions = new DbContextOptionsBuilder<MainContext>()
                .Options;
            
            context.Database.EnsureCreated();

            if (context.Log.Any())
            {
                return;
            }
            
            DataFileNames.Add(typeof(User), $@"..\ErrorCentral.Test\FakeData{Path.DirectorySeparatorChar}users.json");
            DataFileNames.Add(typeof(Environment), $@"..\ErrorCentral.Test\FakeData{Path.DirectorySeparatorChar}environments.json");
            DataFileNames.Add(typeof(Layer), $@"..\ErrorCentral.Test\FakeData{Path.DirectorySeparatorChar}layers.json");
            DataFileNames.Add(typeof(Level), $@"..\ErrorCentral.Test\FakeData{Path.DirectorySeparatorChar}levels.json");
            DataFileNames.Add(typeof(Status), $@"..\ErrorCentral.Test\FakeData{Path.DirectorySeparatorChar}status.json");
            DataFileNames.Add(typeof(Log), $@"..\ErrorCentral.Test\FakeData{Path.DirectorySeparatorChar}logs.json");

            FillWithAll();
        }
        
        public static void FillWithAll()
        {
            FillWith<User>();
            FillWith<Environment>();
            FillWith<Layer>();
            FillWith<Level>();
            FillWith<Status>();
            FillWith<Log>();
        }
        
        public static void FillWith<T>() where T : class
        {
            using (var context = new MainContext(DbOptions))
            {
                if (context.Set<T>().Count() == 0)
                {
                    context.Database.OpenConnection();
                    foreach (var item in GetData<T>())
                    {
                        var fullName = item.GetType().FullName;
                        if (fullName != null)
                        {
                            var itemName = $"[{fullName.Split('.')[3]}]";
                            var turnOn = $"SET IDENTITY_INSERT {itemName} ON";
                            var turnOff = $"SET IDENTITY_INSERT {itemName} OFF";
                            context.Database.ExecuteSqlRaw(turnOn);
                            context.Set<T>().Add(item);
                            context.SaveChanges();
                            context.Database.ExecuteSqlRaw(turnOff);
                        }
                    }
                    context.Database.CloseConnection();
                }
            }
        }
        
        public static List<T> GetData<T>()
        {
            var content = File.ReadAllText(FileName<T>());
            return JsonConvert.DeserializeObject<List<T>>(content);
        }
    }
}