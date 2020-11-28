using System;
using System.Collections.Generic;
using System.Linq;

namespace Ao3RentcarsApi.Models.Dto
{
    public class LocacaoDto
    {
		public int Id { get; set; }

		public DateTime DataInclusao { get; set; }

		public DateTime DataAlteracao { get; set; }

		public DateTime? DataInicio { get; set; }

        public DateTime? DataFimPrevisto { get; set; }

        public DateTime? DataFim { get; set; }

		public int IdUsuario { get; set; }

		public int IdVeiculo { get; set; }

        public int IdCliente { get; set; }

        /// <summary>
        /// Copia os dados de uma Locacao para uma nova LocacaoDto
        /// </summary>
        /// <param name="locacao"></param>
        /// <returns>LocacaoDto</returns>
        public static LocacaoDto ToDto(Locacao locacao)
        {
            LocacaoDto locacaoDto = new LocacaoDto()
            {
                Id = locacao.Id,
                DataInclusao = locacao.DataInclusao,
                DataAlteracao = locacao.DataAlteracao,
                DataInicio = locacao.DataInicio,
                DataFimPrevisto = locacao.DataFimPrevisto,
                DataFim = locacao.DataFim,
                IdUsuario = locacao.IdUsuario,
                IdVeiculo = locacao.IdVeiculo,
                IdCliente = locacao.IdCliente
            };
            return locacaoDto;
        }


        /// <summary>
        /// Copia os dados de um LocacaoDto para um novo Locacao
        /// </summary>
        /// <param name="locacaoDto"></param>
        /// <returns>Locacao</returns>
        public static Locacao ToEntity(LocacaoDto locacaoDto)
        {
            Locacao locacao = new Locacao()
            {
                Id = locacaoDto.Id,
                DataInclusao = locacaoDto.DataInclusao,
                DataAlteracao = locacaoDto.DataAlteracao,
                DataInicio = (DateTime)locacaoDto.DataInicio,
                DataFimPrevisto = (DateTime)locacaoDto.DataFimPrevisto,
                DataFim = locacaoDto.DataFim,
                IdUsuario = locacaoDto.IdUsuario,
                IdVeiculo = locacaoDto.IdVeiculo,
                IdCliente = locacaoDto.IdCliente
            };
            return locacao;
        }


        /// <summary>
        /// Copia só os dados para Update (Put) do locacaoDto para o locacao 
        /// </summary>
        /// <param name="locacao"></param>
        /// <param name="locacaoDto"></param>
        /// <returns>Locacao</returns>
        public static Locacao PutEntity(Locacao locacao, LocacaoDto locacaoDto)
        {
            if (locacaoDto.DataFim != null && locacaoDto.DataFim > DateTime.MinValue)
            {
                locacao.DataFim = locacaoDto.DataFim;
            }
            if (locacaoDto.DataFimPrevisto != null && locacaoDto.DataFimPrevisto > DateTime.MinValue)
            {
                locacao.DataFimPrevisto = (DateTime)locacaoDto.DataFimPrevisto;
            }
            if (locacaoDto.DataFim == null && locacao.DataFim != null)
            {
                locacao.DataFim = locacaoDto.DataFim;
            }
            if (locacaoDto.DataInicio != null && locacaoDto.DataInicio > DateTime.MinValue)
            {
                locacao.DataInicio = (DateTime)locacaoDto.DataInicio;
            }
            locacao.DataAlteracao = DateTime.Now;
            if (locacaoDto.IdUsuario > 0)
            {
                locacao.IdUsuario = locacaoDto.IdUsuario;
            }
            if (locacaoDto.IdVeiculo > 0)
            {
                locacao.IdVeiculo = locacaoDto.IdVeiculo;
            }
            if (locacaoDto.IdCliente > 0)
            {
                locacao.IdCliente = locacaoDto.IdCliente;
            }
            return locacao;
        }

        /// <summary>
        /// Converte uma List de Locacao para uma List de LocacaoDto
        /// </summary>
        /// <param name="locacoes"></param>
        /// <returns>List de LocacaoDto</returns>
        public static List<LocacaoDto> ToDtoList(List<Locacao> locacoes)
        {
            IEnumerable<LocacaoDto> locacoesDto = from l in locacoes
                                                  select new LocacaoDto()
                                                  {
                                                      Id = l.Id,
                                                      DataInclusao = l.DataInclusao,
                                                      DataAlteracao = l.DataAlteracao,
                                                      DataInicio = l.DataInicio,
                                                      DataFimPrevisto = l.DataFimPrevisto,
                                                      DataFim = l.DataFim,
                                                      IdUsuario = l.IdUsuario,
                                                      IdVeiculo = l.IdVeiculo,
                                                      IdCliente = l.IdCliente
                                                  };
            return locacoesDto.ToList();
        }
    }
}
