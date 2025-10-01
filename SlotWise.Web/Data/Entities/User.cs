namespace SlotWise.Web.Data.Entities
{
    public class User
    {
        public Guid Id { get; set; }
        public required string UserName { get; set; }
        public required string PasswordHash { get; set; }
        public required string FirstName { get; set; }
        public required string LastName { get; set; }
        public int? CC { get; set; }
        public int? Age { get; set; }
        public DateTime Birthdate { get; set; }
        public DateTime CreateAt { get; set; } = DateTime.UtcNow;

        // Relación
        public ICollection<Reservation> Reservations { get; set; } = new List<Reservation>();
    }
}
