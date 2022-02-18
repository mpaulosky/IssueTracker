namespace IssueTrackerLibrary.Contracts;

public interface IUserRepository : IBaseRepository<User>
{
	Task<User> GetUserFromAuthentication(string id);
}