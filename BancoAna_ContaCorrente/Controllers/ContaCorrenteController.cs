using Application.DTOs;
using Application.Services;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ContaCorrenteController : ControllerBase
{
    private readonly CriarContaCorrenteService _service;

    public ContaCorrenteController(CriarContaCorrenteService service)
    {
        _service = service;
    }

    [HttpPost]
    public async Task<IActionResult> Post([FromBody] CriarContaCorrenteRequest request)
    {
        var (resposta, tipoErro, msgErro) = await _service.CriarContaAsync(request);

        if (tipoErro is not null)
            return BadRequest(new { tipo = tipoErro, mensagem = msgErro });

        return Ok(resposta);
    }
}
