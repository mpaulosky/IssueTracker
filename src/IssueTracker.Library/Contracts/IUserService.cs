namespace IssueTracker.Library.Contracts;

public interface IUserService
{
	Task CreateUser(UserModel user);

	Task<UserModel> GetUser(string id);

	Task<UserModel> GetUserFromAuthentication(string objectId);

	Task<List<UserModel>> GetUsers();

	Task UpdateUser(UserModel user);
}