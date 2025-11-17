using ATS.Backend.Models;
using ATS.Backend.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace ATS.Backend.Controllers;

[ApiController]
[Route("api/[controller]")]
public class CandidatosController : ControllerBase
{
    private readonly ICandidatoRepository _repo;
    private readonly ILogger<CandidatosController> _logger;

    public CandidatosController(ICandidatoRepository repo, ILogger<CandidatosController> logger)
    {
        _repo = repo;
        _logger = logger;
    }

    [HttpGet]
    public async Task<IActionResult> Get() =>
        Ok(await _repo.GetAllAsync());

    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(string id)
    {
        var candidato = await _repo.GetByIdAsync(id);
        return candidato == null ? NotFound() : Ok(candidato);
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] Candidato candidato)
    {
        var criado = await _repo.CreateAsync(candidato);
        return Ok(criado);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Update(string id, [FromBody] Candidato candidato)
    {
        await _repo.UpdateAsync(id, candidato);
        return NoContent();
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(string id)
    {
        await _repo.DeleteAsync(id);
        return NoContent();
    }
}
