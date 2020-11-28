using System;
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
    public class ClientesController : ControllerBase
    {
        private readonly ClienteDao _dao;

        public ClientesController(RentcarsContext context)
        {
            _dao = new ClienteDao(context);
        }

        // GET: api/Clientes
        [HttpGet]
        public async Task<ActionResult<IEnumerable<ClienteDto>>> GetClientes()
        {
            try
            {
                return ClienteDto.ToDtoList(await _dao.ListaTodos());
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
                throw new ArgumentException("Erro ao tentar buscar a lista de clientes - " + msg);
            }

        }

        // GET: api/Clientes/5
        [HttpGet("{id}")]
        public async Task<ActionResult<ClienteDto>> GetCliente(int id)
        {
            try
            {
                Cliente cliente = await _dao.Busca(id);

                if (cliente == null)
                {
                    return NotFound();
                }

                ClienteDto clienteDto = ClienteDto.ToDto(cliente);

                return clienteDto;
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
                throw new ArgumentException("Erro ao tentar buscar o cliente de id = " + id + " - " + msg);
            }
        }

        // PUT: api/Clientes/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutCliente(int id, ClienteDto clienteDto)
        {
            try
            {
                Cliente cliente = await _dao.Busca(id);

                if (cliente == null)
                {
                    return NotFound();
                }

                cliente = ClienteDto.PutEntity(cliente, clienteDto);
                cliente.DataAlteracao = DateTime.Now;
                ValidaCliente(cliente);

                await _dao.Altera(cliente);

                clienteDto = ClienteDto.ToDto(cliente);

                return CreatedAtAction(nameof(PutCliente), new { id = clienteDto.Id }, clienteDto);
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
                throw new ArgumentException("Erro ao tentar alterar o cliente de id = " + id + " - " + msg);
            }
        }

        // POST: api/Clientes
        [HttpPost]
        public async Task<ActionResult<ClienteDto>> PostCliente(ClienteDto clienteDto)
        {
            try
            {
                Cliente cliente = ClienteDto.ToEntity(clienteDto);
                cliente.Id = 0;
                cliente.DataInclusao = DateTime.Now;
                cliente.DataAlteracao = DateTime.Now;
                ValidaCliente(cliente);
                await _dao.Insere(cliente);
                clienteDto = ClienteDto.ToDto(cliente);

                return CreatedAtAction(nameof(PostCliente), new { id = clienteDto.Id }, clienteDto);
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
                throw new ArgumentException("Erro ao tentar criar um novo cliente - " + msg);
            }
        }

        // DELETE: api/Clientes/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCliente(int id)
        {
            try
            {
                Cliente cliente = await _dao.Busca(id);
                if (cliente == null)
                {
                    return NotFound();
                }

                await _dao.Apaga(cliente);

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
                throw new ArgumentException("Erro ao tentar excluir o cliente de id = " + id + " - " + msg);
            }
        }

        private void ValidaCliente(Cliente cliente)
        {
            int TamanhoMinimoNomeCliente = int.Parse(AppData.Configuration["ConsistenciaDados:TamanhoMinimoNomeCliente"]);
            if (string.IsNullOrEmpty(cliente.Nome) || cliente.Nome.Trim().Length < TamanhoMinimoNomeCliente)
            {
                throw new ArgumentException("O nome do cliente é obrigatório, não pode ser em branco e deve ter mais de " + TamanhoMinimoNomeCliente + " caracteres");
            }
            if (!Validador.CpfValido(cliente.Cpf))
            {
                throw new ArgumentException("Cpf inválido");
            }
            if (cliente.Id == 0)
            {
                // Cadastrando um novo cliente, verificar se o Cpf dele já está cadastrado
                string nome = _dao.NomeDoCpf(cliente.Cpf);
                if (!string.IsNullOrEmpty(nome))
                {
                    throw new ArgumentException("O Cpf informado já está cadastrado para o cliente " + nome);
                }
            }
            else
            {
                // Validado conflito entre Cpfs
                Cliente clienteDb = _dao.BuscaPorCpf(cliente.Cpf);
                if (cliente.Id != clienteDb.Id)
                {
                    throw new ArgumentException("O Cpf informado já está cadastrado para o cliente " + clienteDb.Nome);
                }
            }
        }
    }
}
