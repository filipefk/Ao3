using System;
using System.Threading.Tasks;
using Ao3RentcarsApi.Helpers;
using Ao3RentcarsApi.Models;
using Ao3RentcarsApi.Dao;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Ao3RentcarsApi.Models.Dto;

namespace Ao3RentcarsApi.Controllers
{
    /// <summary>
    /// Faz o controle de acesso gerando um Bearer Token Authorization
    /// </summary>
    /// <remarks>
    /// Usando o pacote Microsoft.AspNetCore.Authentication.JwtBearer
    /// </remarks>
    [Route("api/[controller]")]
    [ApiController]
    public class LoginController : ControllerBase
    {
        private readonly UsuarioDao _dao;

        /// <summary>
        /// Construtor da classe
        /// </summary>
        /// <remarks>
        /// Recebe o contexto e instancia a classe dao passando o contexto
        /// </remarks>
        /// <param name="context">
        /// RentcarsContext
        /// </param>
        public LoginController(RentcarsContext context)
        {
            _dao = new UsuarioDao(context);
        }

        /// <summary>
        /// Valida o usuário e senha e gera um token para uso nas consultas que necessitam autenticação
        /// </summary>
        /// <remarks>
        /// O token tem validade de 10 horas e pode ser guardado em um cookie
        /// </remarks>
        /// <param name="loginDto">
        /// As propriedades do loginDto são case sensitive
        /// </param>
        /// <returns>Json com o token e o UsuarioDto que logou</returns>
        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> Login(LoginDto loginDto)
        {
            try
            {
                Usuario usuario = await _dao.BuscaPorLogin(loginDto.Login);

                if (usuario == null)
                    return BadRequest(new { Message = "Login e/ou senha inválido(s)." });

                if (usuario.Senha != loginDto.Senha)
                    return BadRequest(new { Message = "Login e/ou senha inválido(s)." });

                var token = JwtAuth.GenerateToken(usuario);

                return Ok(new
                {
                    Token = token,
                    Usuario = UsuarioDto.ToDto(usuario)
                });

            }
            catch (Exception ex)
            {
                string msg = ex.Message;
                if (ex.InnerException != null)
                {
                    msg += " - " + ex.InnerException.Message;
                }
                return BadRequest(new { Message = "Ocorreu algum erro na autenticação, por favor tente novamente. - " + msg });
            }
        }
    }
}

