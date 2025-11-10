using Microsoft.AspNetCore.Identity;

namespace SlotWise.Web.Data.Entities
{
    public class User : IdentityUser<Guid>
    {
        public required string FirstName { get; set; }
        public required string LastName { get; set; }
        public int? CC { get; set; }
        public int? Age { get; set; }
        public DateTime Birthdate { get; set; }
        public DateTime CreateAt { get; set; } = DateTime.UtcNow;

        // Relaciones
        public required Guid PrivateRoleId { get; set; }
        public PrivateRole? PrivateRole { get; set; }
        // Un usuario puede tener muchas reservas
        public ICollection<Reservation> Reservations { get; set; } = new List<Reservation>();
        // Propiedad calculada para el nombre completo
        public string FullName => $"{FirstName} {LastName}";
    }
}
