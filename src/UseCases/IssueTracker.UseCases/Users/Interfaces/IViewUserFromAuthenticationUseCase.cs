namespace IssueTracker.UseCases.Users.Interfaces;

public interface IViewUserFromAuthenticationUseCase
{
	Task<UserModel> Execute(string userObjectIdentifierId);
}