namespace RoyalPrestige_API.DTO
{
    public class ReprogramacionesDTO
    {
        public long Id { get; set; }
        public long DemostracionId { get; set; }
        public DateTime NuevaFecha { get; set; }
        public string Observaciones { get; set; }
    }
}
