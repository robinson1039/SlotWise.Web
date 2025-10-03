using System.ComponentModel.DataAnnotations;

namespace SlotWise.Web.DTOs
{
    public class ServiceDTO
    {
        public Guid Id { get; set; }

        [Display(Name = "Nombre del Servicio")]
        [Required(ErrorMessage = "El campo {0} es requerido.")]
        [StringLength(100, ErrorMessage = "El nombre no puede exceder 100 caracteres.")]
        public string NameService { get; set; } = string.Empty;

        [Display(Name = "Precio")]
        [Required(ErrorMessage = "El campo {0} es requerido.")]
        [Range(0.01, 1000000, ErrorMessage = "El precio debe ser mayor a 0.")]
        public decimal Price { get; set; }

        [Display(Name = "Descripción")]
        [Required(ErrorMessage = "El campo {0} es requerido.")]
        [StringLength(500, ErrorMessage = "La descripción no puede exceder 500 caracteres.")]
        public string Description { get; set; } = string.Empty;

        [Display(Name = "Estado")]
        public bool Status { get; set; } = true;

        [Display(Name = "Especialista")]
        [Required(ErrorMessage = "El campo {0} es requerido.")]
        public Guid SpecialistId { get; set; }

        // Opcional: Para mostrar el nombre del especialista
        public string? SpecialistName { get; set; }
    }
}
