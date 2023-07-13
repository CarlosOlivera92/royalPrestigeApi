namespace RoyalPrestige_API.DTO
{
    public class ContratoDTO
    {
        public long Id { get; set; }
        public string Contrato { get; set; }
        public long ClienteId { get; set; }
        public long VendedorId { get; set; }
    }
}
