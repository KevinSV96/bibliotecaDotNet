using Gtk;
using BibliotecaApp.Services;
using BibliotecaApp.Models;
using System;
using System.Linq;

namespace BibliotecaApp;

public class MainWindow : Window
{
    private Notebook? notebook;
    private ListStore? listStoreLibros;
    private ListStore? listStoreUsuarios;
    private ListStore? listStorePrestamos;
    private TreeView? treeLibros;
    private TreeView? treeUsuarios;
    private TreeView? treePrestamos;
    private BibliotecaService service;
    
    public MainWindow() : base(WindowType.Toplevel)
    {
        service = new BibliotecaService();
        
        // Configurar ventana
        Title = "Sistema de Gestión de Biblioteca";
        SetDefaultSize(900, 600);
        SetPosition(WindowPosition.Center);
        BorderWidth = 10;
        
        // Conectar evento de cierre
        DeleteEvent += (o, args) => Application.Quit();
        
        // Construir interfaz
        BuildInterface();
        
        // Cargar datos después de que la ventana se muestre
        Shown += (sender, e) => CargarDatos();
    }
    
    private void BuildInterface()
    {
        // Notebook para las pestañas
        notebook = new Notebook();
        Add(notebook);
        
        // Crear pestañas
        CrearPestañaLibros();
        CrearPestañaUsuarios();
        CrearPestañaPrestamos();
    }
    
    private void CrearPestañaLibros()
    {
        var vbox = new Box(Orientation.Vertical, 5);
        var frame = new Frame("Administración de Libros");
        frame.Add(vbox);
        
        // Botones
        var hboxBotones = new Box(Orientation.Horizontal, 5);
        var btnAgregar = new Button("Agregar Libro");
        var btnEliminar = new Button("Eliminar Libro");
        
        btnAgregar.Clicked += OnAgregarLibro;
        btnEliminar.Clicked += OnEliminarLibro;
        
        hboxBotones.PackStart(btnAgregar, true, true, 0);
        hboxBotones.PackStart(btnEliminar, true, true, 0);
        vbox.PackStart(hboxBotones, false, false, 5);
        
        // TreeView para libros
        listStoreLibros = new ListStore(typeof(int), typeof(string), typeof(string), typeof(int));
        treeLibros = new TreeView(listStoreLibros);
        
        // Columnas
        treeLibros.AppendColumn("ID", new CellRendererText(), "text", 0);
        treeLibros.AppendColumn("Título", new CellRendererText(), "text", 1);
        treeLibros.AppendColumn("Autor", new CellRendererText(), "text", 2);
        treeLibros.AppendColumn("Año", new CellRendererText(), "text", 3);
        
        // Scroll
        var scrolledWindow = new ScrolledWindow();
        scrolledWindow.Add(treeLibros);
        scrolledWindow.SetSizeRequest(-1, 400);
        vbox.PackStart(scrolledWindow, true, true, 0);
        
        notebook.AppendPage(frame, new Label("📚 Libros"));
    }
    
    private void CrearPestañaUsuarios()
    {
        var vbox = new Box(Orientation.Vertical, 5);
        var frame = new Frame("Administración de Usuarios");
        frame.Add(vbox);
        
        // Botones
        var hboxBotones = new Box(Orientation.Horizontal, 5);
        var btnAgregar = new Button("Agregar Usuario");
        var btnEliminar = new Button("Eliminar Usuario");
        
        btnAgregar.Clicked += OnAgregarUsuario;
        btnEliminar.Clicked += OnEliminarUsuario;
        
        hboxBotones.PackStart(btnAgregar, true, true, 0);
        hboxBotones.PackStart(btnEliminar, true, true, 0);
        vbox.PackStart(hboxBotones, false, false, 5);
        
        // TreeView para usuarios
        listStoreUsuarios = new ListStore(typeof(int), typeof(string), typeof(string));
        treeUsuarios = new TreeView(listStoreUsuarios);
        
        // Columnas
        treeUsuarios.AppendColumn("ID", new CellRendererText(), "text", 0);
        treeUsuarios.AppendColumn("Nombre", new CellRendererText(), "text", 1);
        treeUsuarios.AppendColumn("Email", new CellRendererText(), "text", 2);
        
        // Scroll
        var scrolledWindow = new ScrolledWindow();
        scrolledWindow.Add(treeUsuarios);
        scrolledWindow.SetSizeRequest(-1, 400);
        vbox.PackStart(scrolledWindow, true, true, 0);
        
        notebook.AppendPage(frame, new Label("👥 Usuarios"));
    }
    
