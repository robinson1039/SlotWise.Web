using Microsoft.VisualStudio.Web.CodeGenerators.Mvc.Templates.Blazor;
using System.ComponentModel.DataAnnotations;

namespace SlotWise.Web.Data.Entities
{
    public class Specialist
    {
        [Key]
        public Guid Id { get; set; }

        public required string Name { get; set; }

        public int? Age { get; set; }

        public bool Status { get; set; } = false;
        public DateTime Create_at { get; set; } = DateTime.UtcNow;
    }
}
