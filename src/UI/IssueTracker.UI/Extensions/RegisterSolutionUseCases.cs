﻿using IssueTracker.UseCases.Solution;
using IssueTracker.UseCases.Solution.Interfaces;

namespace IssueTracker.UI.Extensions;

public static partial class ServiceCollectionExtensions
{

	public static IServiceCollection RegisterSolutionUseCases(this IServiceCollection services)
	{

		services.AddTransient<IArchiveSolutionUseCase, ArchiveSolutionUseCase>();
		services.AddTransient<ICreateSolutionUseCase, CreateSolutionUseCase>();
		services.AddTransient<IUpdateSolutionUseCase, UpdateSolutionUseCase>();
		services.AddTransient<IViewSolutionsUseCase, ViewSolutionsUseCase>();
		services.AddTransient<IViewSolutionsByIssueIdUseCase, ViewSolutionsByIssueIdUseCase>();
		services.AddTransient<IViewSolutionsByUserUseCase, ViewSolutionsByUserUseCase>();
		services.AddTransient<IViewSolutionsUseCase, ViewSolutionsUseCase>();

		return services;

	}

}
