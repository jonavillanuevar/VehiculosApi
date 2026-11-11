using VehiculosApi.Models;

namespace VehiculosApi.Repositories
{
    public interface IVehiculoRepository
    {
        Task<List<Vehiculo>> ObtenerTodosAsync();
        Task<Vehiculo?> ObtenerPorIdAsync(int id);
        Task AgregarAsync(Vehiculo vehiculo);

        // Devuelven bool: true si encontraron el vehículo, false si no existe
        Task<bool> ActualizarAsync(Vehiculo vehiculo);
        Task<bool> EliminarAsync(int id);
    }
}
