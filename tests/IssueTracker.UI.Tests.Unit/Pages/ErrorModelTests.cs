namespace IssueTracker.UI.Pages;

[ExcludeFromCodeCoverage]
public class ErrorModelTests
{
	[Fact]
	public void ShowRequestIdShouldReturnRequestId()
	{
		//Arrange
		var model = new ErrorModel()
		{
			RequestId = "12345",
		};

		//Assert
		Assert.True(model.ShowRequestId);
		Assert.Equal("12345", model.RequestId);
	}

	[Fact]
	public void ShowRequestIdShouldNotReturnRequestId()
	{
		//Arrange
		var model = new ErrorModel()
		{
			RequestId = null,
		};

		//Assert
		Assert.False(model.ShowRequestId);
		Assert.Null(model.RequestId);
	}
}
