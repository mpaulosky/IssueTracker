namespace IssueTracker.UseCases.Users.Interfaces;

public interface IViewUsersUseCase
{
	Task<IEnumerable<UserModel>> ExecuteAsync();
}