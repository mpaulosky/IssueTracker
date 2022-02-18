using IssueTrackerLibrary.Contracts;

namespace IssueTrackerLibrary.DataAccess;

public class MongoUserService : IUserService
{
	private readonly IUserRepository _repo;

	public MongoUserService(IUserRepository repository)
	{
		_repo = repository;
	}

	public async Task<User> GetUser(string id)
	{
		if (string.IsNullOrWhiteSpace(id))
		{
			throw new ArgumentException("Value cannot be null or whitespace.", nameof(id));
		}

		var results = await _repo.Get(id);
		return results;
	}

	public async Task<List<User>> GetUsers()
	{
		var results = await _repo.Get();
		return results.ToList();
	}

	public async Task<User> GetUserFromAuthentication(string objectId)
	{
		if (string.IsNullOrWhiteSpace(objectId))
		{
			throw new ArgumentException("Value cannot be null or whitespace.", nameof(objectId));
		}

		var results = await _repo.GetUserFromAuthentication(objectId);
		return results;
	}

	public Task CreateUser(User user)
	{
		if (user == null)
		{
			throw new ArgumentNullException(nameof(user));
		}

		return _repo.Create(user);
	}

	public Task UpdateUser(User user)
	{
		if (user == null)
		{
			throw new ArgumentNullException(nameof(user));
		}

		return _repo.Update(user.Id, user);
	}
}