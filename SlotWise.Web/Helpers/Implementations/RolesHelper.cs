using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using SlotWise.Web.Data;
using SlotWise.Web.Helpers.Abstractions;

namespace SlotWise.Web.Helpers.Implementations
{
    public class RolesHelper : IRolesHelper
    {
        private readonly DataContext _context;

        public RolesHelper(DataContext context)
        {
            _context = context;
        }

        public async Task<List<SelectListItem>> GetComboRolesAsync()
        {
            return await _context.PrivateRoles
                .Select(r => new SelectListItem
                {
                    Text = r.Name,
                    Value = r.Id.ToString()
                })
                .ToListAsync();
        }
    }
}
