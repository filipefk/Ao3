using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Ao3RentcarsApi.Models;
using Ao3RentcarsApi.Models.Dto;
using Microsoft.Data.Sqlite;
using Ao3RentcarsApi.Dao;
using Microsoft.AspNetCore.Authorization;

namespace Ao3RentcarsApi.Controllers
{
    /// <summary>
    /// Faz o CRUD dos Usuários
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    public class UsuariosController : ControllerBase
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
        public UsuariosController(RentcarsContext context)
        {
            _dao = new UsuarioDao(context);
        }

        /// <summary>
        /// Rota GET: api/Usuarios
        /// </summary>
        /// <remarks>
        /// Rota protegida. Deve ser inserido no Header a chave "Authorization" e o valor "Bearer token". o token é obtido na rota api/Login <br/>
        /// ATENÇÃO!!! Coloque a palavra "Bearer", um espaço e depois o token no valor do header. <br/>
        /// Se estiver usando o Swagger tem um botão no topo, a direita escrito "Authorize", clique nele e preencha com a palavra "Bearer", um espaço e depois o token
        /// </remarks>
        /// <returns>
        /// Retorna uma lista de todos os Usuários cadastrados. Obs.: Não mostra a senha dos usuários
        /// </returns>
        [HttpGet]
        [Authorize]
        public async Task<ActionResult<IEnumerable<UsuarioDto>>> ListaTodos()
        {
            try
            {
                return UsuarioDto.ToDtoList(await _dao.ListaTodos());
            }
            catch (SqliteException sqlLex)
            {
                string msg = sqlLex.Message;
                if (sqlLex.InnerException != null)
                {
                    msg += " - " + sqlLex.InnerException.Message;
                }
                throw new ArgumentException("Problema de acesso ao banco de dados - " + msg);
            }
            catch (Exception ex)
            {
                string msg = ex.Message;
                if (ex.InnerException != null)
                {
                    msg += " - " + ex.InnerException.Message;
                }
                throw new ArgumentException("Erro ao tentar buscar a lista de usuários - " + msg);
            }

        }

        /// <summary>
        /// Rota GET: api/Usuarios/{id}
        /// </summary>
        /// <remarks>
        /// Rota protegida. Deve ser inserido no Header a chave "Authorization" e o valor "Bearer token". o token é obtido na rota api/Login <br/>
        /// ATENÇÃO!!! Coloque a palavra "Bearer", um espaço e depois o token no valor do header. <br/>
        /// Se estiver usando o Swagger tem um botão no topo, a direita escrito "Authorize", clique nele e preencha com a palavra "Bearer", um espaço e depois o token
        /// </remarks>
        /// <param name="id">
        /// Id do Usuário
        /// </param>
        /// <returns>
        /// Retorna o Usuário do id informado
        /// </returns>
        [HttpGet("{id}")]
        [Authorize]
        public async Task<ActionResult<UsuarioDto>> Busca(int id)
        {
            try
            {
                Usuario usuario = await _dao.Busca(id);

                if (usuario == null)
                {
                    return NotFound();
                }

                UsuarioDto usuarioDto = UsuarioDto.ToDto(usuario);

                return usuarioDto;
            }
            catch (SqliteException sqlLex)
            {
                string msg = sqlLex.Message;
                if (sqlLex.InnerException != null)
                {
                    msg += " - " + sqlLex.InnerException.Message;
                }
                throw new ArgumentException("Problema de acesso ao banco de dados - " + msg);
            }
            catch (Exception ex)
            {
                string msg = ex.Message;
                if (ex.InnerException != null)
                {
                    msg += " - " + ex.InnerException.Message;
                }
                throw new ArgumentException("Erro ao tentar buscar o usuário de id = " + id + " - " + msg);
            }
        }

