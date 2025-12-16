using ReThreaded.Domain.Common;
using ReThreaded.Domain.Enums;
using ReThreaded.Domain.Exceptions;

namespace ReThreaded.Domain.Entities;

public class User : BaseEntity
{
    // Properties - Private setters!
    public string Email { get; private set; }
    public string PasswordHash { get; private set; }
    public string FirstName { get; private set; }
    public string LastName { get; private set; }
    public UserRole Role { get; private set; }
    public string? ProfilePictureUrl { get; private set; }
    public bool IsActive { get; private set; }

    // Navigation properties
    public DesignerProfile? DesignerProfile { get; private set; }
    public Cart? Cart { get; private set; }
    public ICollection<Order> Orders { get; private set; }

    // Private constructor - forces use of factory method
    private User()
    {
        Orders = new List<Order>();
    }

    // Factory Method - THE ONLY way to create a User
    public static User Create(
        string email,
        string passwordHash,
        string firstName,
        string lastName,
        UserRole role)
    {
        // Validation
        if (string.IsNullOrWhiteSpace(email))
            throw new DomainException("Email is required");

        if (!IsValidEmail(email))
            throw new DomainException("Invalid email format");

        if (string.IsNullOrWhiteSpace(passwordHash))
            throw new DomainException("Password hash is required");

        if (string.IsNullOrWhiteSpace(firstName))
            throw new DomainException("First name is required");

        if (string.IsNullOrWhiteSpace(lastName))
            throw new DomainException("Last name is required");

        var user = new User
        {
            Email = email.ToLowerInvariant().Trim(),
            PasswordHash = passwordHash,
            FirstName = firstName.Trim(),
            LastName = lastName.Trim(),
            Role = role,
            IsActive = true
        };

        return user;
    }

    // Business Methods
    public void UpdateProfile(string firstName, string lastName, string? profilePictureUrl)
    {
        if (string.IsNullOrWhiteSpace(firstName))
            throw new DomainException("First name is required");

        if (string.IsNullOrWhiteSpace(lastName))
            throw new DomainException("Last name is required");

        FirstName = firstName.Trim();
        LastName = lastName.Trim();
        ProfilePictureUrl = profilePictureUrl;
        SetUpdatedAt();
    }

    public void UpdatePassword(string newPasswordHash)
    {
        if (string.IsNullOrWhiteSpace(newPasswordHash))
            throw new DomainException("Password hash is required");

        PasswordHash = newPasswordHash;
        SetUpdatedAt();
    }

    public void Deactivate()
    {
        IsActive = false;
        SetUpdatedAt();
    }

    public void Activate()
    {
        IsActive = true;
        SetUpdatedAt();
    }

    // Helper method
    private static bool IsValidEmail(string email)
    {
        try
        {
            var addr = new System.Net.Mail.MailAddress(email);
            return addr.Address == email;
        }
        catch
        {
            return false;
        }
    }

    // Computed property (no setter!)
    public string FullName => $"{FirstName} {LastName}";
}