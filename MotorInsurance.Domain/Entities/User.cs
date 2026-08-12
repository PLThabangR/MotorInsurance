


namespace MotorInsurance.Domain.Entities;



public class User
{       //Only  user class can modify its own data, so the setters are private to enforce encapsulation and maintain integrity of the User entity.
    public Guid Id { get; private set; }    
    public string Email { get;private set; }= string.Empty; //Email should be unique and immutable, and the setter is private to prevent direct modification.
    public string Name  { get;private  set; } = string.Empty; //Name can be updated, but the setter is private to enforce encapsulation and maintain integrity of the User entity.
    public string passwordHash { get; private set; } = string.Empty; //Password hash should be stored securely, and the setter is private to prevent direct modification.
    public string Role { get; private set; } = string.Empty; //Role can be used for authorization purposes, and the setter is private to prevent direct modification.


    public DateTime CreatedAt { get;private set; }
    public bool IsActive { get; private set; } //Indicates whether the user is active or not, and the setter is private to prevent direct modification.

    // Additional properties and methods can be added here as needed
    //Entity Framework requires a parameterless constructor for loading database
    /// <summary>
    /// /Force using the public constructor with parameters to ensure proper initialization of the User entity.
    /// </summary>
    private User()
    {
        //EF Core sets properties directly
    } // Private constructor to prevent direct instantiation

    //Public constructor to enforce business rules and validation

    //Enforce  business rules and validation in the public constructor
    public User(string email, string name, string passwordHash, string role="User")
    {    //Business rule: Email cannot be null or empty       
         ArgumentException.ThrowIfNullOrWhiteSpace(email, nameof(email));
        //Make  email case-insensitive by converting to lower case
        email = email.ToLowerInvariant();
        //Business rule: Name cannot be null or empty
         ArgumentException.ThrowIfNullOrWhiteSpace(name, nameof(name));

         ArgumentException.ThrowIfNullOrWhiteSpace(passwordHash, nameof(passwordHash));


        //Business rule: Email must be in a valid format
        if(!email.Contains("@") || !email.Contains("."))
            throw new ArgumentException("Email is not in a valid format.", nameof(email));

        //Generate ID and set other properties
        Id = Guid.NewGuid();
        Email = email.Trim();
        Name = name.Trim();
        CreatedAt = DateTime.UtcNow; //Ensure timezone consistency by using UTC time for CreatedAt
        IsActive = true; //New users are active by default
        this.passwordHash = passwordHash; //Assuming password hash is already hashed before passing to
    }//End of public constructor


    public void UpdateName(string newName)
    {
        //Business rule: Name cannot be null or empty
        ArgumentException.ThrowIfNullOrWhiteSpace(newName, nameof(newName));
        Name = newName.Trim();
    }

    public void UpadateEmail(string newEmail)
    {
        //Business rule: Email cannot be null or empty
        ArgumentException.ThrowIfNullOrWhiteSpace(newEmail, nameof(newEmail));
        //Business rule: Email must be in a valid format
        if(!newEmail.Contains("@") || !newEmail.Contains("."))
            throw new ArgumentException("Email is not in a valid format.", nameof(newEmail));
        Email = newEmail.Trim().ToLowerInvariant(); //Make email case-insensitive by converting to lower case
    }

    //Deactivate the user account
    public void Deactivate()
    {
        IsActive = false;
    }

    //Activate the user account
    public void Activate()
    {
        IsActive = true;
    }
     //Update role 
    public void UpdateRole(string newRole)
    {
        //Business rule: Role cannot be null or empty
        ArgumentException.ThrowIfNullOrWhiteSpace(newRole, nameof(newRole));
        Role = newRole.Trim();
    }

     public bool VerifyPassword(string passwordToVerify, IPasswordHasher passwordHasher)
    {
        //Business rule: Password to verify cannot be null or empty
        ArgumentException.ThrowIfNullOrWhiteSpace(passwordToVerify, nameof(passwordToVerify));
        ArgumentNullException.ThrowIfNull(passwordHasher, nameof(passwordHasher));
        //Use the provided password hasher to verify the password
        return passwordHasher.VerifyPassword(passwordToVerify, passwordHash);
    }
  

}  //End of User class


