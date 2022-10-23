﻿namespace IssueTracker.Library.Services.UserServicesTests;

[ExcludeFromCodeCoverage]
[Collection("Database")]
public class UpdateUserTests : IClassFixture<IssueTrackerTestFactory>
{

	private readonly IssueTrackerTestFactory _factory;
	private readonly UserService _sut;

	public UpdateUserTests(IssueTrackerTestFactory factory)
	{

		_factory = factory;
		var repo = (IUserRepository)_factory.Services.GetRequiredService(typeof(IUserRepository));
		_sut = new UserService(repo);

	}

	[Fact]
	public async Task UpdateUser_With_ValidData_Should_UpdateTheUser_Test()
	{

		// Arrange
		var expected = FakeUser.GetNewUser();
		await _sut.CreateUser(expected);

		// Act
		expected.DisplayName = "Updated";
		await _sut.UpdateUser(expected);
		var result = await _sut.GetUser(expected.Id);

		// Assert
		result.Should().BeEquivalentTo(expected);

	}

	[Fact]
	public async Task UpdateUser_With_WithInValidData_Should_ThrowArgumentNullException_Test()
	{

		// Arrange
		UserModel expected = null;

		// Act
		var act = async () => await _sut.UpdateUser(expected);

		// Assert
		await act.Should().ThrowAsync<ArgumentNullException>();

	}

}