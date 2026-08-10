


namespace MotorInsurance.Domain.Entities;



public class User
{       //Only  user class can modify its own data, so the setters are private to enforce encapsulation and maintain integrity of the User entity.
    public Guid Id { get; private set; }

        
    public string Email { get;private set; }
    public string Name  { get;private  set; }
    public DateTime CreatedAt { get;private set; }

    // Additional properties and methods can be added here as needed
    //Entity Framework requires a parameterless constructor for loading database
    /// <summary>
    /// /Force using the public constructor with parameters to ensure proper initialization of the User entity.
    /// </summary>
    private User() { } // Private constructor to prevent direct instantiation

    //Enforce  business rules and validation in the public constructor
    public User(string email, string name)
    {    //Business rule: Email and Name cannot be null or empty
        if(string.IsNullOrWhiteSpace(email))        
            throw new ArgumentException("Email cannot be null or empty.", nameof(email));

        //Business rule: Name cannot be null or empty
        if (string.IsNullOrWhiteSpace(name))
            throw new ArgumentException("Name cannot be null or empty.", nameof(name));

        //Business rule: Email must be in a valid format
        if(!email.Contains("@") || !email.Contains("."))
            throw new ArgumentException("Email is not in a valid format.", nameof(email));

        //Generate ID and set other properties
        Id = Guid.NewGuid();
        Email = email.Trim();
        Name = name.Trim();
        CreatedAt = DateTime.UtcNow; //Ensure timezone consistency by using UTC time for CreatedAt

    }//End of public constructor


    public void UpdateName(string newName)
    {
        //Business rule: Name cannot be null or empty
        if (string.IsNullOrWhiteSpace(newName))
            throw new ArgumentException("Name cannot be null or empty.", nameof(newName));
        
        Name = newName.Trim();
    }

  
  

}  //End of User class