    private void CrearPestañaPrestamos()
    {
        var vbox = new Box(Orientation.Vertical, 5);
        var frame = new Frame("Gestión de Préstamos");
        frame.Add(vbox);
        
        // Botones
        var hboxBotones = new Box(Orientation.Horizontal, 5);
        var btnNuevoPrestamo = new Button("Nuevo Préstamo");
        var btnEliminarPrestamo = new Button("Eliminar Préstamo");
        
        btnNuevoPrestamo.Clicked += OnNuevoPrestamo;
        btnEliminarPrestamo.Clicked += OnEliminarPrestamo;
        
        hboxBotones.PackStart(btnNuevoPrestamo, true, true, 0);
        hboxBotones.PackStart(btnEliminarPrestamo, true, true, 0);
        vbox.PackStart(hboxBotones, false, false, 5);
        
        // TreeView para préstamos
        listStorePrestamos = new ListStore(typeof(int), typeof(string), typeof(string), typeof(string), typeof(string));
        treePrestamos = new TreeView(listStorePrestamos);
        
        // Columnas
        treePrestamos.AppendColumn("ID", new CellRendererText(), "text", 0);
        treePrestamos.AppendColumn("Libro", new CellRendererText(), "text", 1);
        treePrestamos.AppendColumn("Usuario", new CellRendererText(), "text", 2);
        treePrestamos.AppendColumn("Fecha Préstamo", new CellRendererText(), "text", 3);
        treePrestamos.AppendColumn("Fecha Devolución", new CellRendererText(), "text", 4);
        
        // Scroll
        var scrolledWindow = new ScrolledWindow();
        scrolledWindow.Add(treePrestamos);
        scrolledWindow.SetSizeRequest(-1, 400);
        vbox.PackStart(scrolledWindow, true, true, 0);
        
        notebook.AppendPage(frame, new Label("📋 Préstamos"));
    }
    
    private void CargarDatos()
    {
        CargarLibros();
        CargarUsuarios();
        CargarPrestamos();
    }
    
    private void CargarLibros()
    {
        if (listStoreLibros == null) return;
        
        listStoreLibros.Clear();
        foreach (var libro in service.Libros)
        {
            listStoreLibros.AppendValues(libro.Id, libro.Titulo, libro.Autor, libro.Anio);
        }
    }
    
    private void CargarUsuarios()
    {
        if (listStoreUsuarios == null) return;
        
        listStoreUsuarios.Clear();
        foreach (var usuario in service.Usuarios)
        {
            listStoreUsuarios.AppendValues(usuario.Id, usuario.Nombre, usuario.Email);
        }
    }
    
    private void CargarPrestamos()
    {
        if (listStorePrestamos == null) return;
        
        listStorePrestamos.Clear();
        foreach (var prestamo in service.Prestamos)
        {
            var libro = service.ObtenerLibroPorId(prestamo.LibroId);
            var usuario = service.ObtenerUsuarioPorId(prestamo.UsuarioId);
            
            listStorePrestamos.AppendValues(
                prestamo.Id,
                libro?.Titulo ?? "Desconocido",
                usuario?.Nombre ?? "Desconocido",
                prestamo.FechaPrestamo.ToString("yyyy-MM-dd"),
                prestamo.FechaDevolucion.ToString("yyyy-MM-dd")
            );
        }
    }
    
    // Event handlers - Libros
    private void OnAgregarLibro(object? sender, EventArgs args)
    {
        var dialog = new Dialog("Agregar Nuevo Libro", this, DialogFlags.Modal)
        {
            DefaultWidth = 300,
            DefaultHeight = 200
        };
        
        var contentArea = dialog.ContentArea;
        var vbox = new Box(Orientation.Vertical, 5);
        
        var entryTitulo = new Entry();
        var entryAutor = new Entry();
        var entryAnio = new Entry();
        
        vbox.PackStart(new Label("Título:"), false, false, 0);
        vbox.PackStart(entryTitulo, false, false, 0);
        vbox.PackStart(new Label("Autor:"), false, false, 0);
        vbox.PackStart(entryAutor, false, false, 0);
        vbox.PackStart(new Label("Año:"), false, false, 0);
        vbox.PackStart(entryAnio, false, false, 0);
        
        contentArea.PackStart(vbox, true, true, 0);
        
        dialog.AddButton("Cancelar", ResponseType.Cancel);
        dialog.AddButton("Agregar", ResponseType.Ok);
        
        dialog.ShowAll();
        
        if (dialog.Run() == (int)ResponseType.Ok)
        {
            if (int.TryParse(entryAnio.Text, out int anio) && !string.IsNullOrWhiteSpace(entryTitulo.Text))
            {
                var nuevoLibro = new Libro(service.ObtenerProximoIdLibro(), entryTitulo.Text, entryAutor.Text, anio);
                service.AgregarLibro(nuevoLibro);
                CargarLibros();
                ShowInfoDialog("Éxito", "Libro agregado correctamente");
            }
            else
            {
                ShowErrorDialog("Error", "Por favor ingrese un año válido y un título");
            }
        }
        
        dialog.Destroy();
    }
    
