using IssueTracker.UseCases.PlugInRepositoryInterfaces;

using ICategoryRepository = IssueTracker.UseCases.PlugInRepositoryInterfaces.ICategoryRepository;
using ICommentRepository = IssueTracker.UseCases.PlugInRepositoryInterfaces.ICommentRepository;
using IIssueRepository = IssueTracker.UseCases.PlugInRepositoryInterfaces.IIssueRepository;
using IStatusRepository = IssueTracker.UseCases.PlugInRepositoryInterfaces.IStatusRepository;
using IUserRepository = IssueTracker.UseCases.PlugInRepositoryInterfaces.IUserRepository;

namespace IssueTracker.UI.Extensions;

public static partial class IServiceCollectionExtensions
{

	public static IServiceCollection RegisterPlugInRepositories(this IServiceCollection services)
	{

		services.AddTransient<ICategoryRepository, CategoryMongoRepository>();
		services.AddTransient<ICommentRepository, CommentMongoRepository>();
		services.AddTransient<IIssueRepository, IssueMongoRepository>();
		services.AddTransient<ISolutionRepository, SolutionMongoRepository>();
		services.AddTransient<IStatusRepository, StatusMongoRepository>();
		services.AddTransient<IUserRepository, UserMongoRepository>();

		return services;

	}

}
