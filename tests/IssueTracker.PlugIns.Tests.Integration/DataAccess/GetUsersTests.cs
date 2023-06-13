// Copyright (c) 2023. All rights reserved.
// File Name :     GetUsersTests.cs
// Company :       mpaulosky
// Author :        Matthew Paulosky
// Solution Name : IssueTracker
// Project Name :  IssueTracker.PlugIns.Tests.Integration

namespace IssueTracker.PlugIns.DataAccess;

[ExcludeFromCodeCoverage]
[Collection("Test Collection")]
public class GetUsersTests : IAsyncLifetime
{
	private const string CleanupValue = "users";

	private readonly IssueTrackerTestFactory _factory;
	private readonly UserRepository _sut;

	public GetUsersTests(IssueTrackerTestFactory factory)
	{
		_factory = factory;
		IMongoDbContextFactory context = _factory.Services.GetRequiredService<IMongoDbContextFactory>();
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
	public async Task GetAllAsync_With_ValidData_Should_ReturnUsers_Test()
	{
		// Arrange
		UserModel expected = FakeUser.GetNewUser();
		await _sut.CreateAsync(expected);

		// Act
		List<UserModel> results = (await _sut.GetAllAsync()).ToList();

		// Assert
		results.Count.Should().Be(1);
		results.First().DisplayName.Should().Be(expected.DisplayName);
		results.First().FirstName.Should().Be(expected.FirstName);
		results.First().LastName.Should().Be(expected.LastName);
	}
}