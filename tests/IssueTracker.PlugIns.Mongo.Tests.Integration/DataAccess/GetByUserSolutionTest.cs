﻿namespace IssueTracker.PlugIns.Mongo.DataAccess;

[ExcludeFromCodeCoverage]
[Collection("Test Collection")]
public class GetByUserSolutionTest : IAsyncLifetime
{

	private readonly IssueTrackerTestFactory _factory;
	private readonly SolutionRepository _sut;
	private const string CleanupValue = "solutions";

	public GetByUserSolutionTest(IssueTrackerTestFactory factory)
	{

		_factory = factory;
		var context = _factory.Services.GetRequiredService<IMongoDbContextFactory>();
		_sut = new SolutionRepository(context);

	}

	[Fact]
	public async Task GetByUserAsync_With_Valid_Data_Should_Be_Successful_TestAsync()
	{

		// Arrange
		var solution = FakeSolution.GetNewSolution();
		await _sut.CreateAsync(solution);

		// Act
		var result = (await _sut.GetByUserAsync(solution.Author))!.ToList();

		// Assert
		result.Should().NotBeNull();
		result.Count.Should().Be(1);
		result[0].Author.Should().BeEquivalentTo(solution.Author);

	}

	public Task InitializeAsync() => Task.CompletedTask;

	public async Task DisposeAsync() => await _factory.ResetCollectionAsync(CleanupValue);

}
