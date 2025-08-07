using Application.DTOs;
using Application.Services;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ContaCorrenteController : ControllerBase
{
    private readonly CriarContaService _service;

    public ContaCorrenteController(CriarContaService service)
    {
        _service = service;
    }

    [HttpPost]
    public async Task<IActionResult> Post([FromBody] CriarContaRequest request)
    {
        var (resposta, tipoErro, msgErro) = await _service.CriarAsync(request);

        if (tipoErro is not null)
            return BadRequest(new { tipo = tipoErro, mensagem = msgErro });

        return Ok(resposta);
    }
}
