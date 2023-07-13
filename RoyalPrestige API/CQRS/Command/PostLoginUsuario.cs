
using AutoMapper;
using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;
using RoyalPrestige_API.Data;
using RoyalPrestige_API.DTO;

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
            public PostLoginUsuarioCommandHandler(ApplicationContext context, IMapper mapper, IValidator<PostLoginUsuarioCommand> validator)
            {
                _context = context;
                _mapper = mapper;
                _validator = validator;
            }

            public async Task<UsuarioDTO> Handle(PostLoginUsuarioCommand request, CancellationToken cancellationToken)
            {
                try
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
                    var usuarioDto = _mapper.Map<UsuarioDTO>(Usuario);
                    return usuarioDto;
                }
                catch (Exception)
                {   
                    throw;
                }
            }
        }
    }
}
