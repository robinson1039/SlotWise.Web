using System.ComponentModel.DataAnnotations;

namespace SlotWise.Web.DTOs
{
    public class ReservationDTO
    {
        public Guid Id { get; set; }

        [Display(Name = "Usuario")]
        [Required(ErrorMessage = "El campo {0} es requerido.")]
        public Guid UserId { get; set; }

        [Display(Name = "Especialista")]
        [Required(ErrorMessage = "El campo {0} es requerido.")]
        public Guid SpecialistId { get; set; }

        // Opcional: Incluir datos de las relaciones
        public string? UserName { get; set; }
        public string? SpecialistName { get; set; }

        [Display(Name = "Estado")]
        public bool Status { get; set; }

        [Display(Name = "Fecha de creación")]
        public DateTime CreateAt { get; set; }
    }
}
