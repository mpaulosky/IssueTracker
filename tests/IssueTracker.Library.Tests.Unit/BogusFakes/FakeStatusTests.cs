namespace IssueTracker.CoreBusiness.BogusFakes;

[ExcludeFromCodeCoverage]
public class FakeStatusTests
{

	public FakeStatusTests()
	{
	}

	[Fact(DisplayName = "FakeStatus GetStatuses Test")]
	public void GetStatuses_With_RequestForStatuses_Should_ReturnFakeStatuses_Test()
	{

		// Arrange


		// Act
		var result = FakeStatus.GetStatuses(1).ToList();

		// Assert
		result.Count.Should().Be(1);
		result.First().Id.Should().NotBeNull();
		result.First().StatusName.Should().NotBeNull();
		result.First().StatusDescription.Should().NotBeNull();

	}

	[Fact(DisplayName = "FakeStatus GetBasicStatuses Test")]
	public void GetBasicStatuses_With_RequestForBasicStatuses_Should_ReturnFakeBasicStatuses_Test()
	{

		// Arrange

		// Act
		var result = FakeStatus.GetBasicStatuses(1).ToList();

		// Assert
		result.Count.Should().Be(1);
		result.First().StatusName.Should().NotBeNull();
		result.First().StatusDescription.Should().NotBeNull();

	}

}