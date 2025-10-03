namespace SlotWise.Web.Data.Entities
{
    public class Service
    {
        public Guid Id { get; set; }
        public required string NameService { get; set; } = string.Empty;
        public decimal Price { get; set; }
        public required string Description { get; set; } = string.Empty;
        public bool Status { get; set; } = false;
        public Guid SpecialistId { get; set; }
        public Specialist Specialist { get; set; } = null!;
    }
}
