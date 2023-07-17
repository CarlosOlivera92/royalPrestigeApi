using AutoMapper;
using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;
using RoyalPrestige_API.Data;
using RoyalPrestige_API.DTO;
using static api.CQRS.Command.PostUsuario;

namespace RoyalPrestige_API.CQRS.Command
{
    public class PutUsuario
    {
        public class PutUsuarioCommand : IRequest<UsuarioDTO>
        {
            public long Id { get; set; }
            public string Nombre { get; set; }
            public string Apellido { get; set; }
            public string Username { get; set; }
            public string Email { get; set; }
            public string ProfilePic { get; set; }
            public DateOnly FechaNacimiento { get; set; }
            public long Telefono { get; set; }
            public long RolId { get; set; }
        }
        public class PutUsuarioCommandValidator : AbstractValidator<PutUsuarioCommand>
        {
            private readonly ApplicationContext _context;
            public PutUsuarioCommandValidator(ApplicationContext context)
            {
                _context = context;

                RuleFor(x => x.Id).NotEmpty().WithMessage("El ID del usuario no puede estar vacío");
                RuleFor(x => x.Username).NotEmpty().WithMessage("El usuario no puede estar vacío");
                RuleFor(x => x).MustAsync((command, cancellationToken) => RolExiste(command.RolId, cancellationToken)).WithMessage("El rol proporcionado no existe");
            }

            // Resto del código de validación
            private async Task<bool> RolExiste(long rolId, CancellationToken token)
            {
                bool existe = await _context.Roles.AnyAsync(x => x.Id == rolId);
                return existe;
            }
        }
        public class PutUsuarioCommandHandler : IRequestHandler<PutUsuarioCommand, UsuarioDTO>
        {
            private readonly ApplicationContext _context;
            private readonly IMapper _mapper;
            private readonly IValidator<PutUsuarioCommand> _validator;

            public PutUsuarioCommandHandler(ApplicationContext context, IMapper mapper, IValidator<PutUsuarioCommand> validator)
            {
                _context = context;
                _mapper = mapper;
                _validator = validator;
            }

            public async Task<UsuarioDTO> Handle(PutUsuarioCommand command, CancellationToken cancellationToken)
            {
                try
                {
                    // Realiza la validación de los campos utilizando el validador
                    await _validator.ValidateAndThrowAsync(command);

                    // Busca el usuario en la base de datos por su ID
                    var usuario = await _context.Usuarios.FindAsync(command.Id);

                    // Si el usuario no existe, puedes lanzar una excepción o manejar el error según tus necesidades
                    if (usuario == null)
                    {
                        throw new Exception("El usuario no existe");
                    }

                    // Actualiza los campos del usuario con los valores del comando
                    usuario.Username = command.Username;
                    usuario.Nombre = command.Nombre;
                    usuario.Apellido = command.Apellido;
                    usuario.Email = command.Email;
                    usuario.FechaNacimiento = command.FechaNacimiento;
                    usuario.ProfilePic = command.ProfilePic;
                    usuario.Telefono = command.Telefono;
                    usuario.RolId = command.RolId;

                    // Guarda los cambios en la base de datos
                    await _context.SaveChangesAsync(cancellationToken);

                    // Mapea el usuario actualizado a UsuarioDTO y lo devuelve como resultado
                    var usuarioDTO = _mapper.Map<UsuarioDTO>(usuario);
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
