using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Sistema_de_votaciones.Data;
using Sistema_de_votaciones.Models;

namespace Sistema_de_votaciones.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class VotosController : ControllerBase
    {
        private readonly AppDbContext _context;

        public VotosController(AppDbContext context)
        {
            _context = context;
        }

        [HttpPost]
        public IActionResult EmitirVotos([FromBody] Voto voto)
        {
            var votante = _context.Votantes.Find(voto.VotanteId);
            if (votante == null)
                return NotFound("Votante no encontrando");
            if (votante.SiVoto)
                return BadRequest("El votante ya emitio su voto");

            var candidato = _context.Candidatos.Find(voto.CandidatoId);
            if (candidato == null)
                return NotFound("Candidato no encontrado");

            votante.SiVoto = true;
            candidato.votos += 1;

            var nuevoVoto = new Voto
            {
                VotanteId = voto.VotanteId,
                CandidatoId = voto.CandidatoId,
            };

            _context.Votos.Add(nuevoVoto);
            _context.SaveChanges();

            return Ok(new
            {
                nuevoVoto.Id,
                Votante = votante,
                Candidato = candidato
            });
        }

        [HttpGet]
        public IActionResult ObtenerVotos()
        {
            var votos = _context.Votos.Include(v => v.Votante)
                .Include(v => v.Candidato)
                .ToList();
            return Ok(votos);
        }

        [HttpGet("Estadisticas")]
        public IActionResult Estadisticas()
        {
            var totalVotantes = _context.Votantes.Count();
            var votantesQueVotaron = _context.Votantes.Count(v => v.SiVoto == true);

            var stats = _context.Candidatos
                .Select(c => new
                {
                    c.Nombre,
                    c.Partido,
                    TotalVotos = c.votos,
                    Porcentaje = votantesQueVotaron == 0
                    ? "0%" : $"{((double)c.votos / votantesQueVotaron * 100):0.##}%"
                }).ToList();

            return Ok(new
            {
                totalVotantes = totalVotantes,
                votantesQueVotaron = votantesQueVotaron,
                Candidatos = stats
            });
        }
    }
}
