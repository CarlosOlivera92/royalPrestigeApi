using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RoyalPrestige_API.CQRS.Command;
using RoyalPrestige_API.DTO;
using static api.CQRS.Command.PostLoginUsuario;
using static api.CQRS.Command.PostUsuario;
using static RoyalPrestige_API.CQRS.Command.DeleteUsuario;
using static RoyalPrestige_API.CQRS.Command.PutUsuario;
using static RoyalPrestige_API.CQRS.Queries.GetUsuarios;

namespace api.Controller
{
    [ApiController]
    [Route("api/[controller]")]
    public class UsuarioController : ControllerBase
    {
        private readonly IMediator _mediator;
        public UsuarioController(IMediator mediator)
        {
            _mediator = mediator;

        }
        [HttpPost]
        [Route("/register")]
        public async Task<IActionResult> Register(PostUsuarioCommand cmd)
        {
            try
            {
                var usuarioDTO = await _mediator.Send(cmd);
                return CreatedAtAction(nameof(Register), usuarioDTO);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [HttpPost]
        [Route("/login")]
        public async Task<IActionResult> Login(PostLoginUsuarioCommand cmd)
        {
            try
            {
                var postP = await _mediator.Send(cmd);
                return CreatedAtAction(nameof(Login), postP);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [HttpGet]
        [Authorize (Roles = ("ADMIN") ) ]
        public async Task<List<UsuarioDTO>> GetUsuarios()
        {
            return await _mediator.Send(new GetUusuariosQuery());
        }
        [HttpPut]
        [Authorize (Roles = "ADMIN" )]
        public async Task<IActionResult> EditUsuario(PutUsuarioCommand cmd)
        {
            try
            {
                var putP = await _mediator.Send(cmd);
                return CreatedAtAction(nameof(EditUsuario), putP);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [HttpDelete]
        public async Task<IActionResult> DeleteUsuario(DeleteUsuarioCommand cmd)
        {
            await _mediator.Send(new DeleteUsuario.DeleteUsuarioCommand { Id = cmd.Id });
            return NoContent();
        }
    }
}
