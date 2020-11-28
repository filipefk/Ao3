﻿using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Ao3RentcarsApi.Models;
using Ao3RentcarsApi.Models.Dto;
using Microsoft.Data.Sqlite;
using Ao3RentcarsApi.Dao;
using Ao3RentcarsApi.Util;

namespace Ao3RentcarsApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsuariosController : ControllerBase
    {
        private readonly UsuarioDao _dao;

        public UsuariosController(RentcarsContext context)
        {
            _dao = new UsuarioDao(context);
        }

        // GET: api/Usuarios
        [HttpGet]
        public async Task<ActionResult<IEnumerable<UsuarioDto>>> GetUsuarios()
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

        // GET: api/Usuarios/5
        [HttpGet("{id}")]
        public async Task<ActionResult<UsuarioDto>> GetUsuario(int id)
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

        // PUT: api/Usuarios/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutUsuario(int id, UsuarioDto usuarioDto)
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
                ValidaUsuario(usuario);

                await _dao.Altera(usuario);

                usuarioDto = UsuarioDto.ToDto(usuario);

                return CreatedAtAction(nameof(PutUsuario), new { id = usuarioDto.Id }, usuarioDto);
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

        // POST: api/Usuarios
        [HttpPost]
        public async Task<ActionResult<UsuarioDto>> PostUsuario(UsuarioDto usuarioDto)
        {
            try
            {
                Usuario usuario = UsuarioDto.ToEntity(usuarioDto);
                usuario.Id = 0;
                usuario.DataInclusao = DateTime.Now;
                usuario.DataAlteracao = DateTime.Now;
                ValidaUsuario(usuario);
                await _dao.Insere(usuario);
                usuarioDto = UsuarioDto.ToDto(usuario);

                return CreatedAtAction(nameof(PostUsuario), new { id = usuarioDto.Id }, usuarioDto);
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

        // DELETE: api/Usuarios/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteUsuario(int id)
        {
            try
            {
                Usuario usuario = await _dao.Busca(id);
                if (usuario == null)
                {
                    return NotFound();
                }

                await _dao.Apaga(usuario);

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

        private void ValidaUsuario(Usuario usuario)
        {
            int TamanhoMinimoNomeUsuario = int.Parse(AppData.Configuration["ConsistenciaDados:TamanhoMinimoNomeUsuario"]);
            if (string.IsNullOrEmpty(usuario.Nome) || usuario.Nome.Trim().Length < TamanhoMinimoNomeUsuario)
            {
                throw new ArgumentException("O nome do usuário é obrigatório, não pode ser em branco e deve ter mais de " + TamanhoMinimoNomeUsuario + " caracteres");
            }
            int TamanhoMinimoLoginUsuario = int.Parse(AppData.Configuration["ConsistenciaDados:TamanhoMinimoLoginUsuario"]);
            if (string.IsNullOrEmpty(usuario.Login) || usuario.Login.Trim().Length < TamanhoMinimoLoginUsuario)
            {
                throw new ArgumentException("O login do usuário é obrigatório, não pode ser em branco e deve ter mais de " + TamanhoMinimoLoginUsuario + " caracteres");
            }
            int TamanhoMinimoSenhaUsuario = int.Parse(AppData.Configuration["ConsistenciaDados:TamanhoMinimoSenhaUsuario"]);
            if (string.IsNullOrEmpty(usuario.Senha) || usuario.Senha.Trim().Length < TamanhoMinimoSenhaUsuario)
            {
                throw new ArgumentException("A senha do usuário é obrigatória, não pode ser em branco e deve ter mais de " + TamanhoMinimoLoginUsuario + " caracteres");
            }
        }

    }
}
