namespace Halter.Application.Interfaces;

public interface IHashPasswordService
{
    Task<string> HashAsync(string password);
    Task<bool> VerifyAsync(string password, string storedHash);
}
