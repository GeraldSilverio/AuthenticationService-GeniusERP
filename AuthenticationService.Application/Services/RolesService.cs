using AuthenticationService.Application.Dtos.Roles;
using AuthenticationService.Application.Interfaces;
using AuthenticationService.Application.Response;
using Dapper;
using Microsoft.Extensions.Configuration;
using Oracle.ManagedDataAccess.Client;

namespace AuthenticationService.Application.Services
{
    public class RolesService(IConfiguration configuration) : IRolesService
    {
        public async Task<Response<CreateRolDto>> AddRolAsync(CreateRolDto createRolDto)
        {
            try
            {
                using OracleConnection connection = new(configuration.GetConnectionString("OracleConnection"));
                const string query = @"INSERT INTO C##GENIUS.ROLES(COD_ROL,NOMBRE,FECHA_ADICIONADO,ADICIONADO_POR,ES_ACTIVO)
                VALUES(:COD_ROL,:NOMBRE,:FECHA_ADICIONADO,:ADICIONADO_POR,:ES_ACTIVO)";

                var parameters = new
                {
                    COD_ROL = Guid.NewGuid().ToString(),
                    NOMBRE = createRolDto.Name,
                    FECHA_ADICIONADO = DateTime.Now,
                    ADICIONADO_POR = "Gerald Silverio",
                    ES_ACTIVO = 1
                };

                int result = await connection.ExecuteAsync(query, parameters);
                if (result == 0)
                {
                    return new Response<CreateRolDto>(new List<string> { "No se pudo crear el rol deseado" }, 400, createRolDto);
                }

                createRolDto.CodeRol = parameters.COD_ROL;
                return new Response<CreateRolDto>(createRolDto, 201);
            }
            catch (Exception ex)
            {
                return new Response<CreateRolDto>(new List<string> { "No se pudo crear el rol deseado, favor revisar los LOGS.", ex.Message }, 400, createRolDto);
            }
        }

        public async Task<Response<List<RolDto>>> GetRolesAsync()
        {
            try
            {
                using OracleConnection connection = new(configuration.GetConnectionString("OracleConnection"));
                const string query = "SELECT COD_ROL as CodeRol, NOMBRE as Name FROM C##GENIUS.ROLES WHERE ES_ACTIVO = 1";
                var roles = await connection.QueryAsync<RolDto>(query);

                if (roles.ToList().Count > 0)
                {
                    return new Response<List<RolDto>>(roles.ToList(), 200);
                }

                return new Response<List<RolDto>>(new List<string> { "No se encontraron roles en el sistema" }, 400, roles.ToList());

            }
            catch (Exception ex)
            {
                throw new NotImplementedException();
            }
        }
    }
}
