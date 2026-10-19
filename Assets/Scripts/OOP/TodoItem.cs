using System;

/// Элемент списка
public class TodoItem : AppEntity, IComparable<TodoItem>
{
    private string _title;
    private string _ownerLogin;
    private bool _isDone;
    private int _priority;

    public TodoItem(string title, string ownerLogin, int priority = 0)
        : base(BuildId(ownerLogin, title))
    {
        Title = title;
        OwnerLogin = ownerLogin;
        Priority = priority;
        _isDone = false;
    }

    public string Title
    {
        get => _title;
        set
        {
            if (string.IsNullOrWhiteSpace(value))
                throw new ArgumentException("Название задачи пустое");
            _title = value.Trim();
        }
    }

    public string OwnerLogin
    {
        get => _ownerLogin;
        set => _ownerLogin = value ?? string.Empty;
    }

    public bool IsDone
    {
        get => _isDone;
        set => _isDone = value;
    }

    public int Priority
    {
        get => _priority;
        set => _priority = Math.Max(0, value);
    }

    public static string BuildId(string owner, string title) => $"{owner}|{title}";

    public void MarkDone() => _isDone = true;

    public void MarkOpen() => _isDone = false;

    public override string Describe() =>
        _isDone ? $"[x] {_title}" : $"[ ] {_title} (p{_priority})";

    public override bool IsValid() =>
        !string.IsNullOrWhiteSpace(_title) && !string.IsNullOrWhiteSpace(_ownerLogin);

    public int CompareTo(TodoItem other)
    {
        if (ReferenceEquals(other, null)) return 1;
        int byPriority = other._priority.CompareTo(_priority);
        if (byPriority != 0) return byPriority;
        return string.Compare(_title, other._title, StringComparison.OrdinalIgnoreCase);
    }

    public override int CompareTo(AppEntity other)
    {
        if (other is TodoItem todo) return CompareTo(todo);
        return base.CompareTo(other);
    }

    public override string ToString() =>
        $"TodoItem(title={_title}, owner={_ownerLogin}, done={_isDone}, priority={_priority})";
}
