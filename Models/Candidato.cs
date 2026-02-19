using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Sistema_de_votaciones.Models
{
    public class Candidato
    {
        public int Id { get; set; }

        [Required]
        public string Nombre { get; set; }

        public string Partido { get; set; }

        [DefaultValue(0)]
        public int votos { get; set; }

    }
}
