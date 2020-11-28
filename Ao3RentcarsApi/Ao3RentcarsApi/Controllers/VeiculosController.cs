using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Ao3RentcarsApi.Models;
using Ao3RentcarsApi.Models.Dto;
using Microsoft.Data.Sqlite;
using Ao3RentcarsApi.Dao;
using Ao3RentcarsApi.Util;
using Microsoft.AspNetCore.Authorization;

namespace Ao3RentcarsApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class VeiculosController : ControllerBase
    {
        private readonly VeiculoDao _dao;

        public VeiculosController(RentcarsContext context)
        {
            _dao = new VeiculoDao(context);
        }

        // GET: api/Veiculos
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

        // GET: api/Veiculos/5
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

        // PUT: api/Veiculos/5
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

        // POST: api/Veiculos
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

        // DELETE: api/Veiculos/5
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
            int MinimoAnoVeiculo = int.Parse(AppData.Configuration["ConsistenciaDados:MinimoAnoVeiculo"]);
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
