namespace IssueTracker.UseCases.Users.Interfaces;

public interface IDeleteUserUseCase
{
	Task ExecuteAsync(UserModel user);
}