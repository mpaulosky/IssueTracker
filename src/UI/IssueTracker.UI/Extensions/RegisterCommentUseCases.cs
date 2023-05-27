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
		services.AddTransient<IUpVoteCommentUseCase, UpVoteCommentUseCase>();
		services.AddTransient<IViewCommentsUseCase, ViewCommentsUseCase>();
		services.AddTransient<IViewCommentUseCase, ViewCommentUseCase>();
		services.AddTransient<IViewCommentsBySourceUseCase, ViewCommentsBySourceUseCase>();
		services.AddTransient<IViewCommentsByUserUseCase, ViewCommentsByUserUseCase>();

		return services;

	}

}