    private void OnEliminarLibro(object? sender, EventArgs args)
    {
        if (listStoreLibros != null && treeLibros != null && treeLibros.Selection.GetSelected(out TreeIter iter))
        {
            var id = (int)listStoreLibros.GetValue(iter, 0);
            var titulo = (string)listStoreLibros.GetValue(iter, 1);
            
            var confirmDialog = new MessageDialog(this, DialogFlags.Modal, MessageType.Question, ButtonsType.YesNo, 
                $"¿Está seguro de eliminar el libro '{titulo}'?");
            confirmDialog.Title = "Confirmar eliminación";
            
            if (confirmDialog.Run() == (int)ResponseType.Yes)
            {
                service.EliminarLibro(id);
                CargarLibros();
                ShowInfoDialog("Éxito", "Libro eliminado correctamente");
            }
            
            confirmDialog.Destroy();
        }
        else
        {
            ShowErrorDialog("Error", "Por favor seleccione un libro para eliminar");
        }
    }
    
    // Event handlers - Usuarios
    private void OnAgregarUsuario(object? sender, EventArgs args)
    {
        var dialog = new Dialog("Agregar Nuevo Usuario", this, DialogFlags.Modal)
        {
            DefaultWidth = 300,
            DefaultHeight = 200
        };
        
        var contentArea = dialog.ContentArea;
        var vbox = new Box(Orientation.Vertical, 5);
        
        var entryNombre = new Entry();
        var entryEmail = new Entry();
        
        vbox.PackStart(new Label("Nombre:"), false, false, 0);
        vbox.PackStart(entryNombre, false, false, 0);
        vbox.PackStart(new Label("Email:"), false, false, 0);
        vbox.PackStart(entryEmail, false, false, 0);
        
        contentArea.PackStart(vbox, true, true, 0);
        
        dialog.AddButton("Cancelar", ResponseType.Cancel);
        dialog.AddButton("Agregar", ResponseType.Ok);
        
        dialog.ShowAll();
        
        if (dialog.Run() == (int)ResponseType.Ok)
        {
            if (!string.IsNullOrWhiteSpace(entryNombre.Text))
            {
                var nuevoUsuario = new Usuario(service.ObtenerProximoIdUsuario(), entryNombre.Text, entryEmail.Text);
                service.AgregarUsuario(nuevoUsuario);
                CargarUsuarios();
                ShowInfoDialog("Éxito", "Usuario agregado correctamente");
            }
            else
            {
                ShowErrorDialog("Error", "Por favor ingrese un nombre válido");
            }
        }
        
        dialog.Destroy();
    }
    
    private void OnEliminarUsuario(object? sender, EventArgs args)
    {
        if (listStoreUsuarios != null && treeUsuarios != null && treeUsuarios.Selection.GetSelected(out TreeIter iter))
        {
            var id = (int)listStoreUsuarios.GetValue(iter, 0);
            var nombre = (string)listStoreUsuarios.GetValue(iter, 1);
            
            var confirmDialog = new MessageDialog(this, DialogFlags.Modal, MessageType.Question, ButtonsType.YesNo, 
                $"¿Está seguro de eliminar al usuario '{nombre}'?");
            confirmDialog.Title = "Confirmar eliminación";
            
            if (confirmDialog.Run() == (int)ResponseType.Yes)
            {
                service.EliminarUsuario(id);
                CargarUsuarios();
                ShowInfoDialog("Éxito", "Usuario eliminado correctamente");
            }
            
            confirmDialog.Destroy();
        }
        else
        {
            ShowErrorDialog("Error", "Por favor seleccione un usuario para eliminar");
        }
    }
    
