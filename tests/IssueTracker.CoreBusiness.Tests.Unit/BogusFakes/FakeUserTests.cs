﻿namespace IssueTracker.CoreBusiness.BogusFakes;

[ExcludeFromCodeCoverage]
public class FakeUserTests
{

	[Theory(DisplayName = "FakeUser GetNewUser Tests")]
	[InlineData(true)]
	[InlineData(false)]
	public void GetNewUser_With_Boolean_Should_Return_With_Or_Without_An_Id_Test(bool expected)
	{

		// Arrange
		// Act
		var result = FakeUser.GetNewUser(expected);

		// Assert
		switch (expected)
		{
			case true:
				result.Id.Should().NotBeNull();
				break;
			default:
				result.Id.Should().BeNullOrWhiteSpace();
				break;
		}

		result.FirstName.Should().NotBeNull();
		result.LastName.Should().NotBeNull();
		result.DisplayName.Should().NotBeNull();
		result.EmailAddress.Should().NotBeNull();
		result.Archived.Should().BeFalse();

	}

	[Fact(DisplayName = "FakeUser GetUsers Test")]
	public void GetUser_WhenUserRequested_Returns_FakeUser_Test()
	{

		// Arrange

		// Act
		var sut = FakeUser.GetUsers(1).ToList();

		// Assert
		sut.Count.Should().Be(1);
		sut.First().Id.Should().NotBeNull();
		sut.First().FirstName.Should().NotBeNull();
		sut.First().LastName.Should().NotBeNull();
		sut.First().DisplayName.Should().NotBeNull();
		sut.First().EmailAddress.Should().NotBeNull();

	}

	[Fact(DisplayName = "FakeUser GetBasicUser Test")]
	public void GetBasicUser_WhenBasicUserRequested_Returns_FakeBasicUser_Test()
	{

		// Arrange

		// Act
		var sut = FakeUser.GetBasicUser(1).ToList();

		// Assert
		sut.Count.Should().Be(1);
		sut.First().Id.Should().NotBeNull();
		sut.First().FirstName.Should().NotBeNull();
		sut.First().LastName.Should().NotBeNull();
		sut.First().DisplayName.Should().NotBeNull();
		sut.First().EmailAddress.Should().NotBeNull();

	}

}
