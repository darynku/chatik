namespace DarynChat.API.JWT;

public interface IJwtService
{
    string Generate(UserEntity user);
}