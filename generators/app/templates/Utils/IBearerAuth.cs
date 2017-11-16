namespace <%= ProjectName %>.Utils
{
    public interface IBearerAuth
    {
        string GenerateToken(string id, string name);
    }
}