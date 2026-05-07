# CSC360 - Notes App

A GTK# notes application built with C# and .NET 8, a lightweight notes application built with C# and GTK#.

## Description
GtkNotes allows users to create, edit, and delete simple text notes
through a native GTK interface.

## Planned Structure
- **Models** — Note data model
- **Views** — GTK windows and widgets
- **Controllers** — Mediates input between Views and Services
- **Services** — Note persistence (file-based storage planned)

## Future Plans
- Save/load notes from local JSON or SQLite
- Search and filter functionality
- Note tagging or categorization


## Design Pattern: Strategy

This project uses the **Strategy pattern** to handle note storage.

### Problem It Solves

The notes app needs to persist data, but the exact storage mechanism shouldn't be hardwired into the
application logic. During development, in-memory storage is fast and has no side effects. In
production, notes need to survive between sessions, so they must be saved to disk.

Without the Strategy pattern, switching backends would require modifying `NoteService` directly —
violating the open/closed principle and making the code harder to test.

### How It Works

```
IStorageStrategy          (interface — defines the contract)
├── InMemoryStorage       (concrete strategy — stores notes in a Dictionary)
└── JsonFileStorage       (concrete strategy — persists notes to notes.json)

NoteService               (context — delegates all storage calls to the active strategy)
```

- **`IStorageStrategy`** declares `LoadAll()`, `Save()`, and `Delete()`.
- **`InMemoryStorage`** keeps notes in a `Dictionary<Guid, Note>` — no disk I/O.
- **`JsonFileStorage`** reads and writes a JSON file, providing persistence.
- **`NoteService`** holds a reference to whichever strategy is active and can swap it at runtime
  via `SetStorage()`.

### Swapping Backends

```csharp
// Development / testing
var service = new NoteService(new InMemoryStorage());

// Production
var service = new NoteService(new JsonFileStorage("notes.json"));

// Runtime swap
service.SetStorage(new InMemoryStorage());
```

`NoteService` never needs to change when a new storage backend is added — only a new class
implementing `IStorageStrategy` is required.