namespace UnrealEstate.Models.Repositories
{
    public interface IUserManager
    {
        bool VerifyLogin(string email, string password);
    }
}