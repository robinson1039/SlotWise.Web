namespace SlotWise.Web.Data.Entities
{
    public class PrivateRole
    {
        public Guid Id { get; set; }
        public required string Name { get; set; }

        // Relación muchos a muchos con Permission a través de RolePermission
        public ICollection<RolePermission>? RolePermissions { get; set; }

    }
}
