namespace RoyalPrestige_API.DTO
{
    public class UsuarioDTO
    {
        public long Id { get; set; }
        public string Nombre { get; set; }
        public string Apellido { get; set; }
        public string Username { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public string ProfilePic { get; set; }
        public DateOnly FechaNacimiento { get; set; }
        public long Telefono { get; set; }
        public long RolId { get; set; }
    }
}
