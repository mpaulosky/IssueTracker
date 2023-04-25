namespace IssueTracker.PlugIns.Tests.Unit.Fixtures;

[ExcludeFromCodeCoverage]
public static class Fixtures
{

	public static Mock<IMongoDbContextFactory> GetMockMongoContext()
	{
		var mockClient = new Mock<IMongoClient>();
		var context = new Mock<IMongoDbContextFactory>();
		var mockSession = new Mock<IClientSessionHandle>();
		context.Setup(op => op.Client).Returns(mockClient.Object);
		context.Setup(op =>
				op.Client.StartSessionAsync(It.IsAny<ClientSessionOptions>(), It.IsAny<CancellationToken>()))
			.Returns(Task.FromResult(mockSession.Object));

		return context;
	}

}
