using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Ao3RentcarsApi.Models;
using Ao3RentcarsApi.Models.Dto;
using Microsoft.Data.Sqlite;
using Ao3RentcarsApi.Dao;
using Ao3RentcarsApi.Helpers;
using Microsoft.AspNetCore.Authorization;

namespace Ao3RentcarsApi.Controllers
{
    /// <summary>
    /// Faz o CRUD dos Clientes
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    public class ClientesController : ControllerBase
    {
        private readonly ClienteDao _dao;

        /// <summary>
        /// Construtor da classe
        /// </summary>
        /// <remarks>
        /// Recebe o contexto e instancia a classe dao passando o contexto
        /// </remarks>
        /// <param name="context">
        /// RentcarsContext
        /// </param>
        public ClientesController(RentcarsContext context)
        {
            _dao = new ClienteDao(context);
        }

        /// <summary>
        /// Rota GET: api/Clientes
        /// </summary>
        /// <remarks>
        /// Rota protegida. Deve ser inserido no Header a chave "Authorization" e o valor "Bearer token". O token é obtido na rota api/Login
        /// </remarks>
        /// <returns>
        /// Retorna uma lista de todos os Clientes Cadastrados
        /// </returns>
        [HttpGet]
        [Authorize]
        public async Task<ActionResult<IEnumerable<ClienteDto>>> ListaTodos()
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

        /// <summary>
        /// Rota GET: api/Clientes/{id}
        /// </summary>
        /// <remarks>
        /// Rota protegida. Deve ser inserido no Header a chave "Authorization" e o valor "Bearer token". O token é obtido na rota api/Login
        /// </remarks>
        /// <param name="id">
        /// Id do Cliente
        /// </param>
        /// <returns>
        /// Retorna o Cliente do id informado
        /// </returns>
        [HttpGet("{id}")]
        [Authorize]
        public async Task<ActionResult<ClienteDto>> Busca(int id)
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

        /// <summary>
        /// Rota GET: api/Clientes/cpf={cpf}
        /// </summary>
        /// <remarks>
        /// Rota protegida. Deve ser inserido no Header a chave "Authorization" e o valor "Bearer token". O token é obtido na rota api/Login
        /// </remarks>
        /// <param name="cpf">
        /// Cpf do Cliente
        /// </param>
        /// <returns>
        /// Retorna o Cliente do cpf informado
        /// </returns>
        [HttpGet("cpf={cpf}")]
        [Authorize]
        public async Task<ActionResult<ClienteDto>> Busca(string cpf)
        {
            try
            {
                Cliente cliente = _dao.BuscaPorCpf(cpf);

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
                throw new ArgumentException("Erro ao tentar buscar o cliente de cpf = " + cpf + " - " + msg);
            }
        }

        /// <summary>
        /// Rota PUT: api/Clientes/{id}
        /// Altera os dados do Cliente do id informado
        /// </summary>
        /// <remarks>
        /// Rota protegida. Deve ser inserido no Header a chave "Authorization" e o valor "Bearer token". o token é obtido na rota api/Login <br/>
        /// As propriedades do Cliente não informadas serão ignoradas <br/>
        /// O Id e DataInclusao do Json sempre são ignorados <br/>
        /// A DataAlteracao é preenchida automaticamente, mesmo que seja informada <br/>
        /// Se informado, o Nome do Cliente deve ter no mínimo 4 caracteres <br/>
        /// Todos os caracteres não numéricos do Cpf são removidos e é feita a validação do mesmo pelo dígito verificador <br/>
        /// Não é permitido o cadastro de Cpf repetido
        /// </remarks>
        /// <param name="id">
        /// id do Cliente a ser alterado
        /// </param>
        /// <param name="clienteDto">
        /// Dados do Cliente que devem ser alterados
        /// </param>
        /// <returns>
        /// Retorna o Cliente com os dados alterados
        /// </returns>
        [HttpPut("{id}")]
        [Authorize]
        public async Task<IActionResult> Altera(int id, ClienteDto clienteDto)
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
                Valida(cliente);

                await _dao.Altera(cliente);

                clienteDto = ClienteDto.ToDto(cliente);

                return CreatedAtAction(nameof(Altera), new { id = clienteDto.Id }, clienteDto);
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

        /// <summary>
        /// Rota POST: api/Clientes
        /// Insere um novo Cliente
        /// </summary>
        /// <remarks>
        /// Rota protegida. Deve ser inserido no Header a chave "Authorization" e o valor "Bearer token". o token é obtido na rota api/Login <br/>
        /// O Id, DataInclusao e DataAlteracao são preenchidos automaticamente, mesmo que sejam informadas <br/>
        /// O Nome do Cliente deve ter no mínimo 4 caracteres <br/>
        /// O Nome e o Cpf é obrigatório <br/>
        /// Todos os caracteres não numéricos do Cpf são removidos e é feita a validação do mesmo pelo dígito verificador <br/>
        /// Não é permitido o cadastro de Cpf repetido
        /// </remarks>
        /// <param name="clienteDto">
        /// Dados do novo Cliente
        /// </param>
        /// <returns>
        /// Retorna o Cliente criado
        /// </returns>
        [HttpPost]
        [Authorize]
        public async Task<ActionResult<ClienteDto>> Insere(ClienteDto clienteDto)
        {
            try
            {
                Cliente cliente = ClienteDto.ToEntity(clienteDto);
                cliente.Id = 0;
                cliente.DataInclusao = DateTime.Now;
                cliente.DataAlteracao = DateTime.Now;
                Valida(cliente);
                await _dao.Insere(cliente);
                clienteDto = ClienteDto.ToDto(cliente);

                return CreatedAtAction(nameof(Insere), new { id = clienteDto.Id }, clienteDto);
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

        /// <summary>
        /// Rota DELETE: api/Clientes/{id}
        /// Exclui o Cliente do id informado
        /// </summary>
        /// <param name="id">
        /// id do Cliente a ser excluído
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
                Cliente cliente = await _dao.Busca(id);
                if (cliente == null)
                {
                    return NotFound();
                }

                await _dao.Exclui(cliente);

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

        private void Valida(Cliente cliente)
        {
            //int TamanhoMinimoNomeCliente = int.Parse(AppData.Configuration["ConsistenciaDados:TamanhoMinimoNomeCliente"]);
            int TamanhoMinimoNomeCliente = 4; // <= ToDo Coloquei fixo aqui porque não tava conseguindo injetar nos testes
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
