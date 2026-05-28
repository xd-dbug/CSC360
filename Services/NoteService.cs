using Final.Models;

namespace Final.Services;

// === SINGLETON PATTERN (Creational) ===
// Ensures only one NoteService instance exists for the lifetime of the app.
public class NoteService
{
    // Static instance — stores the single object (Singleton element)
    private static NoteService? _instance;
    private static readonly object _lock = new();

    // === STRATEGY PATTERN (Behavioral) ===
    // The storage backend is swappable at runtime without changing NoteService.
    private IStorageStrategy _storage;

    // === OBSERVER PATTERN (Behavioral) ===
    // Maintains a list of observers that are notified on every change.
    private readonly List<INoteObserver> _observers = new();

    // Private constructor — prevents external instantiation (Singleton element)
    private NoteService(IStorageStrategy storage)
    {
        _storage = storage;
    }

    // Public static method — provides global access to the single instance (Singleton element)
    public static NoteService GetInstance(IStorageStrategy storage)
    {
        if (_instance == null)
        {
            // Double-checked locking keeps this thread-safe
            lock (_lock)
            {
                _instance ??= new NoteService(storage);
            }
        }
        return _instance;
    }

    // Swap the storage strategy at runtime (Strategy pattern)
    public void SetStorage(IStorageStrategy storage) => _storage = storage;

    // Observer registration
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

    // Notifies all registered observers (Observer pattern)
    private void Notify()
    {
        foreach (var observer in _observers)
            observer.OnNotesChanged();
    }
}