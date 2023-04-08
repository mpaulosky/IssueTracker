﻿namespace IssueTracker.PlugIns.Mongo.Services.UserServicesTests;

[ExcludeFromCodeCoverage]
[Collection("Test Collection")]
public class GetUserFromAuthenticationTests : IAsyncLifetime
{
	private readonly IssueTrackerTestFactory _factory;
	private readonly UserService _sut;
	private string _cleanupValue;

	public GetUserFromAuthenticationTests(IssueTrackerTestFactory factory)
	{

		_factory = factory;
		var repo = (IUserRepository)_factory.Services.GetRequiredService(typeof(IUserRepository));
		_sut = new UserService(repo);

	}

	[Fact]
	public async Task GetUserFromAuthentication_With_ValidData_Should_ReturnAUser_Test()
	{

		// Arrange
		_cleanupValue = "users";
		UserModel expected = FakeUser.GetNewUser();
		await _sut.CreateUser(expected);

		// Act
		UserModel result = await _sut.GetUserFromAuthentication(expected.ObjectIdentifier);

		// Assert
		result.Should().BeEquivalentTo(expected);
	}

	public Task InitializeAsync()
	{
		return Task.CompletedTask;
	}

	public async Task DisposeAsync()
	{

		await _factory.ResetCollectionAsync(_cleanupValue);

	}

}
