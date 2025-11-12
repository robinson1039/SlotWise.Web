namespace SlotWise.Web.DTOs
{
    public class RolesDTO
    {
        public Guid Id { get; set; }

        public string Name { get; set; } = string.Empty;

        // Lista de permisos asociados al rol
        public List<PermissionForRolesDTO> Permissions { get; set; } = new();

        // Se usará al guardar permisos seleccionados desde la vista
        public string? PermissionIds { get; set; }
    }
}

