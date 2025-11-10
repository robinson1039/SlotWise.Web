namespace SlotWise.Web.Data.Entities
{
    public class Permission
    {
        public Guid Id { get; set; }
        public required string Name { get; set; }
        public required string Description { get; set; }
        public required string Module { get; set; }
        // Relación muchos a muchos con PrivateRole a través de RolePermission
        public ICollection<RolePermission>? RolePermissions { get; set; }
    }
}
