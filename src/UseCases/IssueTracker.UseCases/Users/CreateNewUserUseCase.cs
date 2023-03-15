namespace IssueTracker.UseCases.Users;

public class CreateNewUserUseCase : ICreateNewUserUseCase
{

	private readonly IUserRepository _userRepository;

	public CreateNewUserUseCase(IUserRepository userRepository)
	{

		_userRepository = userRepository;

	}

	public async Task ExecuteAsync(UserModel user)
	{

		if (user == null) return;

		await _userRepository.CreateNewUserAsync(user);

	}

}
