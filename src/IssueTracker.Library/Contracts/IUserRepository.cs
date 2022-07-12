namespace IssueTracker.Library.Contracts;

public interface IUserRepository
{
	Task<UserModel> GetUser(string id);

	Task<IEnumerable<UserModel>> GetUsers();

	Task CreateUser(UserModel user);

	Task UpdateUser(string id, UserModel user);

	Task<UserModel> GetUserFromAuthentication(string id);
}