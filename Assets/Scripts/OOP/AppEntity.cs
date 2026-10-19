using System;

/// Абстрактная сущность
public abstract class AppEntity : IComparable<AppEntity>, IEquatable<AppEntity>
{
    private string _id;
    private DateTime _createdAt;

    protected AppEntity(string id)
    {
        _id = id ?? throw new ArgumentNullException(nameof(id));
        _createdAt = DateTime.Now;
    }

    public string Id
    {
        get => _id;
        set
        {
            if (string.IsNullOrWhiteSpace(value))
                throw new ArgumentException("Id не может быть пустым");
            _id = value;
        }
    }

    public DateTime CreatedAt
    {
        get => _createdAt;
        set => _createdAt = value;
    }

    public abstract string Describe();

    public abstract bool IsValid();

    public virtual int CompareTo(AppEntity other)
    {
        if (ReferenceEquals(other, null)) return 1;
        return string.Compare(_id, other._id, StringComparison.Ordinal);
    }

    public virtual bool Equals(AppEntity other)
    {
        if (ReferenceEquals(other, null)) return false;
        if (ReferenceEquals(this, other)) return true;
        return GetType() == other.GetType() && _id == other._id;
    }

    public override bool Equals(object obj) => Equals(obj as AppEntity);

    public override int GetHashCode() => (_id != null ? _id.GetHashCode() : 0);

    public override string ToString() => $"[{GetType().Name}] Id={_id}, Created={_createdAt:dd.MM.yyyy HH:mm:ss}";

    public static bool operator ==(AppEntity left, AppEntity right)
    {
        if (ReferenceEquals(left, right)) return true;
        if (ReferenceEquals(left, null) || ReferenceEquals(right, null)) return false;
        return left.Equals(right);
    }

    public static bool operator !=(AppEntity left, AppEntity right) => !(left == right);

    public static bool operator <(AppEntity left, AppEntity right) => left.CompareTo(right) < 0;
    public static bool operator >(AppEntity left, AppEntity right) => left.CompareTo(right) > 0;
}
