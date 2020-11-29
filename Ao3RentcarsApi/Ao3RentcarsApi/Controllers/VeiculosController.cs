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
    /// Faz o CRUD dos Veículos
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    public class VeiculosController : ControllerBase
    {
        private readonly VeiculoDao _dao;

        /// <summary>
        /// Construtor da classe
        /// </summary>
        /// <remarks>
        /// Recebe o contexto e instancia a classe dao passando o contexto
        /// </remarks>
        /// <param name="context">
        /// RentcarsContext
        /// </param>
        public VeiculosController(RentcarsContext context)
        {
            _dao = new VeiculoDao(context);
        }

        /// <summary>
        /// Rota GET: api/Veiculos
        /// </summary>
        /// <remarks>
        /// Rota desprotegida. Não é necessário autenticação para consulta
        /// </remarks>
        /// <returns>
        /// Retorna uma lista de todos os Veículos cadastrados
        /// </returns>
        [HttpGet]
        [AllowAnonymous]
        public async Task<ActionResult<IEnumerable<VeiculoDto>>> ListaTodos()
        {
            try
            {
                return VeiculoDto.ToDtoList(await _dao.ListaTodos());
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
                throw new ArgumentException("Erro ao tentar buscar a lista de veículos - " + msg);
            }
        }

        /// <summary>
        /// Rota GET: api/Veiculos/Disponiveis
        /// </summary>
        /// <remarks>
        /// Rota desprotegida. Não é necessário autenticação para consulta
        /// </remarks>
        /// <returns>
        /// Retorna uma lista dos Veículos disponíveis para locação
        /// </returns>
        [HttpGet]
        [Route("api/[controller]/Disponiveis")]
        [AllowAnonymous]
        public async Task<ActionResult<IEnumerable<VeiculoDto>>> ListaDisponiveis()
        {
            try
            {
                return VeiculoDto.ToDtoList(await _dao.ListaDisponiveis());
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
                throw new ArgumentException("Erro ao tentar buscar a lista de veículos - " + msg);
            }
        }

        /// <summary>
        /// Rota GET: api/Veiculos/{id}
        /// </summary>
        /// <remarks>
        /// Rota protegida. Deve ser inserido no Header a chave "Authorization" e o valor "Bearer token". O token é obtido na rota api/Login <br/>
        /// Se estiver usando o Swagger tem um botão no topo, a direita escrito "Authorize", clique nele e preencha com a palavra "Bearer", um espaço e depois o token
        /// </remarks>
        /// <param name="id">
        /// Id do Veículo
        /// </param>
        /// <returns>
        /// Retorna o Veículo do id informado
        /// </returns>
        [HttpGet("{id}")]
        [Authorize]
        public async Task<ActionResult<VeiculoDto>> Busca(int id)
        {
            try
            {
                Veiculo veiculo = await _dao.Busca(id);

                if (veiculo == null)
                {
                    return NotFound();
                }

                VeiculoDto veiculoDto = VeiculoDto.ToDto(veiculo);

                return veiculoDto;
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
                throw new ArgumentException("Erro ao tentar buscar o veículo de id = " + id + " - " + msg);
            }

        }

        /// <summary>
        /// Rota PUT: api/Veiculos/{id}
        /// Altera os dados do Veículo do id informado
        /// </summary>
        /// <remarks>
        /// Rota protegida. Deve ser inserido no Header a chave "Authorization" e o valor "Bearer token". O token é obtido na rota api/Login <br/>
        /// Se estiver usando o Swagger tem um botão no topo, a direita escrito "Authorize", clique nele e preencha com a palavra "Bearer", um espaço e depois o token
        /// As propriedades do Veículo não informadas serão ignoradas <br/>
        /// O Id e DataInclusao do Json sempre são ignorados <br/>
        /// A DataAlteracao é preenchida automaticamente, mesmo que seja informada <br/>
        /// O Ano de Fabricação e o Ano do Modelo devem ser no mínimo 1990 e no máximo 1 ano a mais que o ano atual
        /// </remarks>
        /// <param name="id">
        /// id do Veículo a ser alterado
        /// </param>
        /// <param name="veiculoDto">
        /// Dados do Veículo que devem ser alterados
        /// </param>
        /// <returns>
        /// Retorna o Veículo com os dados alterados
        /// </returns>
        [HttpPut("{id}")]
        [Authorize]
        public async Task<IActionResult> Altera(int id, VeiculoDto veiculoDto)
        {
            try
            {
                Veiculo veiculo = await _dao.Busca(id);

                if (veiculo == null)
                {
                    return NotFound();
                }

                veiculo = VeiculoDto.PutEntity(veiculo, veiculoDto);
                veiculo.DataAlteracao = DateTime.Now;
                Valida(veiculo);

                await _dao.Altera(veiculo);

                veiculoDto = VeiculoDto.ToDto(veiculo);

                return CreatedAtAction(nameof(Altera), new { id = veiculoDto.Id }, veiculoDto);
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
                throw new ArgumentException("Erro ao tentar alterar o veículo de id = " + id + " - " + msg);
            }
        }

        /// <summary>
        /// Rota POST: api/Veiculos
        /// Insere um novo Veículo
        /// </summary>
        /// <remarks>
        /// Rota protegida. Deve ser inserido no Header a chave "Authorization" e o valor "Bearer token". O token é obtido na rota api/Login <br/>
        /// Se estiver usando o Swagger tem um botão no topo, a direita escrito "Authorize", clique nele e preencha com a palavra "Bearer", um espaço e depois o token
        /// O Id, DataInclusao e DataAlteracao são preenchidos automaticamente, mesmo que sejam informadas <br/>
        /// O Ano de Fabricação e o Ano do Modelo devem ser no mínimo 1990 e no máximo 1 ano a mais que o ano atual <br/>
        /// A Marca, Modelo e Placa são obrigatórios <br/>
        /// Não é permitido o cadastro de Placa repetida
        /// </remarks>
        /// <param name="veiculoDto">
        /// Dados do novo Veículo
        /// </param>
        /// <returns>
        /// Retorna o Veículo criado
        /// </returns>
        [HttpPost]
        [Authorize]
        public async Task<ActionResult<VeiculoDto>> Insere(VeiculoDto veiculoDto)
        {
            try
            {
                Veiculo veiculo = VeiculoDto.ToEntity(veiculoDto);
                veiculo.Id = 0;
                veiculo.DataInclusao = DateTime.Now;
                veiculo.DataAlteracao = DateTime.Now;
                Valida(veiculo);
                await _dao.Insere(veiculo);
                veiculoDto = VeiculoDto.ToDto(veiculo);

                return CreatedAtAction(nameof(Insere), new { id = veiculoDto.Id }, veiculoDto);
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
                throw new ArgumentException("Erro ao tentar incluir um novo veículo - " + msg);
            }
        }

        /// <summary>
        /// Rota DELETE: api/Veiculos/{id}
        /// Exclui o Veículo do id informado
        /// </summary>
        /// <remarks>
        /// Rota protegida. Deve ser inserido no Header a chave "Authorization" e o valor "Bearer token". o token é obtido na rota api/Login <br/>
        /// Se estiver usando o Swagger tem um botão no topo, a direita escrito "Authorize", clique nele e preencha com a palavra "Bearer", um espaço e depois o token
        /// </remarks>
        /// <param name="id">
        /// id do Veículo a ser excluído
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
                Veiculo veiculo = await _dao.Busca(id);
                if (veiculo == null)
                {
                    return NotFound();
                }

                await _dao.Exclui(veiculo);

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
                throw new ArgumentException("Erro ao tentar excluir o veículo de id = " + id + " - " + msg);
            }
        }

        private void Valida(Veiculo veiculo)
        {
            //int MinimoAnoVeiculo = int.Parse(AppData.Configuration["ConsistenciaDados:MinimoAnoVeiculo"]);
            int MinimoAnoVeiculo = 1990;  // <= ToDo Coloquei fixo aqui porque não tava conseguindo injetar nos testes
            int MaximoAnoVeiculo = DateTime.Now.Year + 1;
            if (veiculo.AnoFabricacao < MinimoAnoVeiculo)
            {
                throw new ArgumentException("O ano de fabricação do veículo deve ser no mínimo " + MinimoAnoVeiculo);
            }
            if (veiculo.AnoFabricacao > MaximoAnoVeiculo)
            {
                throw new ArgumentException("O ano de fabricação do veículo deve ser no máximo " + MaximoAnoVeiculo);
            }
            if (veiculo.AnoModelo < MinimoAnoVeiculo)
            {
                throw new ArgumentException("O ano de modelo do veículo deve ser no mínimo " + MinimoAnoVeiculo);
            }
            if (veiculo.AnoModelo > MaximoAnoVeiculo)
            {
                throw new ArgumentException("O ano de modelo do veículo deve ser no máximo " + MaximoAnoVeiculo);
            }
            if (string.IsNullOrEmpty(veiculo.Marca))
            {
                throw new ArgumentException("A marca do veículo é obrigatória");
            }
            if (string.IsNullOrEmpty(veiculo.Modelo))
            {
                throw new ArgumentException("O modelo do veículo é obrigatório");
            }
            if (string.IsNullOrEmpty(veiculo.Placa))
            {
                throw new ArgumentException("A placa do veículo é obrigatória");
            }
            if (_dao.PlacaJaCadastrada(veiculo))
            {
                throw new ArgumentException("A placa " + veiculo.Placa + " já está cadastrada para outro veículo");
            }
        }

        
    }
}
