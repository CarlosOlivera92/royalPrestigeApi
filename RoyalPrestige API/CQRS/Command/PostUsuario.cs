
using AutoMapper;
using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;
using RoyalPrestige_API.Data;
using RoyalPrestige_API.DTO;
using RoyalPrestige_API.Models;
using static RoyalPrestige_API.CQRS.Command.PostRoles;

namespace api.CQRS.Command
{
    public class PostUsuario
    {
        public class PostUsuarioCommand : IRequest<UsuarioDTO>
        {
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
        public class PostUsuarioCommandValidator : AbstractValidator<PostUsuarioCommand> 
        {
            private readonly ApplicationContext _context;
            public PostUsuarioCommandValidator(ApplicationContext context)
            {
                _context = context;
                RuleFor(x => x.Username).NotEmpty().WithMessage("El usuario no puede estar vacio");
                RuleFor(x => x.Password).NotEmpty().WithMessage("La contraseña no puede estar vacia");
                RuleFor(x => x).MustAsync(PersonaExiste).WithMessage("El usuario no existe");
                RuleFor(x => x).MustAsync((command, cancellationToken) => RolExiste(command.RolId, cancellationToken)).WithMessage("El rol proporcionado no existe");

            }
            private async Task<bool> PersonaExiste(PostUsuarioCommand command, CancellationToken token)
            {
                bool existe = await _context.Usuarios.AnyAsync(x => x.Username != command.Username
                                                              && x.Password != command.Password);
                return !existe;
            }
            private async Task<bool> RolExiste(long rolId, CancellationToken token)
            {
                bool existe = await _context.Roles.AnyAsync(x => x.Id == rolId);
                return existe;
            }
        }
        public class PostUsuarioCommandHandler : IRequestHandler<PostUsuarioCommand, UsuarioDTO>
        {
            private readonly ApplicationContext _context;
            private readonly IMapper _mapper;
            private readonly IValidator<PostUsuarioCommand> _validator;
            public PostUsuarioCommandHandler(ApplicationContext context, IMapper mapper, IValidator<PostUsuarioCommand> validator)
            {
                _context = context;
                _mapper = mapper;
                _validator = validator;
            }
            public async Task<UsuarioDTO> Handle(PostUsuarioCommand command, CancellationToken cancellationToken)
            {
                try
                {
                    // Aquí puedes agregar la lógica de validación de los campos de registro, si es necesario

                    // Crea una nueva instancia de Usuario basada en los datos del comando
                    var nuevoUsuario = new Usuario
                    {
                        Username = command.Username,
                        Nombre = command.Nombre,
                        Apellido = command.Apellido,
                        Email = command.Email,
                        FechaNacimiento = command.FechaNacimiento,
                        ProfilePic = command.ProfilePic,
                        Password = command.Password,
                        Telefono = command.Telefono,
                        RolId = command.RolId,
                        // Otros campos necesarios para el registro
                    };

                    // Agrega el nuevo usuario al contexto de base de datos
                    _context.Usuarios.Add(nuevoUsuario);

                    // Guarda los cambios en la base de datos
                    await _context.SaveChangesAsync(cancellationToken);

                    // Obtén el ID del usuario recién creado
                    var usuarioId = nuevoUsuario.Id;

                    // Obtén el id del rol del usuario recién creado
                    var roleUserId = nuevoUsuario.RolId;

                    // Verifica si el RolId es igual a 3 (Vendedor)
                    if (roleUserId == 3)
                    {
                        // Crea una nueva instancia de Vendedores y establece el campo UsuarioID
                        var vendedor = new Vendedores
                        {
                            VendedorId = usuarioId
                        };

                        // Agrega el vendedor a la tabla Vendedores
                        _context.Vendedores.Add(vendedor);

                        // Guarda los cambios en la base de datos
                        await _context.SaveChangesAsync();
                    }

                    // Mapea el nuevo usuario a UsuarioDTO y lo devuelve como resultado
                    var usuarioDTO = _mapper.Map<UsuarioDTO>(nuevoUsuario);
                    return usuarioDTO;
                }
                catch (Exception)
                {
                    throw;
                }
            }
        }
    }
}
