namespace IssueTracker.Library.Services;

/// <summary>
/// UserService class
/// </summary>
public class UserService : IUserService
{
	private readonly IUserRepository _repo;

	/// <summary>
	/// UserService constructor
	/// </summary>
	/// <param name="repository">IUserRepository</param>
	public UserService(IUserRepository repository)
	{
		_repo = repository;
	}

	/// <summary>
	/// CreateUser method
	/// </summary>
	/// <param name="user">UserModel</param>
	/// <returns>Task</returns>
	/// <exception cref="ArgumentNullException"></exception>
	public Task CreateUser(UserModel user)
	{
		if (user == null)
		{
			throw new ArgumentNullException(nameof(user));
		}

		return _repo.CreateUser(user);
	}

	/// <summary>
	/// GetUser method
	/// </summary>
	/// <param name="id">string</param>
	/// <returns>Task of UserModel</returns>
	/// <exception cref="ArgumentException"></exception>
	public async Task<UserModel> GetUser(string id)
	{
		if (string.IsNullOrWhiteSpace(id))
		{
			throw new ArgumentException("Value cannot be null or whitespace.", nameof(id));
		}

		var results = await _repo.GetUser(id).ConfigureAwait(true);

		return results;
	}

	/// <summary>
	/// GetUsers method
	/// </summary>
	/// <returns>Task if List UserModel</returns>
	public async Task<List<UserModel>> GetUsers()
	{
		var results = await _repo.GetUsers().ConfigureAwait(true);

		return results.ToList();
	}

	/// <summary>
	/// GetUserFromAuthentication method
	/// </summary>
	/// <param name="objectId">string</param>
	/// <returns>Task of UserModel</returns>
	/// <exception cref="ArgumentException"></exception>
	public async Task<UserModel> GetUserFromAuthentication(string objectId)
	{
		if (string.IsNullOrWhiteSpace(objectId))
		{
			throw new ArgumentException("Value cannot be null or whitespace.", nameof(objectId));
		}

		var results = await _repo.GetUserFromAuthentication(objectId).ConfigureAwait(true);

		return results;
	}

/// <summary>
/// UpdateUser method
/// </summary>
/// <param name="user">UserModel</param>
/// <returns>Task</returns>
/// <exception cref="ArgumentNullException"></exception>
	public Task UpdateUser(UserModel user)
	{
		if (user == null)
		{
			throw new ArgumentNullException(nameof(user));
		}

		return _repo.UpdateUser(user.Id, user);
	}
}