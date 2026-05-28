using Final.Controllers;
using Final.Services;
using Final.Views;
using Gtk;

namespace Final;

class Program
{
    static void Main(string[] args)
    {

        Application.Init();
        var service = NoteService.GetInstance(new JsonFileStorage("notes.json"));
        var window  = new MainWindow(service);
        Application.Run();
        /*// === SINGLETON: obtain the single NoteService instance ===
        IStorageStrategy storage = new JsonFileStorage("notes.json");
        var noteService = NoteService.GetInstance(storage);

        // Prove the Singleton — a second call returns the exact same object
        var anotherRef = NoteService.GetInstance(new InMemoryStorage());
        Console.WriteLine("=== Singleton Pattern ===");
        Console.WriteLine($"Same instance? {ReferenceEquals(noteService, anotherRef)}");  // True

        // === OBSERVER: NoteController subscribes to change notifications ===
        var controller = new NoteController(noteService);

        // === STRATEGY: JsonFileStorage is the active storage backend ===
        Console.WriteLine("\n=== Strategy + Observer Pattern ===");
        noteService.CreateNote("First Note", "Hello from the Strategy pattern!");
        noteService.CreateNote("Second Note", "Storage backend is easily swappable.");

        // Swap strategy to in-memory at runtime
        Console.WriteLine("\n--- Switching strategy to InMemoryStorage ---");
        noteService.SetStorage(new InMemoryStorage());
        noteService.CreateNote("Third Note", "Now stored in memory only.");*/
    }
}
