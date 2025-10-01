using System.ComponentModel.DataAnnotations;

namespace SlotWise.Web.DTOs
{
    public class SpecialistDTO
    {
        public Guid Id { get; set; }

        [Display(Name = "Nombres")]
        [Required(ErrorMessage = "El campo {0} es requerido.")]
        public string FirstName { get; set; } = string.Empty;

        [Display(Name = "Apellidos")]
        [Required(ErrorMessage = "El campo {0} es requerido.")]
        public string LastName { get; set; } = string.Empty;

        [Display(Name = "Nombre Completo")]
        public string FullName => $"{FirstName} {LastName}";

        [Display(Name = "Cédula")]
        public int? CC { get; set; }

        [Display(Name = "Email")]
        [Required(ErrorMessage = "El campo {0} es requerido.")]
        [EmailAddress(ErrorMessage = "El formato del email no es válido.")]
        public string Email { get; set; } = string.Empty;

        [Display(Name = "Teléfono")]
        [Required(ErrorMessage = "El campo {0} es requerido.")]
        [Phone(ErrorMessage = "El formato del teléfono no es válido.")]
        public string Phone { get; set; } = string.Empty;

        [Display(Name = "Descripción")]
        [Required(ErrorMessage = "El campo {0} es requerido.")]
        [StringLength(500, ErrorMessage = "La descripción no puede exceder 500 caracteres.")]
        public string SpecialistDescription { get; set; } = string.Empty;

        [Display(Name = "Edad")]
        [Range(18, 100, ErrorMessage = "La edad debe estar entre 18 y 100 años.")]
        public int? Age { get; set; }

        [Display(Name = "Estado")]
        public bool Status { get; set; }

        [Display(Name = "Fecha de creación")]
        public DateTime CreateAt { get; set; }
    }
}
