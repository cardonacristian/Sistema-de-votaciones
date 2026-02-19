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
    public class CandidatosController : ControllerBase
    {
        private readonly AppDbContext _context;

        public CandidatosController(AppDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public IActionResult ObtenerCandidatos(String nombre = "", int pageNumber = 1, int pageSize = 10)
        {
            var query = _context.Candidatos.AsQueryable();

            if (!string.IsNullOrEmpty(nombre))
                query = query.Where(v => v.Nombre.Contains(nombre));

            var total = query.Count();

            var candidatos = query
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToList();

            return Ok(new
            {
                totalCandidatos = total,
                pageNumber,
                pageSize,
                data = candidatos
            });
        }

        [HttpGet("{id}")]
        public IActionResult ObtenerCandidatosId(int id)
        {
            var candidatos = _context.Candidatos.Find(id);
            if (candidatos == null)
                return NotFound();
            return Ok(candidatos);
        }

        [HttpPost]
        public IActionResult CrearCandidatos([FromBody] Candidato candidato)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            if (_context.Votantes.Any(c => c.Nombre == candidato.Nombre))
                return BadRequest("Este candidato ya esta registrado como votante.");

            _context.Candidatos.Add(candidato);
            _context.SaveChanges();

            return CreatedAtAction(nameof(ObtenerCandidatos), new { id = candidato.Id }, candidato);
        }

        [HttpDelete("{id}")]
        public IActionResult EliminarCandidato(int id)
        {
            var candidato = _context.Candidatos.Find(id);
            if (candidato == null)
                return NotFound();
            _context.Remove(candidato);
            _context.SaveChanges();

            return NoContent();
        }
    }
}
