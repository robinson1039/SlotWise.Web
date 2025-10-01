using System.ComponentModel.DataAnnotations;

namespace SlotWise.Web.DTOs
{
    public class UserDTO
    {
        public Guid Id { get; set; }

        [Display(Name = "Nombre de usuario")]
        [Required(ErrorMessage = "El campo {0} es requerido.")]
        public string UserName { get; set; } = string.Empty;

        [Display(Name = "Contraseña")]
        [Required(ErrorMessage = "El campo {0} es requerido.")]
        public string PasswordHash { get; set; } = string.Empty;

        [Display(Name = "Nombres")]
        [Required(ErrorMessage = "El campo {0} es requerido.")]
        public string FirstName { get; set; } = string.Empty;

        [Display(Name = "Apellidos")]
        [Required(ErrorMessage = "El campo {0} es requerido.")]
        public string LastName { get; set; } = string.Empty;

        [Display(Name = "Cédula")]
        public int? CC { get; set; }

        [Display(Name = "Edad")]
        public int? Age { get; set; }

        [Display(Name = "Reserva")]
        public int? ReservaId { get; set; }

        [Display(Name = "Fecha de nacimiento")]
        [Required(ErrorMessage = "El campo {0} es requerido.")]
        public DateTime Birthdate { get; set; }

        public DateTime CreateAt { get; set; }
    }
}
