using IssueTracker.UseCases.Category;
using IssueTracker.UseCases.Category.Interfaces;

namespace IssueTracker.UI.Extensions;

public static partial class IServiceCollectionExtensions
{

	public static IServiceCollection RegisterCategoryUseCases(this IServiceCollection services)
	{

		services.AddTransient<IArchiveCategoryUseCase, ArchiveCategoryUseCase>();
		services.AddTransient<ICreateCategoryUseCase, CreateCategoryUseCase>();
		services.AddTransient<IEditCategoryUseCase, EditCategoryUseCase>();
		services.AddTransient<IViewCategoriesUseCase, ViewCategoriesUseCase>();
		services.AddTransient<IViewCategoryByIdUseCase, ViewCategoryByIdUseCase>();

		return services;

	}

}