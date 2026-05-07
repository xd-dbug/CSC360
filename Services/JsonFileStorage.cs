using System.Text.Json;
using Final.Models;

namespace Final.Services;

public class JsonFileStorage : IStorageStrategy
{
    private readonly string _filePath;
    private static readonly JsonSerializerOptions _jsonOptions = new() { WriteIndented = true };

    public JsonFileStorage(string filePath = "notes.json")
    {
        _filePath = filePath;
    }

    public IEnumerable<Note> LoadAll()
    {
        if (!File.Exists(_filePath))
            return Enumerable.Empty<Note>();

        var json = File.ReadAllText(_filePath);
        return JsonSerializer.Deserialize<List<Note>>(json, _jsonOptions) ?? new List<Note>();
    }

    public void Save(Note note)
    {
        var notes = LoadAll().ToDictionary(n => n.Id);
        note.UpdatedAt = DateTime.UtcNow;
        notes[note.Id] = note;
        File.WriteAllText(_filePath, JsonSerializer.Serialize(notes.Values, _jsonOptions));
    }

    public void Delete(Guid id)
    {
        var notes = LoadAll().Where(n => n.Id != id);
        File.WriteAllText(_filePath, JsonSerializer.Serialize(notes, _jsonOptions));
    }
}