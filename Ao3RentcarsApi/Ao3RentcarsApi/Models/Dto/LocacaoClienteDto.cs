using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Ao3RentcarsApi.Models.Dto
{
    public class LocacaoClienteDto
    {
        public int IdUsuario { get; set; }

        public int IdVeiculo { get; set; }

        public string Nome { get; set; }

        public string Cpf { get; set; }

        public DateTime? DataInicio { get; set; }

        public DateTime? DataFimPrevisto { get; set; }
    }
}
