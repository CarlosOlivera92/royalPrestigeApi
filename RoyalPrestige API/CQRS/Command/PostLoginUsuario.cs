
using AutoMapper;
using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using RoyalPrestige_API.Data;
using RoyalPrestige_API.DTO;
using RoyalPrestige_API.Models;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Threading;

namespace api.CQRS.Command
{
    public class PostLoginUsuario
    {
        public class PostLoginUsuarioCommand : IRequest<UsuarioDTO>
        {
            public string Username { get; set; }
            public string Password { get; set; }
        }
        public class PostLoginUsuarioCommandValidator : AbstractValidator<PostLoginUsuarioCommand>
        {
            private readonly ApplicationContext _context;
            public PostLoginUsuarioCommandValidator(ApplicationContext context)
            {
                _context = context;
                RuleFor(x => x.Username).NotEmpty().WithMessage("El nombre de usuario no puede estar vacío");
                RuleFor(x => x.Password).NotEmpty().WithMessage("La contraseña no puede estar vacia");
                RuleFor(x => x).MustAsync(UsuarioExiste).WithMessage("El usuario no existe");
            }
            public async Task<bool> UsuarioExiste(PostLoginUsuarioCommand command, CancellationToken cancellationToken)
            {
                bool existe = await _context.Usuarios.AnyAsync(x => x.Username != command.Username
                                                              && x.Password != command.Password);
                return !existe;
            }
        }
        public class PostLoginUsuarioCommandHandler : IRequestHandler<PostLoginUsuarioCommand, UsuarioDTO>
        {
            private readonly ApplicationContext _context;
            private readonly IMapper _mapper;
            private readonly IValidator<PostLoginUsuarioCommand> _validator;
            private readonly IConfiguration _config;
            public PostLoginUsuarioCommandHandler(ApplicationContext context, IMapper mapper, IValidator<PostLoginUsuarioCommand> validator, IConfiguration config)
            {
                _context = context;
                _mapper = mapper;
                _validator = validator;
                _config = config;
            }

            public async Task<UsuarioDTO> Handle(PostLoginUsuarioCommand request, CancellationToken cancellationToken)
            {
                try
                {
                    var Usuario = await Authenticate(request, cancellationToken);
                    var usuarioDto = _mapper.Map<UsuarioDTO>(Usuario);
                    usuarioDto.RolNombre = await GetRolNombre(usuarioDto.RolId); // Obtener el nombre del rol
                    var token = GenerateToken(usuarioDto);
                    usuarioDto.Token = token;
                    return usuarioDto;
                }
                catch (Exception)
                {   
                    throw;
                }
            }
            public string GenerateToken(UsuarioDTO usuarioDto)
            {
                var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["JWT:Key"]));
                var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);
                //Crear los claims
                var claims = new[]
                {
                    new Claim(ClaimTypes.NameIdentifier, usuarioDto.Username),
                    new Claim(ClaimTypes.Email, usuarioDto.Email),
                    new Claim(ClaimTypes.GivenName, usuarioDto.Nombre),
                    new Claim(ClaimTypes.Surname, usuarioDto.Apellido),
                    new Claim(ClaimTypes.Role, usuarioDto.RolNombre)
                };
                //Crear el Token
                var token = new JwtSecurityToken(
                    _config["JWT:Issuer"],
                    _config["JWT:Audience"],
                    claims,
                    expires: DateTime.Now.AddDays(1),
                    signingCredentials: credentials);
                return new JwtSecurityTokenHandler().WriteToken(token);
            }
            public async Task<Usuario> Authenticate(PostLoginUsuarioCommand request, CancellationToken cancellationToken)
            {
                var UsuarioResultado = await _validator.ValidateAsync(request, cancellationToken);
                if (!UsuarioResultado.IsValid)
                {
                    throw new ValidationException(UsuarioResultado.Errors);
                }
                var Usuario = await _context.Usuarios.FirstOrDefaultAsync(x => x.Username == request.Username
                && x.Password == request.Password, cancellationToken);
                if (Usuario == null)
                {
                    throw new Exception("Credenciales de inicio de sesión inválidas.");
                }
                return Usuario;
            }
            public async Task<string> GetRolNombre(long rolId)
            {
                var rol = await _context.Roles.FirstOrDefaultAsync(r => r.Id == rolId);
                return rol?.Rol;
            }
        }
    }
}
