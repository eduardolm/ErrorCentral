using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using ErrorCentral.Application.Interfaces;
using ErrorCentral.Domain.Models;
using ErrorCentral.Infra.Context;
using Microsoft.EntityFrameworkCore;
using Moq;
using Newtonsoft.Json;
using Environment = ErrorCentral.Domain.Models.Environment;

namespace ErrorCentral.Test.Unit.Infra.Context
{
    public class FakeContext
    {
        public FakeContext(string testName)
        {
            FakeOptions = new DbContextOptionsBuilder<MainContext>()
                .UseInMemoryDatabase($"ErrorCentral_{testName}")
                .Options;

            var path = @$"{AppDomain.CurrentDomain.BaseDirectory.Split(@"\bin")[0]}\FakeData\";

            DataFileNames.Add(typeof(User), $"{path}users.json");
            DataFileNames.Add(typeof(Layer), $"{path}layers.json");
            DataFileNames.Add(typeof(Level), $"{path}levels.json");
            DataFileNames.Add(typeof(Status), $"{path}status.json");
            DataFileNames.Add(typeof(Environment), $"{path}environments.json");
            DataFileNames.Add(typeof(Log), $"{path}logs.json");
        }

        public DbContextOptions<MainContext> FakeOptions { get; }

        private Dictionary<Type, string> DataFileNames { get; } =
            new Dictionary<Type, string>();

        private string FileName<T>()
        {
            return DataFileNames[typeof(T)];
        }

        public void FillWithAll()
        {
            FillWith<User>();
            FillWith<Layer>();
            FillWith<Level>();
            FillWith<Status>();
            FillWith<Environment>();
            FillWith<Log>();
        }

        public void FillWith<T>() where T : class
        {
            using (var context = new MainContext(FakeOptions))
            {
                if (context.Set<T>().Any()) return;
                foreach (var item in GetFakeData<T>())
                    context.Set<T>().Add(item);
                context.SaveChanges();
            }
        }

        public List<T> GetFakeData<T>()
        {
            var content = File.ReadAllText(FileName<T>());
            return JsonConvert.DeserializeObject<List<T>>(content);
        }
        
        public Mock<IUserService> FakeUserService()
        {
            var service = new Mock<IUserService>();
            var userList = GetFakeData<User>();

            service.Setup(x => x.GetAll())
                .Returns(() => userList.ToList());

            service.Setup(x => x.GetById(It.IsAny<int>())).
                Returns((int id) => GetFakeData<User>().FirstOrDefault(x => x.Id == id));

            service.Setup(x => x.Create(It.IsAny<User>())).
                Returns((User user) => {

                        if (user.Id == 0)
                            user.Id = 999;
                    
                        return user;
                });

            service.Setup(x => x.Update(It.IsAny<User>()))
                .Returns((User user) => user);

            service.Setup(x => x.Delete(It.IsAny<int>()))
                .Returns((int id) =>
                {
                    userList.Remove(userList[userList.FindIndex(x => x.Id == id)]);
                    return userList;
                });

            return service;
        }
    }
}