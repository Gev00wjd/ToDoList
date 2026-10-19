using System;

/// Пользователь 
public class User : AppEntity
{
    private string _login;
    private string _passwordHash;

    public User(string login, string password) : base(login)
    {
        Login = login;
        PasswordHash = HashPassword(password);
    }

    public string Login
    {
        get => _login;
        set
        {
            if (string.IsNullOrWhiteSpace(value))
                throw new ArgumentException("Логин не может быть пустым");
            _login = value;
            Id = value;
        }
    }

    public string PasswordHash
    {
        get => _passwordHash;
        set => _passwordHash = value ?? string.Empty;
    }

    public static string HashPassword(string password)
    {
        if (password == null) return string.Empty;
        // Учебный хэш: не для продакшена, демонстрирует static-метод класса
        return Convert.ToBase64String(System.Text.Encoding.UTF8.GetBytes(password));
    }

    public static bool PasswordsMatch(string raw, string storedHash)
    {
        return HashPassword(raw) == storedHash || raw == storedHash;
    }

    public bool CheckPassword(string rawPassword) => PasswordsMatch(rawPassword, _passwordHash);

    public override string Describe() => $"Пользователь {_login}";

    public override bool IsValid() =>
        !string.IsNullOrWhiteSpace(_login) &&
        _login.Length > 8 &&
        !string.IsNullOrEmpty(_passwordHash);

    public override string ToString() => $"User(login={_login}, created={CreatedAt:dd.MM.yyyy HH:mm:ss})";
}
