using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace WebApp.Services.Setting
{
    public interface ISettingService
    {
        Task<List<SelectListItem>> GetAspiceVersionsForSelect(string jwt);
    }
}
