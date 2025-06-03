using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dapper;
using MySqlConnector;
using System.Configuration;
using Entities;
using System.Data;


namespace DataAccess
{
    public class UsuarioDA
    {
        private readonly string _connectionString; // Para conexión a la base de datos
        public UsuarioDA(string connectionString)
        {
            _connectionString = connectionString; // Inicializa la cadena de conexión desde program.cs
        }

        // Para login, verifica las credenciales del usuario
        public CredentialsUsuario VerifyUsuario(string nombreUsuario)
        {
            if (string.IsNullOrWhiteSpace(nombreUsuario))
                throw new ArgumentException("Nombre de usuario inválido", nameof(nombreUsuario));

            using (var connection = new MySqlConnection(_connectionString))
            {
                connection.Open();

                var parameters = new DynamicParameters();

                // Parámetro de entrada
                parameters.Add("@pI_nombreUsuario", nombreUsuario, DbType.String, ParameterDirection.Input);

                // Parámetros de salida
                parameters.Add("@pS_nombreUsuario", dbType: DbType.String, size: 100, direction: ParameterDirection.Output);
                parameters.Add("@pS_contrasenia_cifrada", dbType: DbType.Binary, size: 256, direction: ParameterDirection.Output);
                parameters.Add("@pS_nonce", dbType: DbType.Binary, size: 12, direction: ParameterDirection.Output);
                parameters.Add("@pS_encontrado", dbType: DbType.Int32, direction: ParameterDirection.Output);

                // Ejecutar el procedimiento almacenado
                connection.Execute("verificar_credencial", parameters, commandType: CommandType.StoredProcedure);

                // Verificar si se encontró el usuario y asgnar los valores de salida a entities
                return new CredentialsUsuario
                {
                    NombreUsuario = parameters.Get<string>("@pS_nombreUsuario"),
                    ContraseniaCifrada = parameters.Get<byte[]>("@pS_contrasenia_cifrada"),
                    Nonce = parameters.Get<byte[]>("@pS_nonce"),
                    Encontrado = parameters.Get<int>("@pS_encontrado")
                };
            }
        }
    }
}
