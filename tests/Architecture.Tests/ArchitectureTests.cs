namespace IssueTracker.Architecture;

public class ArchitectureTests
{
	/// <summary>
	/// AppHost MUST NOT be referenced by other projects.
	/// AppHost is the entry point and should have zero dependencies from the rest of the system.
	/// </summary>
	[Fact]
	public void AppHost_MustNotBeReferencedByOtherProjects()
	{
		var appHostAssembly = System.Reflection.Assembly.Load("AppHost");

		var result = Types.InAssembly(appHostAssembly)
			.ShouldNot()
			.HaveDependencyOnAny("IssueTracker.CoreBusiness", "IssueTracker.PlugIns", "IssueTracker.Services", "IssueTracker.UI")
			.GetResult();

		result.IsSuccessful.Should().BeTrue();
	}

	/// <summary>
	/// UI should have well-defined dependencies on infrastructure and services.
	/// This ensures the presentation layer properly integrates with the rest of the system.
	/// </summary>
	[Fact]
	public void UI_ShouldNotDependOnAppHost()
	{
		var uiAssembly = System.Reflection.Assembly.Load("IssueTracker.UI");

		var result = Types.InAssembly(uiAssembly)
			.ShouldNot()
			.HaveDependencyOn("AppHost")
			.GetResult();

		result.IsSuccessful.Should().BeTrue();
	}

	/// <summary>
	/// ServiceDefaults MUST have no inbound dependencies from main projects.
	/// Ensures ServiceDefaults remains a foundational layer with no circular references.
	/// </summary>
	[Fact]
	public void ServiceDefaults_MustHaveNoCircularDependencies()
	{
		var serviceDefaultsAssembly = System.Reflection.Assembly.Load("ServiceDefaults");

		var result = Types.InAssembly(serviceDefaultsAssembly)
			.ShouldNot()
			.HaveDependencyOnAny("IssueTracker.UI", "IssueTracker.CoreBusiness", "IssueTracker.Services", "IssueTracker.PlugIns")
			.GetResult();

		result.IsSuccessful.Should().BeTrue();
	}

	/// <summary>
	/// CoreBusiness should not depend on UI or AppHost (business logic isolation).
	/// Business logic must remain independent of presentation and infrastructure layers.
	/// </summary>
	[Fact]
	public void CoreBusiness_ShouldNotDependOnUIOrAppHost()
	{
		var coreBusinessAssembly = System.Reflection.Assembly.Load("IssueTracker.CoreBusiness");

		var result = Types.InAssembly(coreBusinessAssembly)
			.ShouldNot()
			.HaveDependencyOnAny("IssueTracker.UI", "AppHost")
			.GetResult();

		result.IsSuccessful.Should().BeTrue();
	}
}
