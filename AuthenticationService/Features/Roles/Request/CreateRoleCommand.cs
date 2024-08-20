using AuthenticationService.Api.Entities;
using AuthenticationService.Api.Results;
using AuthenticationService.Application.Interfaces;
using MediatR;

namespace AuthenticationService.Api.Features.Roles.Request
{
    public class CreateRoleCommand : IRequest<Result<Rol>>
    {
        public string Name { get; set; }
    }
    public sealed class CreateRoleCommandHandler : IRequestHandler<CreateRoleCommand, Result<Rol>> 
    {
        private readonly IRoleRepository _rolRepository;

        public CreateRoleCommandHandler(IRoleRepository rolRepository)
        {
            _rolRepository = rolRepository;
        }

        public async Task<Result<Rol>> Handle(CreateRoleCommand request, CancellationToken cancellationToken)
        {

            try
            {
                Rol rol = new()
                {
                    Name = request.Name
                };
                rol = await _rolRepository.AddAsync(rol);
                return new Result<Rol>(rol, 201);
            }
            catch (Exception ex)
            {
                return new Result<Rol>(ex);
            }
        }
    }
}