    // Event handlers - Préstamos
    private void OnNuevoPrestamo(object? sender, EventArgs args)
    {
        if (service.Libros.Count == 0 || service.Usuarios.Count == 0)
        {
            ShowErrorDialog("Error", "Debe haber al menos un libro y un usuario registrado");
            return;
        }

        var dialog = new Dialog("Nuevo Préstamo", this, DialogFlags.Modal)
        {
            DefaultWidth = 400,
            DefaultHeight = 300
        };
        
        var contentArea = dialog.ContentArea;
        var vbox = new Box(Orientation.Vertical, 5);
        
        // ComboBox para Libros
        var comboLibros = new ComboBox(service.Libros.Select(l => l.ToString()).ToArray());
        var comboUsuarios = new ComboBox(service.Usuarios.Select(u => u.ToString()).ToArray());
        
        var entryDiasPrestamo = new Entry { Text = "15" };
        
        vbox.PackStart(new Label("Libro:"), false, false, 0);
        vbox.PackStart(comboLibros, false, false, 0);
        vbox.PackStart(new Label("Usuario:"), false, false, 0);
        vbox.PackStart(comboUsuarios, false, false, 0);
        vbox.PackStart(new Label("Días de préstamo:"), false, false, 0);
        vbox.PackStart(entryDiasPrestamo, false, false, 0);
        
        contentArea.PackStart(vbox, true, true, 0);
        
        dialog.AddButton("Cancelar", ResponseType.Cancel);
        dialog.AddButton("Crear Préstamo", ResponseType.Ok);
        
        dialog.ShowAll();
        
        if (dialog.Run() == (int)ResponseType.Ok)
        {
            if (int.TryParse(entryDiasPrestamo.Text, out int dias) && dias > 0)
            {
                var libroIndex = comboLibros.Active;
                var usuarioIndex = comboUsuarios.Active;
                
                if (libroIndex >= 0 && usuarioIndex >= 0)
                {
                    var libro = service.Libros[libroIndex];
                    var usuario = service.Usuarios[usuarioIndex];
                    var fechaPrestamo = DateTime.Now;
                    var fechaDevolucion = fechaPrestamo.AddDays(dias);
                    
                    var nuevoPrestamo = new Prestamo(
                        service.ObtenerProximoIdPrestamo(),
                        libro.Id,
                        usuario.Id,
                        fechaPrestamo,
                        fechaDevolucion
                    );
                    
                    service.AgregarPrestamo(nuevoPrestamo);
                    CargarPrestamos();
                    ShowInfoDialog("Éxito", "Préstamo creado correctamente");
                }
            }
            else
            {
                ShowErrorDialog("Error", "Por favor ingrese un número válido de días");
            }
        }
        
        dialog.Destroy();
    }
    
    private void OnEliminarPrestamo(object? sender, EventArgs args)
    {
        if (listStorePrestamos != null && treePrestamos != null && treePrestamos.Selection.GetSelected(out TreeIter iter))
        {
            var id = (int)listStorePrestamos.GetValue(iter, 0);
            var libro = (string)listStorePrestamos.GetValue(iter, 1);
            
            var confirmDialog = new MessageDialog(this, DialogFlags.Modal, MessageType.Question, ButtonsType.YesNo, 
                $"¿Está seguro de eliminar el préstamo del libro '{libro}'?");
            confirmDialog.Title = "Confirmar eliminación";
            
            if (confirmDialog.Run() == (int)ResponseType.Yes)
            {
                service.EliminarPrestamo(id);
                CargarPrestamos();
                ShowInfoDialog("Éxito", "Préstamo eliminado correctamente");
            }
            
            confirmDialog.Destroy();
        }
        else
        {
            ShowErrorDialog("Error", "Por favor seleccione un préstamo para eliminar");
        }
    }
    
    // Métodos auxiliares
    private void ShowInfoDialog(string title, string message)
    {
        var md = new MessageDialog(this, 
            DialogFlags.Modal, 
            MessageType.Info, 
            ButtonsType.Ok, 
            message)
        {
            Title = title
        };
        
        md.Run();
        md.Destroy();
    }
    
    private void ShowErrorDialog(string title, string message)
    {
        var md = new MessageDialog(this, 
            DialogFlags.Modal, 
            MessageType.Error, 
            ButtonsType.Ok, 
            message)
        {
            Title = title
        };
        
        md.Run();
        md.Destroy();
    }
}