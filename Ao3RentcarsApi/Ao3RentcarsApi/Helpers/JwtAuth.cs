﻿using System;
using System.Text;
using System.Security.Claims;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.IdentityModel.Tokens;
using Ao3RentcarsApi.Models;

namespace Ao3RentcarsApi.Helpers
{
    public static class JwtAuth
    {
        public static string GenerateToken(Usuario usuario)
        {
            var tokenHandler = new JwtSecurityTokenHandler();

            //var key = Encoding.ASCII.GetBytes(AppData.Configuration["ControleAcesso:ChaveSecreta"]);
            var key = Encoding.ASCII.GetBytes("ZWRpw6fDo28gZW0gY29tcHV0YWRvcmE="); // <= ToDo Coloquei fixo aqui porque não tava conseguindo injetar nos testes

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new Claim[]
                {
                    new Claim(ClaimTypes.Name, usuario.Login.ToString()),
                    new Claim(ClaimTypes.Role, RoleFactory(usuario.Tipo))
                }),
                Expires = DateTime.UtcNow.AddHours(10),

                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };

            var token = tokenHandler.CreateToken(tokenDescriptor);

            return tokenHandler.WriteToken(token);
        }

        private static string RoleFactory(int roleNumber)
        {
            switch (roleNumber)
            {
                case 1:
                    return "Funcionario";

                default:
                    throw new Exception();
            }
        }
    }
}
