//namespace IssueTracker.Library;

//public class SharedTestContext : IAsyncLifetime
//{

//	public const string AppUrl = "https://localhost:7780";

//	private static readonly string _localDirectory = Directory.GetCurrentDirectory();

//	private static readonly string _dockerComposeFile =
//			Path.Combine(_localDirectory, (TemplateString)@"..\..\..\docker-compose.integration.yml");

//	private IPlaywright _playwright;

//	public IBrowser Browser { get; private set; }

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

//		_playwright = await Playwright.CreateAsync();

//		Browser = await _playwright.Chromium.LaunchAsync(new BrowserTypeLaunchOptions
//		{

//			//Headless = false,
//			SlowMo = 100

//		});

//	}

//	public async Task DisposeAsync()
//	{

//		await Browser!.DisposeAsync();
//		_playwright!.Dispose();
//		_dockerService.Dispose();

//	}

//}
