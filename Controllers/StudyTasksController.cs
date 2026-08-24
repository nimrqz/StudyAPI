using Microsoft.AspNetCore.Mvc;
using StudyAPI.DTOs;
using StudyAPI.Services;

namespace StudyAPI.Controllers;

[ApiController]
[Route("api/[controller]")]
public class StudyTasksController : ControllerBase
{
    private readonly IStudyTaskService _service;

    public StudyTasksController(IStudyTaskService service)
    {
        _service = service;
    }

    [HttpGet]
    [ProducesResponseType(typeof(PaginatedResponseDto<StudyTaskResponseDto>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetAll([FromQuery] StudyTaskQueryDto query)
    {
        var result = await _service.GetFilteredAsync(query);
        return Ok(result);
    }

    [HttpGet("{id:int}")]
    [ProducesResponseType(typeof(StudyTaskResponseDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetById(int id)
    {
        var task = await _service.GetByIdAsync(id);
        if (task == null)
            return NotFound(new { message = $"Tarefa com ID {id} não encontrada." });

        return Ok(task);
    }

    [HttpPost]
    [ProducesResponseType(typeof(StudyTaskResponseDto), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Create([FromBody] CreateStudyTaskDto dto)
    {
        try
        {
            var task = await _service.CreateAsync(dto);
            return CreatedAtAction(nameof(GetById), new { id = task.Id }, task);
        }
        catch (KeyNotFoundException ex)
        {
            return NotFound(new { message = ex.Message });
        }
    }

    [HttpPut("{id:int}")]
    [ProducesResponseType(typeof(StudyTaskResponseDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Update(int id, [FromBody] UpdateStudyTaskDto dto)
    {
        try
        {
            var task = await _service.UpdateAsync(id, dto);
            if (task == null)
                return NotFound(new { message = $"Tarefa com ID {id} não encontrada." });

            return Ok(task);
        }
        catch (KeyNotFoundException ex)
        {
            return NotFound(new { message = ex.Message });
        }
    }

    [HttpDelete("{id:int}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Delete(int id)
    {
        var deleted = await _service.DeleteAsync(id);
        if (!deleted)
            return NotFound(new { message = $"Tarefa com ID {id} não encontrada." });

        return NoContent();
    }

    [HttpGet("stats/status")]
    [ProducesResponseType(typeof(Dictionary<string, int>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetStatsByStatus()
    {
        var stats = await _service.GetCountByStatusAsync();
        var result = stats.ToDictionary(kvp => kvp.Key.ToString(), kvp => kvp.Value);
        return Ok(result);
    }

    [HttpGet("stats/priority")]
    [ProducesResponseType(typeof(Dictionary<string, int>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetStatsByPriority()
    {
        var stats = await _service.GetCountByPriorityAsync();
        var result = stats.ToDictionary(kvp => kvp.Key.ToString(), kvp => kvp.Value);
        return Ok(result);
    }
}
