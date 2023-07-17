using MediatR;
using RoyalPrestige_API.Data;

namespace RoyalPrestige_API.CQRS.Command
{
    public class DeleteUsuario
    {
        public class DeleteUsuarioCommand : IRequest<Unit>
        {
            public long Id { get; set; } 
        }
        public class DeleteUsuarioCommandHandler : IRequestHandler<DeleteUsuarioCommand, Unit>
        {
            private readonly ApplicationContext _context;

            public DeleteUsuarioCommandHandler(ApplicationContext context)
            {
                _context = context;
            }

            public async Task<Unit> Handle(DeleteUsuarioCommand command, CancellationToken cancellationToken)
            {
                try
                {
                    // Busca el usuario en la base de datos por su ID
                    var usuario = await _context.Usuarios.FindAsync(command.Id);

                    // Si el usuario no existe, puedes lanzar una excepción o manejar el error según tus necesidades
                    if (usuario == null)
                    {
                        throw new Exception("El usuario no existe");
                    }

                    // Elimina el usuario de la base de datos
                    _context.Usuarios.Remove(usuario);
                    await _context.SaveChangesAsync(cancellationToken);

                    return Unit.Value;
                }
                catch (Exception)
                {
                    throw;
                }
            }
        }
    }
}
