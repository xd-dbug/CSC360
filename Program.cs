using Final.Controllers;
using Final.Services;

namespace Final;

class Program
{
    static void Main(string[] args)
    {
        IStorageStrategy storage = new JsonFileStorage("notes.json");
        var noteService = new NoteService(storage);
        var controller = new NoteController(noteService);

        noteService.CreateNote("First Note", "Hello from the Strategy pattern!");
        noteService.CreateNote("Second Note", "Storage backend is easily swappable.");
    }
}
