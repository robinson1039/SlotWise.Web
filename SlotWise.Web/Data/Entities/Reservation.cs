namespace SlotWise.Web.Data.Entities
{
    public class Reservation
    {
        public Guid Id { get; set; }
        public Guid UserId { get; set; }
        public Guid SpecialistId { get; set; }
        public Guid ServiceId { get; set; }
        public User User { get; set; } = null!;
        public Specialist Specialist { get; set; } = null!;
        public Service Service { get; set; } = null!;
        public bool Status { get; set; } = false;
        public DateTime CreateAt { get; set; } = DateTime.UtcNow;

    }
}
