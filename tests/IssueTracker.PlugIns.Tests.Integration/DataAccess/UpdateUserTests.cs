﻿namespace IssueTracker.PlugIns.DataAccess;

[ExcludeFromCodeCoverage]
[Collection("Test Collection")]
public class UpdateUserTests : IAsyncLifetime
{
	private const string CleanupValue = "users";

	private readonly IssueTrackerTestFactory _factory;
	private readonly UserRepository _sut;

	public UpdateUserTests(IssueTrackerTestFactory factory)
	{
		_factory = factory;
		var context = _factory.Services.GetRequiredService<IMongoDbContextFactory>();
		_sut = new UserRepository(context);
	}

	public Task InitializeAsync()
	{
		return Task.CompletedTask;
	}

	public async Task DisposeAsync()
	{
		await _factory.ResetCollectionAsync(CleanupValue);
	}

	[Fact]
	public async Task UpdateAsync_With_ValidData_Should_UpdateTheUser_Test()
	{
		// Arrange
		var expected = FakeUser.GetNewUser();
		await _sut.CreateAsync(expected);

		// Act
		expected.DisplayName = "Updated";
		await _sut.UpdateAsync(expected.Id, expected);
		var result = await _sut.GetAsync(expected.Id);

		// Assert
		result.Should().BeEquivalentTo(expected);
	}

	[Fact]
	public async Task UpdateAsync_With_WithInValidData_Should_ThrowArgumentNullException_Test()
	{
		// Arrange

		// Act
		var act = async () => await _sut.UpdateAsync(null!, null!);

		// Assert
		await act.Should().ThrowAsync<ArgumentNullException>();
	}
}