        /// <summary>
        /// Rota PUT: api/Usuarios/{id}
        /// Altera os dados do Usuário do id informado
        /// </summary>
        /// <remarks>
        /// Rota protegida. Deve ser inserido no Header a chave "Authorization" e o valor "Bearer token". o token é obtido na rota api/Login <br/>
        /// ATENÇÃO!!! Coloque a palavra "Bearer", um espaço e depois o token no valor do header. <br/>
        /// Se estiver usando o Swagger tem um botão no topo, a direita escrito "Authorize", clique nele e preencha com a palavra "Bearer", um espaço e depois o token
        /// As propriedades do Usuário não informadas serão ignoradas <br/>
        /// O Id e DataInclusao do Json sempre são ignorados <br/>
        /// A DataAlteracao é preenchida automaticamente, mesmo que seja informada <br/>
        /// Se informados, o Nome, Login e Senha do Usuário devem ter no mínimo 4 caracteres <br/>
        /// Não é permitido o cadastro de Login repetido
        /// </remarks>
        /// <param name="id">
        /// id do Usuário a ser alterado
        /// </param>
        /// <param name="usuarioDto">
        /// Dados do Usuário que devem ser alterados
        /// </param>
        /// <returns>
        /// Retorna o Usuário com os dados alterados
        /// </returns>
        [HttpPut("{id}")]
        [Authorize]
        public async Task<IActionResult> Altera(int id, UsuarioDto usuarioDto)
        {
            try
            {
                Usuario usuario = await _dao.Busca(id);

                if (usuario == null)
                {
                    return NotFound();
                }

                usuario = UsuarioDto.PutEntity(usuario, usuarioDto);
                usuario.DataAlteracao = DateTime.Now;
                Valida(usuario);

                await _dao.Altera(usuario);

                usuarioDto = UsuarioDto.ToDto(usuario);

                return CreatedAtAction(nameof(Altera), new { id = usuarioDto.Id }, usuarioDto);
            }
            catch (DbUpdateException dbUex)
            {
                string msg = dbUex.Message;
                if (dbUex.InnerException != null)
                {
                    msg += " - " + dbUex.InnerException.Message;
                }
                throw new ArgumentException("Erro ao tentar salvar no banco - " + msg);
            }
            catch (SqliteException sqlLex)
            {
                string msg = sqlLex.Message;
                if (sqlLex.InnerException != null)
                {
                    msg += " - " + sqlLex.InnerException.Message;
                }
                throw new ArgumentException("Problema de acesso ao banco de dados - " + msg);
            }
            catch (Exception ex)
            {
                string msg = ex.Message;
                if (ex.InnerException != null)
                {
                    msg += " - " + ex.InnerException.Message;
                }
                throw new ArgumentException("Erro ao tentar alterar o usuário de id = " + id + " - " + msg);
            }
        }

        /// <summary>
        /// Rota POST: api/Usuarios
        /// Insere um novo Usuário
        /// </summary>
        /// <remarks>
        /// Rota protegida. Deve ser inserido no Header a chave "Authorization" e o valor "Bearer token". o token é obtido na rota api/Login <br/>
        /// ATENÇÃO!!! Coloque a palavra "Bearer", um espaço e depois o token no valor do header. <br/>
        /// Se estiver usando o Swagger tem um botão no topo, a direita escrito "Authorize", clique nele e preencha com a palavra "Bearer", um espaço e depois o token
        /// O Id, DataInclusao e DataAlteracao são preenchidos automaticamente, mesmo que sejam informadas <br/>
        /// O Nome, Login e Senha do Usuário devem ter no mínimo 4 caracteres <br/>
        /// Não é permitido o cadastro de Login repetido
        /// </remarks>
        /// <param name="usuarioDto">
        /// Dados do novo Usuário
        /// </param>
        /// <returns>
        /// Retorna o Usuário criado
        /// </returns>
        [HttpPost]
        [Authorize]
        public async Task<ActionResult<UsuarioDto>> Insere(UsuarioDto usuarioDto)
        {
            try
            {
                Usuario usuario = UsuarioDto.ToEntity(usuarioDto);
                usuario.Id = 0;
                usuario.DataInclusao = DateTime.Now;
                usuario.DataAlteracao = DateTime.Now;
                Valida(usuario);
                await _dao.Insere(usuario);
                usuarioDto = UsuarioDto.ToDto(usuario);

                return CreatedAtAction(nameof(Insere), new { id = usuarioDto.Id }, usuarioDto);
            }
            catch (DbUpdateException dbUex)
            {
                string msg = dbUex.Message;
                if (dbUex.InnerException != null)
                {
                    msg += " - " + dbUex.InnerException.Message;
                }
                throw new ArgumentException("Erro ao tentar salvar no banco - " + msg);
            }
            catch (SqliteException sqlLex)
            {
                string msg = sqlLex.Message;
                if (sqlLex.InnerException != null)
                {
                    msg += " - " + sqlLex.InnerException.Message;
                }
                throw new ArgumentException("Problema de acesso ao banco de dados - " + msg);
            }
            catch (Exception ex)
            {
                string msg = ex.Message;
                if (ex.InnerException != null)
                {
                    msg += " - " + ex.InnerException.Message;
                }
                throw new ArgumentException("Erro ao tentar criar um novo usuário - " + msg);
            }
        }

