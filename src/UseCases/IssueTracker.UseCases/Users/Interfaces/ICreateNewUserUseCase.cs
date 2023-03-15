namespace IssueTracker.UseCases.Users.Interfaces;

public interface ICreateNewUserUseCase
{
	Task ExecuteAsync(UserModel user);
}