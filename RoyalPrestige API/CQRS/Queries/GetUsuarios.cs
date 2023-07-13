using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using RoyalPrestige_API.Data;
using RoyalPrestige_API.DTO;

namespace RoyalPrestige_API.CQRS.Queries
{
    public class GetUsuarios
    {
        public class GetUusuariosQuery : IRequest<List<UsuarioDTO>>
        {

        }
        public class GetUsuariosQueryHandler : IRequestHandler<GetUusuariosQuery, List<UsuarioDTO>>
        {
            private readonly ApplicationContext _context;
            private readonly IMapper _mapper;

            public GetUsuariosQueryHandler(ApplicationContext context, IMapper mapper)
            {
                _context = context;
                _mapper = mapper;
            }
            public async Task<List<UsuarioDTO>> Handle(GetUusuariosQuery request, CancellationToken cancellationToken)
            {
                var usuarios = await _context.Usuarios.ToListAsync();
                var usuariosDTO = usuarios.Select(u => new UsuarioDTO
                {
                    Id = u.Id,
                    Nombre = u.Nombre,
                    Apellido = u.Apellido,
                    Username = u.Username,
                    Email = u.Email,
                    ProfilePic = u.ProfilePic,
                    FechaNacimiento = u.FechaNacimiento,
                    Telefono = u.Telefono,
                    RolId = u.RolId,
                }).ToList();
                return usuariosDTO;
            }
        }
    }
}
