//namespace IssueTracker.UI.Tests.Integration;

//public class UIFactory : WebApplicationFactory<IAppMarker>, IAsyncLifetime
//{

//	public const string AppUrl = "https://localhost:7780";

//	public const string DbName = "db";

//	private static readonly string? _localDirectory = Directory.GetCurrentDirectory();

//	private static readonly string? _dockerComposeFile =
//			Path.Combine(_localDirectory, (TemplateString)@"..\..\..\docker-compose.yml");

//	protected override void ConfigureWebHost(IWebHostBuilder builder)
//	{

//		var descriptorDatabaseSettings = builder.ConfigureServices(services =>
//		{

//			var descriptorMongoDbContext = services.FirstOrDefault(d => d.ServiceType == typeof(MongoDbContext));
//			services.Remove(item: descriptorMongoDbContext!);

//			var dbSettings = new DatabaseSettings("mongodb://course:whatever@localhost:27017/?authSource=admin", DbName);

//			services.AddSingleton<IDatabaseSettings>(dbSettings);
//			services.AddSingleton<IMongoDbContextFactory, TestContextFactory>();
//			services.AddSingleton<ICategoryService, CategoryService>();
//			services.AddSingleton<ICommentService, CommentService>();
//			services.AddSingleton<IStatusService, StatusService>();
//			services.AddSingleton<IIssueService, IssueService>();
//			services.AddSingleton<IUserService, UserService>();

//			services.AddSingleton<ICategoryRepository, CategoryRepository>();
//			services.AddSingleton<ICommentRepository, CommentRepository>();
//			services.AddSingleton<IStatusRepository, StatusRepository>();
//			services.AddSingleton<IIssueRepository, IssueRepository>();
//			services.AddSingleton<IUserRepository, UserRepository>();

//		});

//	}

//	private readonly ICompositeService _dockerService = new Builder()
//			.UseContainer()
//			.UseCompose()
//			.FromFile(_dockerComposeFile)
//			.RemoveOrphans()
//			.WaitForHttp("test-app", AppUrl)
//			.Build();

//	public async Task InitializeAsync()
//	{

//		_dockerService.Start();

//	}

//	async Task IAsyncLifetime.DisposeAsync()
//	{
//		await Task.Delay(0);
//		_dockerService.Dispose();

//	}

//}
