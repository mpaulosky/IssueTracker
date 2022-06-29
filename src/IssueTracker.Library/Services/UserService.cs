namespace IssueTracker.Library.Services;

public class UserService : IUserService
{
	private readonly IUserRepository _repo;

	public UserService(IUserRepository repository)
	{
		_repo = repository;
	}

	public Task CreateUser(User user)
	{
		if (user == null)
		{
			throw new ArgumentNullException(nameof(user));
		}

		return _repo.CreateUser(user);
	}

	public async Task<User> GetUser(string id)
	{
		if (string.IsNullOrWhiteSpace(id))
		{
			throw new ArgumentException("Value cannot be null or whitespace.", nameof(id));
		}

		var results = await _repo.GetUser(id);

		return results;
	}

	public async Task<List<User>> GetUsers()
	{
		var results = await _repo.GetUsers();

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


	public Task UpdateUser(User user)
	{
		if (user == null)
		{
			throw new ArgumentNullException(nameof(user));
		}

		return _repo.UpdateUser(user.Id, user);
	}
}