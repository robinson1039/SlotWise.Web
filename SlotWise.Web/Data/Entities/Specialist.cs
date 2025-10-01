namespace SlotWise.Web.Data.Entities
{
    public class Specialist
    {
        public Guid Id { get; set; }
        public required string FirstName { get; set; }
        public required string LastName { get; set; }
        public int? CC { get; set; }
        public required string Email { get; set; }
        public required string Phone { get; set; }
        public required string SpecialistDescription { get; set; }
        public int? Age { get; set; }
        public bool Status { get; set; } = false;
        public DateTime Create_at { get; set; } = DateTime.UtcNow;

        // Relaciones
        public ICollection<Service> Services { get; set; } = new List<Service>();
        public ICollection<Reservation> Reservations { get; set; } = new List<Reservation>();
    }
}
