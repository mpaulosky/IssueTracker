namespace IssueTracker.UseCases.Users.Interfaces;

public interface IViewUserByIdUseCase
{
	Task<UserModel> ExecuteAsync(string id);
}