using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RoyalPrestige_API.DTO;
using static api.CQRS.Command.PostUsuario;
using static RoyalPrestige_API.CQRS.Command.PostRoles;
using static RoyalPrestige_API.CQRS.Queries.GetRoles;
using static RoyalPrestige_API.CQRS.Queries.GetUsuarios;

namespace RoyalPrestige_API.Controllers
{
    [ApiController]
    [Authorize (Roles = ("ADMIN") )]
    [Route("api/[controller]")]
    public class RolesController : ControllerBase
    {
        private readonly IMediator _mediator;
        public RolesController(IMediator mediator)
        {
            _mediator = mediator;

        }
        [HttpPost]
        [Route("/create")]
        public async Task<IActionResult> Create(PostRolesCommand cmd)
        {
            try
            {
                var roleDTO = await _mediator.Send(cmd);
                return CreatedAtAction(nameof(Create), roleDTO);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [HttpGet]
        public async Task<List<RolDTO>> GetRoles()
        {
            return await _mediator.Send(new GetRolesQuery());
        }
    }
}
