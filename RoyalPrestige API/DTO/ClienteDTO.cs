namespace RoyalPrestige_API.DTO
{
    public class ClienteDTO
    {
        public long Id { get; set; }
        public string Nombre { get; set; }
        public string Apellido { get; set; }
        public long Telefono { get; set; }
        public string Direccion { get; set; }
        public string Observaciones { get; set; }
        public string Email { get; set; }
        public long VendedorId { get; set; }
    }
}
