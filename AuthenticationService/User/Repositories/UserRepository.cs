using AuthenticationService.Api.User.Command;
using AuthenticationService.Api.User.Domain;
using AuthenticationService.Api.User.Repositories;
using Dapper;
using Oracle.ManagedDataAccess.Client;

namespace AuthenticationService.Application.Services
{
    public class UserRepository : IUserRepository
    {
        private readonly string? _connectionString;

        public UserRepository(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("OracleConnection");
        }

        public async Task<string> CreateUserAsync(RegisterUserCommand user, string fireBaseCode)
        {
            try
            {
                using OracleConnection connection = new(_connectionString);
                var parameters = new
                {
                    COD_USUARIO = Guid.NewGuid().ToString(),
                    NOMBRES = user.Names,
                    APELLIDOS = user.LastNames,
                    COD_FIREBASE = fireBaseCode,
                    NOMBRE_USUARIO = user.UserName,
                    TELEFONO = user.PhoneNumber,
                    CORREO_ELECTRONICO = user.Email,
                    IDENTIFICACION = user.Identification,
                    SEXO = user.Gender,
                    DIRECCION = user.Address,
                    COD_POSTAL = user.ZipCode,
                    IMAGEN = user.PhotoUrl,
                    COD_PAIS = user.CountryId,
                    COD_NEGOCIO = user.BusinessId,
                    COD_TIPO_IDENTIFICACION = user.TypeIdentificationId,
                    ADICIONADO_POR = user.CreateBy,
                    FECHA_ADICIONADO = DateTime.Now,
                    ES_ACTIVO = 1
                };
                const string insertUserQuery = @"INSERT INTO C##GENIUS.USUARIOS
                (COD_USUARIO,NOMBRES,APELLIDOS,NOMBRE_USUARIO,TELEFONO,CORREO_ELECTRONICO,IDENTIFICACION,COD_FIREBASE,SEXO,DIRECCION,COD_POSTAL,IMAGEN,COD_PAIS,
                COD_NEGOCIO,COD_TIPO_IDENTIFICACION,ADICIONADO_POR,FECHA_ADICIONADO,ES_ACTIVO) 
                VALUES(:COD_USUARIO,:NOMBRES,:APELLIDOS,:NOMBRE_USUARIO,:TELEFONO,:CORREO_ELECTRONICO,:IDENTIFICACION,:COD_FIREBASE,:SEXO,:DIRECCION,:COD_POSTAL,
                :IMAGEN,:COD_PAIS,:COD_NEGOCIO,:COD_TIPO_IDENTIFICACION,:ADICIONADO_POR,:FECHA_ADICIONADO,:ES_ACTIVO)";

                int result = await connection.ExecuteAsync(insertUserQuery, parameters);
                connection.Dispose();
                return result != 0 ? parameters.COD_USUARIO : null;
            }
            catch (Exception ex)
            {
                throw new ApplicationException(ex.Message, ex);
            }
        }

        public async Task<User> GetUserAsync(string email)
        {
            try
            {
                using OracleConnection connection = new(_connectionString);
                const string selectQuery = @"SELECT U.COD_USUARIO AS UserId,U.NOMBRES AS Name,U.APELLIDOS AS LastName,U.IDENTIFICACION AS Identification,N.NOMBRE AS Business,P.NOMBRE AS Country,
                U.DIRECCION AS Address,U.COD_PAIS AS CountryId,U.COD_NEGOCIO BusinessId,U.COD_FIREBASE AS FirebaseId
                FROM C##GENIUS.USUARIOS U
                INNER JOIN C##GENIUS.NEGOCIOS N ON U.COD_NEGOCIO  = N.COD_NEGOCIO
                INNER JOIN C##GENIUS.PAISES P ON U.COD_PAIS = P.COD_PAIS
                WHERE U.CORREO_ELECTRONICO  = :email";
                User? result = await connection.QueryFirstOrDefaultAsync<User>(selectQuery, new { email });
                connection.Dispose();
                return result;
            }
            catch (Exception ex)
            {
                throw new ApplicationException(ex.Message, ex);
            }
        }
    }
}
