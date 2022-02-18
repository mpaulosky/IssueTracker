namespace IssueTrackerLibrary.Contracts;

public interface IUserService
{
	Task<List<User>> GetUsers();
	Task<User> GetUser(string id);
	Task<User> GetUserFromAuthentication(string objectId);
	Task CreateUser(User user);
	Task UpdateUser(User user);
}