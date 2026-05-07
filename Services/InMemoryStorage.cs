using Final.Models;

namespace Final.Services;

public class InMemoryStorage : IStorageStrategy
{
    private readonly Dictionary<Guid, Note> _notes = new();

    public IEnumerable<Note> LoadAll() => _notes.Values;

    public void Save(Note note)
    {
        note.UpdatedAt = DateTime.UtcNow;
        _notes[note.Id] = note;
    }

    public void Delete(Guid id) => _notes.Remove(id);
}