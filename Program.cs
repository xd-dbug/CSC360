using Final.Services;

namespace Final;

class Program
{
    static void Main(string[] args)
    {
        IStorageStrategy storage = new JsonFileStorage("notes.json");
        var noteService = new NoteService(storage);

        noteService.CreateNote("First Note", "Hello from the Strategy pattern!");
        noteService.CreateNote("Second Note", "Storage backend is easily swappable.");

        foreach (var note in noteService.GetAllNotes())
            Console.WriteLine($"[{note.Id}] {note.Title}: {note.Content}");
    }
}