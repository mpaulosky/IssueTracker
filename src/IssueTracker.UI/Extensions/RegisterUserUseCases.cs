using IssueTracker.UseCases.Users;
using IssueTracker.UseCases.Users.Interfaces;

namespace IssueTracker.UI.Extensions;

public static partial class ServiceCollectionExtensions
{
	public static IServiceCollection RegisterUserUseCases(this IServiceCollection services)
	{

		services.AddTransient<IArchiveUserUseCase, ArchiveUserUseCase>();
		services.AddTransient<ICreateNewUserUseCase, CreateNewUserUseCase>();
		services.AddTransient<IEditUserUseCase, EditUserUseCase>();
		services.AddTransient<IViewUsersUseCase, ViewUsersUseCase>();
		services.AddTransient<IViewUserByIdUseCase, ViewUserByIdUseCase>();
		services.AddTransient<IViewUserFromAuthenticationUseCase, ViewUserFromAuthenticationUseCase>();

		return services;

	}

}
