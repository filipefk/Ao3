using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Ao3RentcarsApi.Models
{
    [Table("Veiculo")]
    public class Veiculo
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        [Required]
        public string Modelo { get; set; }
        [Required]
        public string Marca { get; set; }
        [Required]
        [MinLength(7)]
        [MaxLength(7)]
        public string Placa { get; set; }
        [Required]
        public int AnoModelo { get; set; }
        [Required]
        public int AnoFabricacao { get; set; }
    }
}
