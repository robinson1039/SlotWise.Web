namespace SlotWise.Web.DTOs
{
    public class PermissionForRolesDTO
    {
        public Guid Id { get; set; }
        public string Module { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public bool Selected { get; set; } // Si el permiso está activo para este rol
    }
}
