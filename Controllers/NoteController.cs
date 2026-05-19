using Final.Services;

namespace Final.Controllers;

public class NoteController : INoteObserver
{
    private readonly NoteService _noteService;

    public NoteController(NoteService noteService)
    {
        _noteService = noteService;
        _noteService.Subscribe(this);
    }

    public void OnNotesChanged()
    {
        Console.WriteLine("\n[NoteController] Notes updated:");
        foreach (var note in _noteService.GetAllNotes())
            Console.WriteLine($"  [{note.Id}] {note.Title}: {note.Content}");
    }
}