using AuthenticationService.Application.Dtos.Account;
using AuthenticationService.Application.Interfaces;
using Dapper;
using FirebaseAdmin.Messaging;
using FluentValidation;
using Microsoft.Extensions.Configuration;
using Oracle.ManagedDataAccess.Client;

namespace AuthenticationService.Application.Services
{
    public class UserService : IUserService
    {
        private readonly string? _connectionString;

        public UserService(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("OracleConnection");
        }

        public async Task<string> CreateUserAsync(RegisterUserDto user, string fireBaseCode)
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
                    ADICIONADO_POR = "AppWeb",
                    FECHA_ADICIONADO = DateTime.Now,
                    ES_ACTIVO = 1
                };
                const string insertUserQuery = @"INSERT INTO C##GENIUS.USUARIOS
                (COD_USUARIO,NOMBRES,APELLIDOS,NOMBRE_USUARIO,TELEFONO,CORREO_ELECTRONICO,IDENTIFICACION,COD_FIREBASE,SEXO,DIRECCION,COD_POSTAL,IMAGEN,COD_PAIS,
                COD_NEGOCIO,COD_TIPO_IDENTIFICACION,ADICIONADO_POR,FECHA_ADICIONADO,ES_ACTIVO) 
                VALUES(:COD_USUARIO,:NOMBRES,:APELLIDOS,:NOMBRE_USUARIO,:TELEFONO,:CORREO_ELECTRONICO,:IDENTIFICACION,:COD_FIREBASE,:SEXO,:DIRECCION,:COD_POSTAL,
                :IMAGEN,:COD_PAIS,:COD_NEGOCIO,:COD_TIPO_IDENTIFICACION,:ADICIONADO_POR,:FECHA_ADICIONADO,:ES_ACTIVO)";

                int result = await connection.ExecuteAsync(insertUserQuery, parameters);
                return result != 0 ? parameters.COD_USUARIO : null;

            }
            catch (Exception ex)
            {
                throw new ApplicationException(ex.Message, ex);
            }
        }
    }
}
