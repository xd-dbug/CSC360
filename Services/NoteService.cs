using Final.Models;

namespace Final.Services;

public class NoteService
{
    private IStorageStrategy _storage;
    private readonly List<INoteObserver> _observers = new();

    public NoteService(IStorageStrategy storage)
    {
        _storage = storage;
    }

    public void SetStorage(IStorageStrategy storage) => _storage = storage;

    public void Subscribe(INoteObserver observer) => _observers.Add(observer);
    public void Unsubscribe(INoteObserver observer) => _observers.Remove(observer);

    public IEnumerable<Note> GetAllNotes() => _storage.LoadAll();

    public void CreateNote(string title, string content)
    {
        var note = new Note { Title = title, Content = content };
        _storage.Save(note);
        Notify();
    }

    public void UpdateNote(Note note)
    {
        _storage.Save(note);
        Notify();
    }

    public void DeleteNote(Guid id)
    {
        _storage.Delete(id);
        Notify();
    }

    private void Notify()
    {
        foreach (var observer in _observers)
            observer.OnNotesChanged();
    }
}