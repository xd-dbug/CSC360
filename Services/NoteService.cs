using Final.Models;

namespace Final.Services;

public class NoteService
{
    private IStorageStrategy _storage;

    public NoteService(IStorageStrategy storage)
    {
        _storage = storage;
    }

    // Swap the storage backend at runtime
    public void SetStorage(IStorageStrategy storage) => _storage = storage;

    public IEnumerable<Note> GetAllNotes() => _storage.LoadAll();

    public void CreateNote(string title, string content)
    {
        var note = new Note { Title = title, Content = content };
        _storage.Save(note);
    }

    public void UpdateNote(Note note) => _storage.Save(note);

    public void DeleteNote(Guid id) => _storage.Delete(id);
}