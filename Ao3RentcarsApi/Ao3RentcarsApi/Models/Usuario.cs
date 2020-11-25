using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Ao3RentcarsApi.Models
{
    public class Usuario
    {
        public int Id { get; set; }

        public string Nome { get; set; }

        public string Login { get; set; }

        public string Senha { get; set; }
    }
}
