namespace BibliotecaApp.Models;

public class Prestamo
{
    public int Id { get; set; }
    public int LibroId { get; set; }
    public int UsuarioId { get; set; }
    public DateTime FechaPrestamo { get; set; }
    public DateTime FechaDevolucion { get; set; }
    
    public Prestamo(int id, int libroId, int usuarioId, DateTime fechaPrestamo, DateTime fechaDevolucion)
    {
        Id = id;
        LibroId = libroId;
        UsuarioId = usuarioId;
        FechaPrestamo = fechaPrestamo;
        FechaDevolucion = fechaDevolucion;
    }
}