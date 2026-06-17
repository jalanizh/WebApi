using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Protocols.Configuration;
using WebAPI.Entidades;

namespace WebAPI.Controllers
{
    [Route("api/laptops")]
    [ApiController]
    public class LaptopsController : ControllerBase
    {
        private readonly ApplicationDbContext context;

        public LaptopsController(ApplicationDbContext context)
        {
            this.context = context;
        }


        [HttpGet]
        public async Task<List<Laptop>> Get()
        {
            //await Task.Delay(2000);
            return await context.Laptops.ToListAsync();
        }

        [HttpGet("{id:int}", Name = "ObtenerLaptopPorId")]
        public async Task<ActionResult<Laptop>> Get(int id)
        {
           // await Task.Delay(2000);

            var laptop = await context.Laptops.FirstOrDefaultAsync(x => x.Id == id);

            if (laptop is null)
            {
                return NotFound();
            }

            return laptop;
        }

        [HttpGet("{nombre}/existe")]
        public async Task<ActionResult<bool>> Get(string nombre, int id)
        {
            if (id == 0)
            {
                return await context.Laptops.AnyAsync(x => x.Nombre == nombre);
            }
            else
            {
                return await context.Laptops.AnyAsync(x => x.Nombre == nombre && x.Id!=id);
            }
        }

            [HttpPost]
        public async Task<ActionResult> Post([FromBody] Laptop laptop)
        {
            var yaExiste = await context.Laptops.AnyAsync(x => x.Nombre == laptop.Nombre);
            if (yaExiste)
            {
                var mensaje = $"ya existe la laptop con el nombre {laptop.Nombre}";
                ModelState.AddModelError(nameof(laptop.Nombre), mensaje);
                return ValidationProblem(ModelState);
            }
            context.Add(laptop);
            await context.SaveChangesAsync();
            return CreatedAtRoute("ObtenerLaptopPorId", new { id = laptop.Id }, laptop);
        }

        [HttpPut("{id:int}")]
        public async Task<ActionResult> Put(int id, [FromBody] Laptop laptop)
        {
            var existeLaptop = await context.Laptops.AnyAsync(x => x.Id == id);

            if (!existeLaptop)
            {
                return NotFound();
            }

            var yaExisteNombre = await context.Laptops.AnyAsync(x => x.Nombre == laptop.Nombre && laptop.Id != id);
            if (yaExisteNombre)
            {
                var mensaje = $"ya existe la laptop con el nombre {laptop.Nombre}";
                ModelState.AddModelError(nameof(laptop.Nombre), mensaje);
                return ValidationProblem(ModelState);
            }

            laptop.Id = id;
            context.Update(laptop);
            await context.SaveChangesAsync();
            return NoContent();
        }

        [HttpDelete("{id:int}")]
        public async Task<ActionResult> Delete(int id)
        {
            var filasBorradas = await context.Laptops.Where(x => x.Id == id).ExecuteDeleteAsync();

            if (filasBorradas == 0)
            {
                return NotFound();
            }

            return NoContent();
        }

        [HttpGet("error")]
        public IActionResult Error()
        {
            throw new Exception("Cadena de conexión: Server=100.43.34.321;Database=TiendaDB;User=uTienda;Password=9415Entrya#dds");
        }



    }
}
