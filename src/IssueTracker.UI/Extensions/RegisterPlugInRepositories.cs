
namespace IssueTracker.UI.Extensions;

public static partial class IServiceCollectionExtensions
{

	public static IServiceCollection RegisterPlugInRepositories(this IServiceCollection services)
	{

		services.AddTransient<ICategoryRepository, CategoryRepository>();
		services.AddTransient<ICommentRepository, CommentRepository>();
		services.AddTransient<IIssueRepository, IssueRepository>();
		services.AddTransient<IStatusRepository, StatusRepository>();
		services.AddTransient<IUserRepository, UserRepository>();

		return services;

	}

}
