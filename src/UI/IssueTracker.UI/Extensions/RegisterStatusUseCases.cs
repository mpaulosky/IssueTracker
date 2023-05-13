using IssueTracker.UseCases.Status;
using IssueTracker.UseCases.Status.Interfaces;

namespace IssueTracker.UI.Extensions;

public static partial class ServiceCollectionExtensions
{

	public static IServiceCollection RegisterStatusUseCases(this IServiceCollection services)
	{

		services.AddTransient<IArchiveStatusUseCase, ArchiveStatusUseCase>();
		services.AddTransient<ICreateStatusUseCase, CreateStatusUseCase>();
		services.AddTransient<IUpdateStatusUseCase, UpdateStatusUseCase>();
		services.AddTransient<IViewStatusesUseCase, ViewStatusesUseCase>();
		services.AddTransient<IViewStatusUseCase, ViewStatusUseCase>();

		return services;

	}

}
