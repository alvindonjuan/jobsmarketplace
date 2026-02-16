
using JobsMarketplace.Application.DTOs.Contractor;
using JobsMarketplace.Application.Interfaces.Services;
using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("api/[controller]")]
public class ContractorsController : ControllerBase
{
    private readonly IContractorService _service;

    public ContractorsController(IContractorService service)
    {
        _service = service;
    }

    [HttpGet("{id:guid}")]
    public async Task<IActionResult> GetById(Guid id)
    {
        var result = await _service.GetByIdAsync(id);
        if (result == null)
            return NotFound();

        return Ok(result);
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreateContractorRequest request)
    {
        var id = await _service.CreateAsync(request);
        return CreatedAtAction(nameof(GetById), new { id }, null);
    }

    [HttpPut("{id:guid}")]
    public async Task<IActionResult> Update(Guid id, UpdateContractorRequest request)
    {
        await _service.UpdateAsync(id, request);
        return NoContent();
    }

    //[HttpPut("{id:guid}")]
    //public async Task<IActionResult> UpdateRatingAsync(Guid id, UpdateContractorRequest request)
    //{
    //    await _service.UpdateAsync(id, request);
    //    return NoContent();
    //}

    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> Delete(Guid id)
    {
        await _service.DeleteAsync(id);

        return NoContent();
    }




    [HttpGet("search")]
    public async Task<IActionResult> Search([FromQuery] SearchContractorsRequest request)
    {
        var results = await _service.SearchContractorsAsync(request);
        return Ok(results);
    }


}
