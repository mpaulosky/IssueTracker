namespace IssueTracker.UseCases.PlugInRepositories;

public interface IUserRepository
{
	Task CreateNewUserAsync(UserModel user);
	Task UpdateUserAsync(UserModel user);
	Task<IEnumerable<UserModel>> ViewUsersAsync();
	Task<UserModel> ViewUserByAuthenticationIdAsync(string userObjectIdentifierId);
	Task<UserModel> ViewUserByIdAsync(string id);
}