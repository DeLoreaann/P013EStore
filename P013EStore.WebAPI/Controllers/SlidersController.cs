using Microsoft.AspNetCore.Mvc;
using P013EStore.Core.Entities;
using P013EStore.Service.Abstract;

namespace P013EStore.WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SlidersController : ControllerBase
    {
        private readonly IService<Slider> _service;

        public SlidersController(IService<Slider> service)
        {
            _service = service;
        }

        // GET: api/<SlidersController>
        [HttpGet]
        public async Task<IEnumerable<Slider>> GetAsync()
        {
            return await _service.GetAllAsync();
        }

        // GET api/<SlidersController>/5
        [HttpGet("{id}")]
        public async Task<Slider> GetAsync(int id)
        {
            return await _service.FindAsync(id);
        }

        // POST api/<SlidersController>
        [HttpPost]
        public async Task<IActionResult> Post([FromBody] Slider value)
        {
            await _service.AddAsync(value);
            await _service.SaveAsync();
            return Ok(value);
        }

        // PUT api/<SlidersController>/5
        [HttpPut]
        public async Task<IActionResult> Put([FromBody] Slider value)
        {
            _service.Update(value);
            await _service.SaveAsync();
            return Ok(value);
        }

        // DELETE api/<SlidersController>/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var data = await _service.FindAsync(id);
            if (data == null)
            {
                return NotFound();
            }
            _service.Delete(data);
            var sonuc = await _service.SaveAsync();
            if (sonuc > 0)
            {
                return Ok(data);
            }
            return Problem();
        }
    }
}
