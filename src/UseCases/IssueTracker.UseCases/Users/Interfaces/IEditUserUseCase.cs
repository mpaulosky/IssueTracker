namespace IssueTracker.UseCases.Users.Interfaces;

public interface IEditUserUseCase
{
	Task ExecuteAsync(UserModel user);
}