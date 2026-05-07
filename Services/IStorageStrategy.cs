using Final.Models;

namespace Final.Services;

public interface IStorageStrategy
{
    IEnumerable<Note> LoadAll();
    void Save(Note note);
    void Delete(Guid id);
}