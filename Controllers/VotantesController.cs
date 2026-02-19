using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Sistema_de_votaciones.Data;
using Sistema_de_votaciones.Models;

namespace Sistema_de_votaciones.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class VotantesController : ControllerBase
    {
        private readonly AppDbContext _context;

        public VotantesController(AppDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public IActionResult ObtenerVotantes(string nombre = "", int pageNumber = 1, int pageSize = 10)
        {
            var query = _context.Votantes.AsQueryable();

            if (!string.IsNullOrEmpty(nombre))
                query = query.Where(v => v.Nombre.Contains(nombre));

            var total = query.Count();

            var votantes = query
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToList();

            return Ok(new
            {
                totalVotantes = total,
                pageNumber,
                pageSize,
                data = votantes
            });
        }

        [HttpGet("{id}")]
        public IActionResult ObtenerVotantesId(int id)
        {
            var votantes = _context.Votantes.Find(id);
            if (votantes == null)
                return NotFound();
            return Ok(votantes);
        }

        [HttpPost]
        public IActionResult CrearVotantes([FromBody] Votante votante)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            if (_context.Votantes.Any(v => v.Email == votante.Email))
                return BadRequest("Este correo ya esta registrado.");
            if (_context.Candidatos.Any(c => c.Nombre == votante.Nombre))
                return BadRequest("Este votante ya esta registrado como candidato.");

            _context.Votantes.Add(votante);
            _context.SaveChanges();

            return CreatedAtAction(nameof(ObtenerVotantes), new {id = votante.Id}, votante);
        }

        [HttpDelete("{id}")]
        public IActionResult EliminarVotante(int id)
        {
            var votantes = _context.Votantes.Find(id);
            if (votantes == null)
                return NotFound();
            _context.Remove(votantes);
            _context.SaveChanges();

            return NoContent();
        }
    }
}
