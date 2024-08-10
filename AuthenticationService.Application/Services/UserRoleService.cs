using AuthenticationService.Application.Dtos.UserRole;
using AuthenticationService.Application.Interfaces;
using Dapper;
using Microsoft.Extensions.Configuration;
using Oracle.ManagedDataAccess.Client;

namespace AuthenticationService.Application.Services
{
    public class UserRoleService : IUserRoleService
    {
        private readonly string? _connectionString;

        public UserRoleService(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("OracleConnection");
        }
        public async Task<bool> CreateUserRoleAsync(CreateUserRole createUserRole)
        {
            try
            {
                using OracleConnection connection = new(_connectionString);
                const string insertQuery = @"INSERT INTO C##GENIUS.USUARIOROLES
                (COD_USUARIO,COD_ROL,ADICIONADO_POR,FECHA_ADICIONADO,ES_ACTIVO)
                VALUES(:COD_USUARIO,:COD_ROL,:ADICIONADO_POR,:FECHA_ADICIONADO,:ES_ACTIVO)";

                foreach (string rolId in createUserRole.RolesId)
                {
                    var insertModel = new
                    {
                        COD_USUARIO = createUserRole.UserId,
                        COD_ROL = rolId,
                    };
                    await connection.ExecuteAsync(insertQuery, insertModel);
                }
                return true;

            }
            //AQUI IRA EL SERILOG, PARA GUARDAR UN LOG EN CASO DE QUE FALLE LA APLICACION.
            catch (Exception ex)
            {
                throw new ApplicationException(ex.Message, ex);
            }
        }
    }
}
