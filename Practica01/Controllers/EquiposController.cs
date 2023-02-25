using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Practica01.Data;
using Practica01.Models;
using Microsoft.EntityFrameworkCore;

namespace Practica01.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EquiposController : ControllerBase
    {
        private readonly EquipoContext _equipoContext;


        public EquiposController(EquipoContext equipoContext)
        {
            _equipoContext= equipoContext;
        }

        [HttpGet]
        [Route("GetAll")]
        public IActionResult Get()
        {

            List<Equipo> listadoEquipos = (from equipo in _equipoContext.equipos select equipo).ToList();

            if (listadoEquipos.Count == 0)
            {
                return NotFound();
            }

            return Ok(listadoEquipos);

        }

        [HttpGet]
        [Route("getbyid/{id}/")]

        public IActionResult get(int id)
        {

            Equipo? unEquipo = (from e in _equipoContext.equipos where e.id_equipos == id select e).FirstOrDefault();


            if (unEquipo == null)
                return NotFound();


            return Ok(unEquipo);
        }



        [HttpGet]
        [Route("find")]

        public IActionResult buscar(string filtro)
        {
            List<Equipo> equipos = (from e in _equipoContext.equipos where e.nombre.Contains(filtro) || e.descripcion.Contains(filtro) select e).ToList();

            if (equipos.Any())
            {
                return Ok(equipos);
            }

            return NotFound();
        }

        [HttpPost]
        [Route("add")]

        public IActionResult create([FromBody] Equipo equipoNuevo)
        {
            try 
            {
                _equipoContext.equipos.Add(equipoNuevo);
                _equipoContext.SaveChanges();

                return Ok();
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }

            ;
        }

        [HttpPut]
        [Route("actualzar/{id}")]

        public IActionResult actualizarEquipo(int id, [FromBody]Equipo equipoModificar)
        {
            Equipo? equipoExiste = (from e in _equipoContext.equipos where e.id_equipos == id select e).FirstOrDefault();

            if (equipoExiste == null)
                return NotFound();

            equipoExiste.nombre = equipoModificar.nombre;
            equipoExiste.descripcion = equipoModificar.descripcion;

            _equipoContext.Entry(equipoExiste).State = EntityState.Modified;
            _equipoContext.SaveChanges();

            return Ok(equipoExiste);
        }

        [HttpDelete]
        [Route("delete/{id}")]

        public IActionResult deleteEquipo(int id)
        {
            Equipo? equipoExiste = (from e in _equipoContext.equipos where e.id_equipos == id select e).FirstOrDefault();

            if (equipoExiste == null)
                return NotFound();

            equipoExiste.estado = "I";
            _equipoContext.Entry(equipoExiste);
            _equipoContext.SaveChanges();
            //Eliminar no es correcto por lo tanto se actualiza los registros
            //_equipoContext.equipos.Attach(equipoExiste);
            //_equipoContext.equipos.Remove(equipoExiste);
            _equipoContext.SaveChanges();

            return Ok(equipoExiste);
        }
    }
}
