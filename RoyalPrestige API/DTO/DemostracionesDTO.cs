namespace RoyalPrestige_API.DTO
{
    public class DemostracionesDTO
    {
        public long Id { get; set; }
        public DateTime FechaDemostracion { get; set; }
        public long ClienteId { get; set; }
        public long EstadoVisitaId { get; set; }
        public long EstadoVenta { get; set; }
    }
}
