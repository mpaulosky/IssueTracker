namespace IssueTracker.Library.Contracts;

public interface IUserRepository
{
	Task<User> GetUser(string id);
	
	Task<IEnumerable<User>> GetUsers();
	
	Task CreateUser(User user);

	Task UpdateUser(string id, User user);

	Task<User> GetUserFromAuthentication(string id);
}