using IssueTracker.UseCases.Issue;
using IssueTracker.UseCases.Issue.Interfaces;

namespace IssueTracker.UI.Extensions;

public static partial class ServiceCollectionExtensions
{

	public static IServiceCollection RegisterIssueUseCases(this IServiceCollection services)
	{

		services.AddTransient<IArchiveIssueUseCase, ArchiveIssueUseCase>();
		services.AddTransient<ICreateNewIssueUseCase, CreateNewIssueUseCase>();
		services.AddTransient<IEditIssueUseCase, EditIssueUseCase>();
		services.AddTransient<IViewIssuesUseCase, ViewIssuesUseCase>();
		services.AddTransient<IViewIssueByIdUseCase, ViewIssueByIdUseCase>();
		services.AddTransient<IViewIssuesApprovedUseCase, ViewIssuesApprovedUseCase>();
		services.AddTransient<IViewIssuesByUserIdUseCase, ViewIssuesByUserIdUseCase>();
		services.AddTransient<IViewIssuesUseCase, ViewIssuesUseCase>();
		services.AddTransient<IViewIssuesWaitingForApprovalUseCase, ViewIssuesWaitingForApprovalUseCase>();

		return services;

	}

}
