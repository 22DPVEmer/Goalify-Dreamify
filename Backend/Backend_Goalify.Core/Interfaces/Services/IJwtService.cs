using Backend_Goalify.Core.Entities;
public interface IJwtService
{
    string GenerateToken(ApplicationUser user);
} 