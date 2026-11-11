using Microsoft.EntityFrameworkCore;
using VehiculosApi.Data;
using VehiculosApi.Models;

namespace VehiculosApi.Repositories
{
    public class VehiculoRepository : IVehiculoRepository
    {

        private readonly VehiculosDbContext _context;       // Campo para guardar la referencia

        // .NET ve este constructor y entrega automáticamente un VehiculosDbContext
        public VehiculoRepository(VehiculosDbContext context)
        {
            _context = context;
        }


        public async Task AgregarAsync(Vehiculo vehiculo)
        {
            _context.Vehiculos.Add(vehiculo);
            await _context.SaveChangesAsync();
        }


        public async Task<Vehiculo?> ObtenerPorIdAsync(int id)
            => await _context.Vehiculos.FindAsync(id);


        public async Task<List<Vehiculo>> ObtenerTodosAsync()
            => await _context.Vehiculos.ToListAsync();


        public async Task<bool> ActualizarAsync(Vehiculo vehiculo)
        {
            // (1) Buscamos el registro REAL en la base de datos
            var existente = await _context.Vehiculos.FindAsync(vehiculo.Id);

            if (existente is null)
                return false;

            // (2) Copiamos los valores nuevos al objeto que EF ya está siguiendo
            existente.Marca = vehiculo.Marca;
            existente.Modelo = vehiculo.Modelo;
            existente.Anio = vehiculo.Anio;
            existente.Precio = vehiculo.Precio;
            existente.Disponible = vehiculo.Disponible;

            // (3) EF detecta los cambios y genera el UPDATE
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> EliminarAsync(int id)
        {
            var vehiculo = await _context.Vehiculos.FindAsync(id);

            if (vehiculo is null)
                return false;

            _context.Vehiculos.Remove(vehiculo);   // Marca el objeto para borrar
            await _context.SaveChangesAsync();     // Genera el DELETE
            return true;
        }
    }
}
