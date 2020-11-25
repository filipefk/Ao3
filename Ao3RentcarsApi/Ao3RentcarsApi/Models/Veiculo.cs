using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Ao3RentcarsApi.Models
{
    public class Veiculo
    {
        public int Id { get; set; }

        public string Modelo { get; set; }

        public string Marca { get; set; }

        public string Placa { get; set; }

        public int AnoModelo { get; set; }

        public int AnoFabricacao { get; set; }
    }
}
