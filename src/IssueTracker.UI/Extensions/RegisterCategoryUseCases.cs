using IssueTracker.UseCases.Category;
using IssueTracker.UseCases.Category.Interfaces;

namespace IssueTracker.UI.Extensions;

public static partial class ServiceCollectionExtensions
{

	public static IServiceCollection RegisterCategoryUseCases(this IServiceCollection services)
	{

		services.AddTransient<IArchiveCategoryUseCase, ArchiveCategoryUseCase>();
		services.AddTransient<ICreateCategoryUseCase, CreateCategoryUseCase>();
		services.AddTransient<IUpdateCategoryUseCase, UpdateCategoryUseCase>();
		services.AddTransient<IViewCategoriesUseCase, ViewCategoriesUseCase>();
		services.AddTransient<IViewCategoryUseCase, ViewCategoryUseCase>();

		return services;

	}

}
