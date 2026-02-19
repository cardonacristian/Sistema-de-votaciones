using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Sistema_de_votaciones.Models
{
    public class Votante
    {
        public int Id { get; set; }

        [Required]
        public string Nombre {  get; set; }

        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [DefaultValue(false)]
        public bool SiVoto { get; set; }
    }
}
