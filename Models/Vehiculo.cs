namespace VehiculosApi.Models
{
    public class Vehiculo
    {
        public int Id { get; set; }
        public required string Marca { get; set; }
        public required string Modelo { get; set; }
        public int Anio { get; set; }
        public decimal Precio { get; set; }
        public bool Disponible { get; set; }
    }
}
