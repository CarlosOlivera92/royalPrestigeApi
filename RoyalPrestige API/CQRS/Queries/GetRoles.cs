using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using RoyalPrestige_API.Data;
using RoyalPrestige_API.DTO;

namespace RoyalPrestige_API.CQRS.Queries
{
    public class GetRoles
    {
        public class GetRolesQuery : IRequest<List<RolDTO>>
        {

        }
        public class GetRolesQueryHandler : IRequestHandler<GetRolesQuery, List<RolDTO>>
        {
            private readonly ApplicationContext _context;
            private readonly IMapper _mapper;

            public GetRolesQueryHandler(ApplicationContext context, IMapper mapper)
            {
                _context = context;
                _mapper = mapper;
            }
            public async Task<List<RolDTO>> Handle(GetRolesQuery request, CancellationToken cancellationToken)
            {
                var roles = await _context.Roles.ToListAsync();
                var rolesDTO = roles.Select(u => new RolDTO
                {
                    Id = u.Id,
                    Rol = u.Rol,
                }).ToList();
                return rolesDTO;
            }
        }
    }
}
