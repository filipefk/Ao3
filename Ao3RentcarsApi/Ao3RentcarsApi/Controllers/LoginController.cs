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
    [Route("api/[controller]")]
    [ApiController]
    public class LoginController : ControllerBase
    {
        private readonly UsuarioDao _dao;

        public LoginController(RentcarsContext context)
        {
            _dao = new UsuarioDao(context);
        }

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

