using IssueTracker.UseCases.Comment;
using IssueTracker.UseCases.Comment.Interfaces;

namespace IssueTracker.UI.Extensions;

public static partial class ServiceCollectionExtensions
{

	public static IServiceCollection RegisterCommentUseCases(this IServiceCollection services)
	{

		services.AddTransient<IArchiveCommentUseCase, ArchiveCommentUseCase>();
		services.AddTransient<ICreateCommentUseCase, CreateCommentUseCase>();
		services.AddTransient<IUpdateCommentUseCase, UpdateCommentUseCase>();
		services.AddTransient<IViewCommentsUseCase, ViewCommentsUseCase>();
		services.AddTransient<IViewCommentUseCase, ViewCommentUseCase>();

		return services;

	}

}
