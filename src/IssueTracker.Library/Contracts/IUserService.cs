namespace IssueTracker.Library.Contracts;

public interface IUserService
{
	Task CreateUser(User user);
	
	Task<User> GetUser(string id);
	
	Task<User> GetUserFromAuthentication(string objectId);
	
	Task<List<User>> GetUsers();
	
	Task UpdateUser(User user);
}