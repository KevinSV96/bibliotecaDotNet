using BibliotecaApp.Models;
using System.Linq;

namespace BibliotecaApp.Services;

public class BibliotecaService
{
    public List<Libro> Libros { get; private set; }
    public List<Usuario> Usuarios { get; private set; }
    public List<Prestamo> Prestamos { get; private set; }
    
    public BibliotecaService()
    {
        Libros = new List<Libro>();
        Usuarios = new List<Usuario>();
        Prestamos = new List<Prestamo>();
        CargarDatosEjemplo();
    }
    
    private void CargarDatosEjemplo()
    {
        // Libros de ejemplo
        AgregarLibro(new Libro(1, "El Gran Gatsby", "F. Scott Fitzgerald", 1925));
        AgregarLibro(new Libro(2, "1984", "George Orwell", 1949));
        AgregarLibro(new Libro(3, "Cien años de soledad", "Gabriel García Márquez", 1967));
        AgregarLibro(new Libro(4, "Don Quijote", "Miguel de Cervantes", 1605));
        
        // Usuarios de ejemplo
        AgregarUsuario(new Usuario(1, "Juan Pérez", "juan.perez@mail.com"));
        AgregarUsuario(new Usuario(2, "María García", "maria.garcia@mail.com"));
        AgregarUsuario(new Usuario(3, "Carlos López", "carlos.lopez@mail.com"));
        
        // Préstamos de ejemplo
        var hoy = DateTime.Now;
        AgregarPrestamo(new Prestamo(1, 1, 1, hoy.AddDays(-10), hoy.AddDays(20)));
        AgregarPrestamo(new Prestamo(2, 2, 2, hoy.AddDays(-5), hoy.AddDays(15)));
    }
    
    // CRUD Libros
    public void AgregarLibro(Libro libro) => Libros.Add(libro);
    
    public void EliminarLibro(int id)
    {
        var libro = Libros.FirstOrDefault(l => l.Id == id);
        if (libro != null)
            Libros.Remove(libro);
    }
    
    public void ActualizarLibro(int id, string titulo, string autor, int anio)
    {
        var libro = Libros.FirstOrDefault(l => l.Id == id);
        if (libro != null)
        {
            libro.Titulo = titulo;
            libro.Autor = autor;
            libro.Anio = anio;
        }
    }
    
    // CRUD Usuarios
    public void AgregarUsuario(Usuario usuario) => Usuarios.Add(usuario);
    
    public void EliminarUsuario(int id)
    {
        var usuario = Usuarios.FirstOrDefault(u => u.Id == id);
        if (usuario != null)
            Usuarios.Remove(usuario);
    }
    
    // CRUD Préstamos
    public void AgregarPrestamo(Prestamo prestamo) => Prestamos.Add(prestamo);
    
    public void EliminarPrestamo(int id)
    {
        var prestamo = Prestamos.FirstOrDefault(p => p.Id == id);
        if (prestamo != null)
            Prestamos.Remove(prestamo);
    }
    
    // Métodos de consulta
    public Libro? ObtenerLibroPorId(int id) => Libros.FirstOrDefault(l => l.Id == id);
    public Usuario? ObtenerUsuarioPorId(int id) => Usuarios.FirstOrDefault(u => u.Id == id);
    
    // Métodos para obtener próximos IDs
    public int ObtenerProximoIdLibro() => Libros.Count > 0 ? Libros.Max(l => l.Id) + 1 : 1;
    public int ObtenerProximoIdUsuario() => Usuarios.Count > 0 ? Usuarios.Max(u => u.Id) + 1 : 1;
    public int ObtenerProximoIdPrestamo() => Prestamos.Count > 0 ? Prestamos.Max(p => p.Id) + 1 : 1;
    
    // Métodos de validación
    public bool ExisteLibro(int id) => Libros.Any(l => l.Id == id);
    public bool ExisteUsuario(int id) => Usuarios.Any(u => u.Id == id);
}