﻿using Oracle.ManagedDataAccess.Client;
using APIRest.Helpers;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using APIRest.Models;

namespace APIRest.Services
{
    public class UserService
    {
        private readonly OracleDbHelper _dbHelper;
        private readonly IConfiguration _configuration;

        public UserService(OracleDbHelper dbHelper, IConfiguration configuration)
        {
            _dbHelper = dbHelper;
            _configuration = configuration;
        }

        public async Task<bool> ValidateUserAsync(string email, string password)
        {
            const string query = "SELECT PASSWORD, SALT FROM TB_USERS WHERE EMAIL = :Email";

            using (var connection = _dbHelper.GetConnection())
            {
                connection.Open();

                using (var command = new OracleCommand(query, connection as OracleConnection))
                {
                    command.Parameters.Add(new OracleParameter("Email", email));

                    using (var reader = await command.ExecuteReaderAsync())
                    {
                        if (await reader.ReadAsync())
                        {
                            string storedHash = reader.GetString(0);
                            string storedSalt = reader.GetString(1);
                            return EncryptionHelper.VerifyPassword(password, storedHash, storedSalt);
                        }
                    }
                }
            }

            return false;
        }

        public async Task<string> GetUserRoleAsync(string email, string password)
        {
            var encryptedPassword = EncryptionHelper.HashPassword(password);

            const string query = "SELECT ROLE FROM TB_USERS WHERE EMAIL = :Email";

            using (var connection = _dbHelper.GetConnection())
            {
                connection.Open();

                using (var command = new OracleCommand(query, connection as OracleConnection))
                {
                    command.Parameters.Add(new OracleParameter("Email", email));

                    var result = await command.ExecuteScalarAsync();
                    return result?.ToString() ?? string.Empty;
                }
            }
        }

        public async Task<PasswordsModel> GetUserByEmailAsync(string email)
        {
            const string query = "SELECT PASSWORD, SALT FROM TB_USERS WHERE EMAIL = :Email";

            using (var connection = _dbHelper.GetConnection())
            {
                connection.Open();

                using (var command = new OracleCommand(query, connection as OracleConnection))
                {
                    command.Parameters.Add(new OracleParameter("Email", email));

                    var reader = await command.ExecuteReaderAsync();
                    Console.WriteLine(reader);
                    if (reader.HasRows)
                    {
                        await reader.ReadAsync();

                        var password = reader.GetString(0);
                        var salt = reader.GetString(1);

                        return new PasswordsModel { Password = password, Salt = salt };
                    }
                    else
                    {
                        return null;
                    }
                }
            }
        }

        public async Task AddUserAsync(string email, string password, string salt, string role)
        {
            const string query = @"
                INSERT INTO TB_USERS (USER_ID, EMAIL, PASSWORD, SALT, ROLE)
                VALUES (SEQ_USERS.NEXTVAL, :Email, :Password, :SALT, :Role)";

            using (var connection = _dbHelper.GetConnection())
            {
                connection.Open();

                using (var command = new OracleCommand(query, connection as OracleConnection))
                {
                    command.Parameters.Add(new OracleParameter("Email", email));
                    command.Parameters.Add(new OracleParameter("Password", password));
                    command.Parameters.Add(new OracleParameter("Salt", salt));
                    command.Parameters.Add(new OracleParameter("Role", role));

                    await command.ExecuteNonQueryAsync();
                }
            }
        }

        private string GetSecretKey()
        {
            return _configuration["JwtSettings:SecretKey"];
        }

        public async Task<string> GenerateJwtTokenAsync(string email, string role)
        {
            var secretKey = Encoding.UTF8.GetBytes(GetSecretKey());
            var signingKey = new SymmetricSecurityKey(secretKey);

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new[]
                {
                new Claim(ClaimTypes.Email, email),
                new Claim(ClaimTypes.Role, role)
            }),
                Expires = DateTime.UtcNow.AddHours(1),
                SigningCredentials = new SigningCredentials(signingKey, SecurityAlgorithms.HmacSha256)
            };

            var tokenHandler = new JwtSecurityTokenHandler();
            var token = tokenHandler.CreateToken(tokenDescriptor);

            return tokenHandler.WriteToken(token);
        }
    }
}
