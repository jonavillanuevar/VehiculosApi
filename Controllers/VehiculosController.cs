using Microsoft.AspNetCore.Mvc;
using VehiculosApi.Models;
using VehiculosApi.Repositories;

namespace VehiculosApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class VehiculosController : ControllerBase
    {
        // (1) Guardamos la INTERFAZ, no la clase concreta
        private readonly IVehiculoRepository _repositorio;

        // (2) .NET entrega automáticamente un VehiculoRepository
        public VehiculosController(IVehiculoRepository repositorio)
        {
            _repositorio = repositorio;
        }

        [HttpGet]
        public async Task<ActionResult<ApiResponse<List<Vehiculo>>>> ObtenerVehiculos()
        {
            // (3) Esperamos a la base de datos y guardamos la lista
            var vehiculos = await _repositorio.ObtenerTodosAsync();

            return Ok(new ApiResponse<List<Vehiculo>>
            {
                Success = true,
                Message = vehiculos.Count == 0 ? "No hay vehículos registrados." : "Vehículos obtenidos exitosamente.",
                Data = vehiculos
            });
        }

        [HttpGet("{id:int}")]
        public async Task<ActionResult<ApiResponse<Vehiculo>>> ObtenerPorId(int id)
        {
            var vehiculo = await _repositorio.ObtenerPorIdAsync(id);

            // (4) FindAsync devuelve null si no existe
            if (vehiculo is null)
            {
                return NotFound(new ApiResponse<Vehiculo>
                {
                    Success = false,
                    Message = "Vehículo no encontrado.",
                    Data = null,
                    Errors = new List<string> { $"No existe ningún vehículo con el Id {id}." }
                });
            }

            return Ok(new ApiResponse<Vehiculo>
            {
                Success = true,
                Message = "Vehículo encontrado.",
                Data = vehiculo
            });
        }

        [HttpPost]
        public async Task<ActionResult<ApiResponse<Vehiculo>>> AgregarVehiculo([FromBody] Vehiculo vehiculo)
        {
            // (5) La base de datos genera el Id; ignoramos el que mande el cliente
            vehiculo.Id = 0;

            await _repositorio.AgregarAsync(vehiculo);

            // (6) Después de SaveChanges, vehiculo.Id ya tiene el valor real
            return CreatedAtAction(nameof(ObtenerPorId), new { id = vehiculo.Id }, new ApiResponse<Vehiculo>
            {
                Success = true,
                Message = "Vehículo registrado correctamente.",
                Data = vehiculo
            });
        }

        [HttpPut("{id:int}")]
        public async Task<ActionResult<ApiResponse<Vehiculo>>> ActualizarVehiculo(int id, [FromBody] Vehiculo vehiculo)
        {
            // (1) El Id viene de la URL, no del cuerpo del JSON
            vehiculo.Id = id;

            var actualizado = await _repositorio.ActualizarAsync(vehiculo);

            if (!actualizado)
            {
                return NotFound(new ApiResponse<Vehiculo>
                {
                    Success = false,
                    Message = "Vehículo no encontrado.",
                    Data = null,
                    Errors = new List<string> { $"No existe ningún vehículo con el Id {id}." }
                });
            }

            return Ok(new ApiResponse<Vehiculo>
            {
                Success = true,
                Message = "Vehículo actualizado correctamente.",
                Data = vehiculo
            });
        }

        [HttpDelete("{id:int}")]
        public async Task<ActionResult<ApiResponse<object>>> EliminarVehiculo(int id)
        {
            var eliminado = await _repositorio.EliminarAsync(id);

            if (!eliminado)
            {
                return NotFound(new ApiResponse<object>
                {
                    Success = false,
                    Message = "Vehículo no encontrado.",
                    Data = null,
                    Errors = new List<string> { $"No existe ningún vehículo con el Id {id}." }
                });
            }

            return Ok(new ApiResponse<object>
            {
                Success = true,
                Message = "Vehículo eliminado correctamente.",
                Data = null
            });
        }
    }
}