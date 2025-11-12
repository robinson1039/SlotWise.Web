using Microsoft.AspNetCore.Mvc.Rendering;

namespace SlotWise.Web.Helpers.Abstractions
{
    public interface IRolesHelper
    {
        Task<List<SelectListItem>> GetComboRolesAsync();
    }
}

