using GestranChecklist.Application.Dtos;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

[Route("api/[controller]")]
[ApiController]
public class ChecklistController : ControllerBase
{
    private readonly IChecklistService _checklistService;

    public ChecklistController(IChecklistService checklistService)
    {
        _checklistService = checklistService;
    }

    [HttpPost]
    [SwaggerOperation(Summary = "Método para criação de um novo Checklist, com ou sem os Itens do Checklist.")]
    [ProducesResponseType(200)]
    [ProducesResponseType(typeof(ResultViewModel), 400)]
    public async Task<IActionResult> CriarChecklist(ChecklistDto checklist)
    {
        var result = await _checklistService.CriarChecklist(checklist);

        if (!result.IsSuccess)
        {
            return BadRequest(result);
        }

        return Ok();
    }

    [HttpPost("{checklistId}/itens")]
    [SwaggerOperation(Summary = "Método para adicionar Itens a um Checklist já existente.")]
    [ProducesResponseType(200)]
    [ProducesResponseType(typeof(ResultViewModel), 400)]
    public async Task<IActionResult> AdicionarItemAoChecklist(int checklistId, [FromBody] ChecklistItemDto itemModel, [FromHeader] string executorId)
    {
        var resultado = await _checklistService.AdicionarItemAoChecklist(checklistId, itemModel, executorId);

        if (!resultado.IsSuccess)
        {
            return BadRequest(resultado.Message);
        }

        return Ok();
    }

    [HttpGet("{id}")]
    [SwaggerOperation(Summary = "Método para buscar um Checklist por Id.")]
    [ProducesResponseType(typeof(ResultViewModel<Checklist>), 200)]
    [ProducesResponseType(404)]
    public async Task<IActionResult> ObterChecklist(int id)
    {
        var result = await _checklistService.ObterChecklist(id);

        if (!result.IsSuccess)
        {
            return NotFound();
        }

        return Ok(result);
    }

    [HttpPost("{id}")]
    [SwaggerOperation(Summary = "Método para realizar a aprovação de um Checklist.")]
    [ProducesResponseType(201)]
    [ProducesResponseType(typeof(ResultViewModel), 400)]
    public async Task<IActionResult> AprovarChecklist(int id, string supervisorId)
    {
        var resultado = await _checklistService.AprovarChecklist(id, supervisorId);

        if (!resultado.IsSuccess)
        {
            return BadRequest(resultado.Message);
        }

        return NoContent();
    }

    [HttpGet]
    [SwaggerOperation(Summary = "Método que retorna todos os checklists existentes.")]
    [ProducesResponseType(typeof(ResultViewModel<Checklist>), 200)]
    public async Task<IActionResult> ObterTodosChecklists()
    {
        var checklists = await _checklistService.ListarChecklists();
        return Ok(checklists);
    }

    [HttpPut("{id}")]
    [SwaggerOperation(Summary = "Método que atualiza um checklist.")]
    [ProducesResponseType(204)]
    [ProducesResponseType(404)]
    public async Task<IActionResult> AtualizarChecklist(int id, [FromBody] ChecklistDto checklistDto)
    {
        var resultado = await _checklistService.AtualizarChecklist(id, checklistDto);

        if (!resultado.IsSuccess)
        {
            return NotFound(resultado.Message);
        }

        return NoContent();
    }
}