        /// <summary>
        /// Rota DELETE: api/Usuarios/{id}
        /// Exclui o Usuário do id informado
        /// </summary>
        /// <remarks>
        /// Rota protegida. Deve ser inserido no Header a chave "Authorization" e o valor "Bearer token". o token é obtido na rota api/Login <br/>
        /// ATENÇÃO!!! Coloque a palavra "Bearer", um espaço e depois o token no valor do header. <br/>
        /// Se estiver usando o Swagger tem um botão no topo, a direita escrito "Authorize", clique nele e preencha com a palavra "Bearer", um espaço e depois o token
        /// </remarks>
        /// <param name="id">
        /// id do Usuário a ser excluído
        /// </param>
        /// <returns>
        /// Retorna NoContent
        /// </returns>
        [HttpDelete("{id}")]
        [Authorize]
        public async Task<IActionResult> Exclui(int id)
        {
            try
            {
                Usuario usuario = await _dao.Busca(id);
                if (usuario == null)
                {
                    return NotFound();
                }

                await _dao.Exclui(usuario);

                return NoContent();
            }
            catch (SqliteException sqlLex)
            {
                string msg = sqlLex.Message;
                if (sqlLex.InnerException != null)
                {
                    msg += " - " + sqlLex.InnerException.Message;
                }
                throw new ArgumentException("Problema de acesso ao banco de dados - " + msg);
            }
            catch (Exception ex)
            {
                string msg = ex.Message;
                if (ex.InnerException != null)
                {
                    msg += " - " + ex.InnerException.Message;
                }
                throw new ArgumentException("Erro ao tentar excluir o usuário de id = " + id + " - " + msg);
            }
        }

        private void Valida(Usuario usuario)
        {
            // int TamanhoMinimoNomeUsuario = int.Parse(AppData.Configuration["ConsistenciaDados:TamanhoMinimoNomeUsuario"]);
            int TamanhoMinimoNomeUsuario = 4; // <= ToDo Coloquei fixo aqui porque não tava conseguindo injetar nos testes
            if (string.IsNullOrEmpty(usuario.Nome) || usuario.Nome.Trim().Length < TamanhoMinimoNomeUsuario)
            {
                throw new ArgumentException("O nome do usuário é obrigatório, não pode ser em branco e deve ter mais de " + TamanhoMinimoNomeUsuario + " caracteres");
            }
            // int TamanhoMinimoLoginUsuario = int.Parse(AppData.Configuration["ConsistenciaDados:TamanhoMinimoLoginUsuario"]);
            int TamanhoMinimoLoginUsuario = 4; // <= ToDo Coloquei fixo aqui porque não tava conseguindo injetar nos testes
            if (string.IsNullOrEmpty(usuario.Login) || usuario.Login.Trim().Length < TamanhoMinimoLoginUsuario)
            {
                throw new ArgumentException("O login do usuário é obrigatório, não pode ser em branco e deve ter mais de " + TamanhoMinimoLoginUsuario + " caracteres");
            }
            // int TamanhoMinimoSenhaUsuario = int.Parse(AppData.Configuration["ConsistenciaDados:TamanhoMinimoSenhaUsuario"]);
            int TamanhoMinimoSenhaUsuario = 4; // <= ToDo Coloquei fixo aqui porque não tava conseguindo injetar nos testes
            if (string.IsNullOrEmpty(usuario.Senha) || usuario.Senha.Trim().Length < TamanhoMinimoSenhaUsuario)
            {
                throw new ArgumentException("A senha do usuário é obrigatória, não pode ser em branco e deve ter mais de " + TamanhoMinimoLoginUsuario + " caracteres");
            }
            Task<Usuario> usuarioDb = _dao.BuscaPorLogin(usuario.Login);
            if (usuarioDb != null && usuarioDb.Id != usuario.Id)
            {
                throw new ArgumentException("Já existe outro usuário com o Login '" + usuario.Login + "' cadastrado");
            }

        }

    }
}
