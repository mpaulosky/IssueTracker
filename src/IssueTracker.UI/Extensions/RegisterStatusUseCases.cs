using IssueTracker.UseCases.Status;
using IssueTracker.UseCases.Status.Interfaces;

namespace IssueTracker.UI.Extensions;

public static partial class IServiceCollectionExtensions
{

	public static IServiceCollection RegisterStatusUseCases(this IServiceCollection services)
	{

		services.AddTransient<IArchiveStatusUseCase, ArchiveStatusUseCase>();
		services.AddTransient<ICreateNewStatusUseCase, CreateNewStatusUseCase>();
		services.AddTransient<IEditStatusUseCase, EditStatusUseCase>();
		services.AddTransient<IViewStatusesUseCase, ViewStatusesUseCase>();
		services.AddTransient<IViewStatusByIdUseCase, ViewStatusByIdUseCase>();

		return services;

	}

}
