using Microsoft.AspNetCore.Mvc;
using P013EStore.Core.Entities;
using P013EStore.Service.Abstract;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace P013EStore.WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AppUsersController : ControllerBase
    {
        private readonly IService<AppUser> _service;

        public AppUsersController(IService<AppUser> service)
        {
            _service = service;
        }
        // GET: api/<AppUsersController>
        [HttpGet]
        public IEnumerable<AppUser> Get() // liste döner
        {
            return _service.GetAll();
        }

        // GET api/<AppUsersController>/5
        [HttpGet("{id}")]
        public AppUser Get(int id) // tek kayıt döner
        {
            return _service.Find(id);
        }

        // POST api/<AppUsersController>
        [HttpPost]
        public void Post([FromBody] AppUser value)
        {
            _service.Add(value);
            _service.Save();
        }

        // PUT api/<AppUsersController>/5
        [HttpPut] // http put güncelleme için kullanılıyor.
        public void Put(int id, [FromBody] AppUser value)
        {
            _service.Update(value);
            _service.Save();
        }

        // DELETE api/<AppUsersController>/5
        [HttpDelete("{id}")]
        public ActionResult Delete(int id) // metot kurulumda standart olarak void geliyor. geriye birşey döndürmek için void yerine actionresult kullanıyoruz.
        {
            var kayit = _service.Find(id);
            if (kayit == null)
            {
                return BadRequest();
            }
            _service.Delete(kayit);
            _service.Save();
            return Ok(kayit);
        }
    }
}
