using System.ComponentModel.DataAnnotations;

namespace SlotWise.Web.DTOs
{
    public class SpecialistDTO
    {
        public Guid Id { get; set; }   // Se mantiene para identificar el especialista
        [Display(Name = "Especialista")]
        [Required(ErrorMessage = "El campo {0} es requerido.")]
        public string Name { get; set; } = string.Empty;  // Obligatorio
        [Display(Name = "Edad")]
        public int? Age { get; set; }  // Opcional
        [Display(Name = "¿Está Disponible?")]
        public bool Status { get; set; }  // Activo/Inactivo

        public DateTime CreateAt { get; set; }  // Fecha de creación
    }
}
