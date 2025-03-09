using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Orders.Backend.Data;
using Orders.Shared.Entities;

namespace Orders.Backend.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CountriesController(DataContext context) : ControllerBase
    {
        [HttpGet]
        public async Task<IActionResult> GetAsync()
        {
            var countries = await context.Countries.ToListAsync();
            return Ok(countries);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetAsyncId(int id)
        {
            var country = await context.Countries.FindAsync(id);
            if (country == null)
            {
                return NotFound();
            }
            return Ok(country);
        }

        [HttpPost]
        public async Task<IActionResult> PostAsync(Country country)
        {
            context.Add(country);
            await context.SaveChangesAsync();
            return Ok(country);
        }

        [HttpPatch]
        public async Task<IActionResult> PatchAsync(Country country)
        {
            context.Update(country);
            await context.SaveChangesAsync();
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteAsync(int id)
        {
            var country = await context.Countries.FindAsync(id);
            if (country == null)
            {
                return NotFound();
            }
            context.Remove(country);
            await context.SaveChangesAsync();
            return NoContent();
        }
    }
}