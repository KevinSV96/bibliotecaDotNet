using Gtk;
using Application = Gtk.Application;

namespace BibliotecaApp;

class Program
{
    static void Main(string[] args)
    {
        Application.Init();
        
        // Crear y mostrar la ventana principal
        var mainWindow = new MainWindow();
        mainWindow.ShowAll();
        
        Application.Run();
    }
}