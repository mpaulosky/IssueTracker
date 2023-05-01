using IssueTracker.UseCases.Issue;
using IssueTracker.UseCases.Issue.Interfaces;

namespace IssueTracker.UI.Extensions;

public static partial class ServiceCollectionExtensions
{

	public static IServiceCollection RegisterIssueUseCases(this IServiceCollection services)
	{

		services.AddTransient<IArchiveIssueUseCase, ArchiveIssueUseCase>();
		services.AddTransient<ICreateIssueUseCase, CreateIssueUseCase>();
		services.AddTransient<IUpdateIssueUseCase, UpdateIssueUseCase>();
		services.AddTransient<IViewIssuesUseCase, ViewIssuesUseCase>();
		services.AddTransient<IViewIssueUseCase, ViewIssueUseCase>();
		services.AddTransient<IViewIssuesApprovedUseCase, ViewIssuesApprovedUseCase>();
		services.AddTransient<IViewIssuesByUserUseCase, ViewIssuesByUserUseCase>();
		services.AddTransient<IViewIssuesUseCase, ViewIssuesUseCase>();
		services.AddTransient<IViewIssuesWaitingForApprovalUseCase, ViewIssuesWaitingForApprovalUseCase>();

		return services;

	}

}
