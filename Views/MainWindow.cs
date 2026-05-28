using Gtk;
using Final.Models;
using Final.Services;

namespace Final.Views;

// GTK main window — also a Concrete Observer so it refreshes automatically on note changes
public class MainWindow : Window, INoteObserver
{
    private readonly NoteService _noteService;
    private readonly ListBox _noteList;
    private readonly Entry _titleEntry;
    private readonly Entry _contentEntry;

    public MainWindow(NoteService noteService) : base("Notes App")
    {
        _noteService = noteService;
        _noteService.Subscribe(this);  // register as observer

        SetDefaultSize(500, 600);
        DeleteEvent += (_, _) => Application.Quit();

        // --- layout ---
        var root = new Box(Orientation.Vertical, 8) { BorderWidth = 12 };

        // Note list (scrollable)
        _noteList = new ListBox { SelectionMode = SelectionMode.Single };
        var scrolled = new ScrolledWindow { ShadowType = ShadowType.In };
        scrolled.Add(_noteList);

        // Input fields
        _titleEntry   = new Entry { PlaceholderText = "Title" };
        _contentEntry = new Entry { PlaceholderText = "Content" };

        var addButton = new Button("Add Note");
        addButton.Clicked += OnAddClicked;

        var deleteButton = new Button("Delete Selected");
        deleteButton.Clicked += OnDeleteClicked;

        var buttonRow = new Box(Orientation.Horizontal, 8);
        buttonRow.PackStart(addButton,    true, true, 0);
        buttonRow.PackStart(deleteButton, true, true, 0);

        root.PackStart(scrolled,        true,  true,  0);
        root.PackStart(_titleEntry,     false, false, 0);
        root.PackStart(_contentEntry,   false, false, 0);
        root.PackStart(buttonRow,       false, false, 0);

        Add(root);
        ShowAll();

        Refresh();
    }

    // INoteObserver — called by NoteService after every mutation
    public void OnNotesChanged()
    {
        // GTK widgets must be updated on the main thread
        Application.Invoke((_, _) => Refresh());
    }

    private void Refresh()
    {
        foreach (Widget child in _noteList.Children)
            _noteList.Remove(child);

        foreach (Note note in _noteService.GetAllNotes())
        {
            var row = new ListBoxRow { Name = note.Id.ToString() };
            row.Add(new Label($"{note.Title}  —  {note.Content}") { Xalign = 0, Margin = 6 });
            _noteList.Insert(row, -1);
        }

        _noteList.ShowAll();
    }

    private void OnAddClicked(object? sender, EventArgs e)
    {
        var title   = _titleEntry.Text.Trim();
        var content = _contentEntry.Text.Trim();
        if (string.IsNullOrEmpty(title)) return;

        _noteService.CreateNote(title, content);
        _titleEntry.Text   = string.Empty;
        _contentEntry.Text = string.Empty;
    }

    private void OnDeleteClicked(object? sender, EventArgs e)
    {
        var row = _noteList.SelectedRow;
        if (row == null) return;

        if (Guid.TryParse(row.Name, out Guid id))
            _noteService.DeleteNote(id);
    }
}