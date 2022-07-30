namespace ChickenTinder.Client.Data
{
    public interface IUserService
    {
        Task<User> GetRandomUser(List<string> alreadyUsedNames);
    }
}