

namespace Authentification.JWT.Service.Interfaces
{
    public interface IJwtService
    {
        string GenerateToken(int userId);
    }
}
