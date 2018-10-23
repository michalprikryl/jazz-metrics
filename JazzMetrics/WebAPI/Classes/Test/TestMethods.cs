using WebAPI.Classes.Error;
using WebAPI.Models.Error;
using System;
using System.Reflection;
using System.Threading.Tasks;
using Database;

namespace WebAPI.Classes.Test
{
    public class TestMethods
    {
        public async Task<object> RunTest()
        {
            bool dbValue = true;
            string dbMessage = null;
            try
            {
                using (JazzMetricsEntities entities = new JazzMetricsEntities())
                {
                    await entities.Database.Connection.OpenAsync();
                    entities.Database.Connection.Close();
                }
            }
            catch (Exception e)
            {
                await ErrorManager.SaveErrorToDB(new ErrorModel(e, module: "TestMethods", function: "RunTest"));

                dbValue = false;
                dbMessage = e.Message;

                if (e.InnerException != null)
                {
                    dbMessage += $"; {e.InnerException.Message}";
                }
            }

            return new { ConnectionDB = dbValue, MessageDB = dbMessage, APIVersion = Assembly.GetExecutingAssembly().GetName().Version.ToString() };
        }
    }
}