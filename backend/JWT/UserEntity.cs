namespace DarynChat.API.JWT;

public class UserEntity
{
    private UserEntity(Guid id,string userName, string password)
    {
        Id = id;
        UserName = userName;
        Password = password;
    }
    public Guid Id { get; private set; }
    public string UserName { get; private set; } 
    public string Password { get; private set; }

    public static UserEntity Create(Guid id, string userName, string password)
    {
        return new UserEntity(id, userName, password);
    }
}