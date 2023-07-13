using AutoMapper;
using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;
using RoyalPrestige_API.Data;
using RoyalPrestige_API.DTO;
using RoyalPrestige_API.Models;
using static api.CQRS.Command.PostUsuario;

namespace RoyalPrestige_API.CQRS.Command
{
    public class PostRoles
    {
        public class PostRolesCommand : IRequest<RolDTO>
        {
            public string Rol { get; set; }
        }
        public class PostRolesCommandValidator : AbstractValidator<PostRolesCommand>
        {
            private readonly ApplicationContext _context;
            public PostRolesCommandValidator(ApplicationContext context)
            {
                _context = context;
                RuleFor(x => x.Rol).NotEmpty().WithMessage("La contraseña no puede estar vacia");
                RuleFor(x => x).MustAsync(RoleExiste).WithMessage("El usuario no existe");

            }
            private async Task<bool> RoleExiste(PostRolesCommand command, CancellationToken token)
            {
                bool existe = await _context.Roles.AnyAsync(x => x.Rol != command.Rol);
                return !existe;
            }
            public class PostRolesCommanHandler : IRequestHandler<PostRolesCommand, RolDTO>
            {
                private readonly ApplicationContext _context;
                private readonly IMapper _mapper;
                private readonly IValidator<PostRolesCommand> _validator;
                public PostRolesCommanHandler(ApplicationContext context, IMapper mapper, IValidator<PostRolesCommand> validator)
                {
                    _context = context;
                    _mapper = mapper;
                    _validator = validator;
                }
                public async Task<RolDTO> Handle(PostRolesCommand command, CancellationToken cancellationToken)
                {
                    try
                    {

                        // Crea una nueva instancia de Roles basada en los datos del comando
                        var nuevoRole = new Role
                        {
                            Rol = command.Rol,
                        };

                        // Agrega el nuevo Rol al contexto de base de datos
                        _context.Roles.Add(nuevoRole);

                        // Guarda los cambios en la base de datos
                        await _context.SaveChangesAsync(cancellationToken);

  

                        // Mapea el nuevo usuario a UsuarioDTO y lo devuelve como resultado
                        var roleDTO = _mapper.Map<RolDTO>(nuevoRole);
                        return roleDTO;
                    }
                    catch (Exception)
                    {
                        throw;
                    }
                }
            }
        }
    }
}
