using IssueTracker.UseCases.Comment;
using IssueTracker.UseCases.Comment.Interfaces;

namespace IssueTracker.UI.Extensions;

public static partial class ServiceCollectionExtensions
{

	public static IServiceCollection RegisterCommentUseCases(this IServiceCollection services)
	{

		services.AddTransient<IArchiveCommentUseCase, ArchiveCommentUseCase>();
		services.AddTransient<ICreateNewCommentUseCase, CreateNewCommentUseCase>();
		services.AddTransient<IEditCommentUseCase, EditCommentUseCase>();
		services.AddTransient<IViewCommentsUseCase, ViewCommentsUseCase>();
		services.AddTransient<IViewCommentByIdUseCase, ViewCommentByIdUseCase>();

		return services;

	}

}
