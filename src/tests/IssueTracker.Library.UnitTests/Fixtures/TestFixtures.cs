using MongoDB.Driver;

using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace IssueTracker.Library.UnitTests.Fixtures;

[ExcludeFromCodeCoverage]
public static class TestFixtures
{
	public static Mock<IAsyncCursor<T>> GetMockCursor<T>(List<T> list)
	{
		var cursor = new Mock<IAsyncCursor<T>>();
		cursor.Setup(_ => _.Current).Returns(list);
		cursor
			.SetupSequence(_ => _.MoveNext(It.IsAny<CancellationToken>()))
			.Returns(true)
			.Returns(false);
		cursor
			.SetupSequence(_ => _.MoveNextAsync(It.IsAny<CancellationToken>()))
			.Returns(Task.FromResult(true))
			.Returns(Task.FromResult(false));
		return cursor;
	}

	public static Mock<IMongoCollection<T>> GetMockCollection<T>(Mock<IAsyncCursor<T>> cursor)
	{
		var collection = new Mock<IMongoCollection<T>> { Name = CollectionNames.GetCollectionName(nameof(T)) };
		collection.Setup(op =>
				op.FindAsync
				(
					It.IsAny<FilterDefinition<T>>(),
					It.IsAny<FindOptions<T, T>>(),
					It.IsAny<CancellationToken>()
				))
			.ReturnsAsync(cursor.Object);
		collection.Setup(op =>
			op.InsertOneAsync
			(
				It.IsAny<T>(),
				It.IsAny<InsertOneOptions>(),
				It.IsAny<CancellationToken>()
			)).Returns(Task.CompletedTask);
		return collection;
	}

	public static Mock<IMongoDbContext> GetMockContext<T>()
	{
		var mockClient = new Mock<IMongoClient>();
		var context = new Mock<IMongoDbContext>();
		var mockSession = new Mock<IClientSessionHandle>();
		context.Setup(op => op.Client).Returns(mockClient.Object);
		context.Setup(op => op.Client.StartSessionAsync(It.IsAny<ClientSessionOptions>(), It.IsAny<CancellationToken>())).Returns(Task.FromResult<IClientSessionHandle>(mockSession.Object));

		return context;
	}

	public static IOptions<DatabaseSettings> Settings()
	{
		var settings = new DatabaseSettings() { DatabaseName = "TestDb", ConnectionString = "mongodb://tes123" };

		return Options.Create(settings);
	}
}