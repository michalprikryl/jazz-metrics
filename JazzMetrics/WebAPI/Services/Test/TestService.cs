using Database;
using Library.Models.Test;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.Extensions.Configuration;
using System;
using WebAPI.Services.Helpers;

namespace WebAPI.Services.Test
{
    public class TestService : BaseDatabase, ITestService
    {
        private readonly IConfiguration _config;

        public TestService(JazzMetricsContext db, IConfiguration config) : base(db)
        {
            _config = config;
        }

        public TestModel RunTest()
        {
            TestModel model = new TestModel { ConnectionDB = true, ApiVersion = _config["Version"] };

            try
            {
                Database.Database.GetService<IRelationalDatabaseCreator>().Exists();
            }
            catch (Exception e)
            {
                model.ConnectionDB = false;
                model.MessageDB = e.Message;

                if (e.InnerException != null)
                {
                    model.MessageDB += $"; {e.InnerException.Message}";
                }
            }

            return model;
        }
    }
}
