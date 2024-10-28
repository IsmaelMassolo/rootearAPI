using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using rootearAPI.Data;

namespace rootearAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LugarController : ControllerBase
    {
        private readonly apiContext _context;

        public LugarController(apiContext context)
        {
            _context = context;
        }


        [HttpGet(Name = "GetLugar")]
        public async Task<IActionResult> Obtener()
        {
            try
            {
                var lista = await _context.LUGAR.ToListAsync();
                return Ok(lista);
            }
            catch (Exception ex) { return BadRequest(ex.Message); }
        }
    }
}
