using AuthenticationService.Api.Roles;
using AuthenticationService.Api.Roles.Interface;
using AuthenticationService.Application.Response;
using Dapper;
using Oracle.ManagedDataAccess.Client;

namespace AuthenticationService.Api.Roles.Repository
{
    public class RolRepository(IConfiguration configuration) : IRoleRepository
    {
        public async Task<Rol> AddAsync(Rol rol)
        {
            try
            {
                using OracleConnection connection = new(configuration.GetConnectionString("OracleConnection"));
                const string query = @"INSERT INTO C##GENIUS.ROLES(COD_ROL,NOMBRE,FECHA_ADICIONADO,ADICIONADO_POR,ES_ACTIVO)
                VALUES(:COD_ROL,:NOMBRE,:FECHA_ADICIONADO,:ADICIONADO_POR,:ES_ACTIVO)";

                var parameters = new
                {
                    COD_ROL = Guid.NewGuid().ToString(),
                    NOMBRE = rol.Name,
                    FECHA_ADICIONADO = DateTime.UtcNow,
                    ADICIONADO_POR = "Gerald Silverio",
                    ES_ACTIVO = 1
                };
                int result = await connection.ExecuteAsync(query, parameters);
                if (result > 0)
                {
                    rol.CodeRol = parameters.COD_ROL;
                    return rol;
                }
                return rol;
            }
            catch (Exception ex)
            {
                throw new ApplicationException(ex.Message, ex);
            }
        }

        public async Task<List<Rol>> GetAsync()
        {
            try
            {
                using OracleConnection connection = new(configuration.GetConnectionString("OracleConnection"));
                const string query = "SELECT COD_ROL as CodeRol, NOMBRE as Name FROM C##GENIUS.ROLES WHERE ES_ACTIVO = 1";
                var roles = await connection.QueryAsync<Rol>(query);
                return roles.ToList();
            }
            catch (Exception ex)
            {
                throw new ApplicationException(ex.Message, ex);
            }
        }
    }
}
