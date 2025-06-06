using System.Text.Json;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PopsicleFactory.DataProvider.Models;
using PopsicleFactory.WebApi.Models;
using PopsicleFactory.WebApi.Services;

namespace PopsicleFactory.WebApi.Controllers.V1;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class PopsicleController(IPopsicleService _service, IMapper mapper) : ControllerBase
{
    [HttpGet]
    public async Task<ActionResult<IEnumerable<PopsicleReadDto>>> GetAllPopsicles()
    {
        var popsicles = await _service.GetAllPopsicles();
        return Ok(mapper.Map<IEnumerable<PopsicleReadDto>>(popsicles));
    }

    [HttpGet("{id}", Name = "GetPopsicleById")]
    public async Task<ActionResult<PopsicleReadDto>> Get(Guid id)
    {
        var popsicle = await _service.GetPopsicleById(id);

        if (popsicle is not null) return Ok(mapper.Map<PopsicleReadDto>(popsicle));

        return NotFound("Popsicle not found.");
    }

    [HttpGet("Search")]
    public async Task<ActionResult<IEnumerable<PopsicleReadDto>>> Search([FromQuery] string? flavor, [FromQuery] decimal? minPrice, [FromQuery] decimal? maxPrice)
    {
        var popsicles = await _service.Search(flavor, minPrice, maxPrice);
        return Ok(mapper.Map<IEnumerable<PopsicleReadDto>>(popsicles));
    }

    [HttpPost]
    [Authorize(Roles = "Admin")]
    public async Task<ActionResult<PopsicleReadDto>> CreatePopsicle([FromBody] PopsicleCreateDto popsicle)
    {
        var popsicleModel = mapper.Map<Popsicle>(popsicle);

        var created = await _service.CreatePopsicle(popsicleModel);
        return CreatedAtAction(nameof(Get), new { id = created.Id }, created);
    }


    [HttpPut("{id}")]
    [Authorize(Roles = "Admin")]
    public async Task<ActionResult<PopsicleReadDto>> Update(Guid id, [FromBody] PopsicleCreateDto popsicle)
    {
        var popsicleModel = mapper.Map<Popsicle>(popsicle);

        var result = await _service.UpdateInformation(id, popsicleModel);
        return result == null ? NotFound("Popsicle not found.") : Ok(mapper.Map<PopsicleReadDto>(result));
    }

    [HttpPatch("{id}")]
    [Authorize(Roles = "Admin")]
    public async Task<ActionResult<PopsicleReadDto>> PartialUpdate(Guid id, [FromBody] JsonElement updates)
    {
        var result = await _service.PartialUpdate(id, updates);
        return result == null ? NotFound("Popsicle not found.") : Ok(mapper.Map<PopsicleReadDto>(result));
    }

    [HttpDelete("{id}")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> DeletePopsicle(Guid id)
    {
        var success = await _service.DeletePopsicle(id);
        return success ? NoContent() : NotFound("Popsicle not found.");
    }
}
