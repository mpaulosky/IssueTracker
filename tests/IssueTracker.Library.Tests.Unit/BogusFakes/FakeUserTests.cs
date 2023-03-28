namespace IssueTracker.CoreBusiness.BogusFakes;

[ExcludeFromCodeCoverage]
public class FakeUserTests
{

	public FakeUserTests()
	{
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
		sut.First().DisplayName.Should().NotBeNull();

	}

}