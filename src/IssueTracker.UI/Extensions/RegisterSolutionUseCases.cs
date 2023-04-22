using IssueTracker.UseCases.Solution;
using IssueTracker.UseCases.Solution.Interfaces;

namespace IssueTracker.UI.Extensions;

public static partial class IServiceCollectionExtensions
{

	public static IServiceCollection RegisterSolutionUseCases(this IServiceCollection services)
	{

		services.AddTransient<IArchiveSolutionUseCase, ArchiveSolutionUseCase>();
		services.AddTransient<ICreateNewSolutionUseCase, CreateNewSolutionUseCase>();
		services.AddTransient<IEditSolutionUseCase, EditSolutionUseCase>();
		services.AddTransient<IViewSolutionsUseCase, ViewSolutionsUseCase>();
		services.AddTransient<IViewSolutionsByIssueIdUseCase, ViewSolutionsByIssueIdUseCase>();
		services.AddTransient<IViewSolutionsByUserIdUseCase, ViewSolutionsByUserIdUseCase>();
		services.AddTransient<IViewSolutionsUseCase, ViewSolutionsUseCase>();

		return services;

	}

}
