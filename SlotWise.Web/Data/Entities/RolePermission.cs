namespace SlotWise.Web.Data.Entities
{
    public class RolePermission
    {
        // Tabla que relaciona PrivateRole y Permission (muchos a muchos)
        public required Guid PrivateRoleId { get; set; }
        public required Guid PermissionId { get; set; }
        // propiedades de navegación 
        public  PrivateRole PrivateRole { get; set; }
        public  Permission Permission { get; set; }

    }
}
