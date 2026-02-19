using System.Text.Json.Serialization;

namespace Sistema_de_votaciones.Models
{
    public class Voto
    {
        public int Id { get; set; }
        public int VotanteId { get; set; }
        public int CandidatoId { get; set; }

        [JsonIgnore]
        public Votante? Votante { get; set; }
        [JsonIgnore]
        public Candidato? Candidato { get; set; }

    }
}
