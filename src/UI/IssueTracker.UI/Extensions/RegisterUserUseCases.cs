using IssueTracker.UseCases.Users;
using IssueTracker.UseCases.Users.Interfaces;

namespace IssueTracker.UI.Extensions;

public static partial class ServiceCollectionExtensions
{
	public static IServiceCollection RegisterUserUseCases(this IServiceCollection services)
	{

		services.AddTransient<IArchiveUserUseCase, ArchiveUserUseCase>();
		services.AddTransient<ICreateUserUseCase, CreateUserUseCase>();
		services.AddTransient<IUpdateUserUseCase, UpdateUserUseCase>();
		services.AddTransient<IViewUsersUseCase, ViewUsersUseCase>();
		services.AddTransient<IViewUserUseCase, ViewUserUseCase>();
		services.AddTransient<IViewUserFromAuthenticationUseCase, ViewUserFromAuthenticationUseCase>();

		return services;

	}

}
