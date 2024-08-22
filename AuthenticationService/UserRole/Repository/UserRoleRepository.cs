using AuthenticationService.Api.UserRole.Domain;
using Dapper;
using Oracle.ManagedDataAccess.Client;

namespace AuthenticationService.Api.UserRole.Repository
{
    public class UserRoleRepository : IUserRoleRepository
    {
        private readonly string? _connectionString;

        public UserRoleRepository(IConfiguration configuration)
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

                var insertModel = new
                {
                    COD_USUARIO = createUserRole.UserId,
                    COD_ROL = createUserRole.RoleId,
                    ADICIONADO_POR = createUserRole.CreatedBy,
                    FECHA_ADICIONADO = DateTime.Now,
                    ES_ACTIVO = 1
                };
                await connection.ExecuteAsync(insertQuery, insertModel);
                return true;

            }
            //AQUI IRA EL SERILOG, PARA GUARDAR UN LOG EN CASO DE QUE FALLE LA APLICACION.
            catch (Exception ex)
            {
                throw new ApplicationException(ex.Message, ex);
            }
        }

        public async Task<bool> DeleteUserRoleAsync(DeleteUserRole deleteUserRole)
        {
            {
                try
                {
                    using OracleConnection connection = new(_connectionString);
                    const string updateQuery = @"UPDATE C##GENIUS.USUARIOROLES SET ES_ACTIVO = 0 AND MODIFICADO_POR = :modifiedBy 
                    AND FECHA_MODIFICADO = :dateModified WHERE COD_ROL = :codeRol AND COD_USUARIO = :codeUser";

                    var parameters = new
                    {
                        modifiedBy = deleteUserRole.ModifiedBy,
                        codeUser = deleteUserRole.UserId,
                        codeRol = deleteUserRole.RoleId,
                        dateModified = DateTime.Now
                    };

                    var isSuccess = await connection.ExecuteAsync(updateQuery, parameters);
                    return isSuccess == 1;
                }
                catch (Exception ex)
                {
                    throw new ApplicationException(ex.Message, ex);
                }
            }
        }
        public async Task<List<GetUserRole>> GetUserRolesAsync(string userId)
        {
            try
            {
                using OracleConnection connection = new(_connectionString);
                const string selectQuery = @"SELECT R.NOMBRE AS Name FROM C##GENIUS.USUARIOROLES UR
                INNER JOIN C##GENIUS.ROLES R ON R.COD_ROL = UR.COD_ROL WHERE COD_USUARIO = :userId AND UR.ES_ACTIVO = 1";
                var userRoles = await connection.QueryAsync<GetUserRole>(selectQuery, new { userId });
                return userRoles.ToList();
            }
            catch (Exception ex)
            {
                throw new ApplicationException(ex.Message, ex);
            }
        }
    }
}
