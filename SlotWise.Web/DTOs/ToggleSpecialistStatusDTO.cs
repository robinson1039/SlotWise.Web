namespace SlotWise.Web.DTOs
{
    public class ToggleSpecialistStatusDTO
    {
   
        public Guid Id { get; set; }   // Id del especialista

        public bool Status { get; set; }  // Nuevo estado (true = activo, false = inactivo)
        
    }
}
