namespace IssueTracker.UI.Pages;

[ExcludeFromCodeCoverage]
public class ErrorTests
{
	private readonly ErrorModel _errorModel;

	public ErrorTests()
	{
		_errorModel = new ErrorModel();
	}

	[Fact]
	public void ShowRequestId_Should_ReturnFalse()
	{
		bool result = _errorModel.ShowRequestId;

		Assert.False(result);
	}

	[Fact]
	public void RequestId_Should_ReturnNull()
	{
		string result = _errorModel.RequestId;

		Assert.Null(result);
	}
}