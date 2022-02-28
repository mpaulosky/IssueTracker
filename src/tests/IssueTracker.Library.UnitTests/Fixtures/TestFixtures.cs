using IssueTrackerLibrary.Contracts;

using MongoDB.Driver;

using Moq;

using System.Diagnostics.CodeAnalysis;
using System.Threading;
using System.Threading.Tasks;

namespace IssueTracker.Library.UnitTests.Fixtures;

[ExcludeFromCodeCoverage]
public static class TestFixtures
{
	public static Mock<IAsyncCursor<T>> MockCursor<T>()
	{
		var _cursor = new Mock<IAsyncCursor<T>>();
		_cursor
			.SetupSequence(_ => _.MoveNext(It.IsAny<CancellationToken>()))
			.Returns(true)
			.Returns(false);
		_cursor
			.SetupSequence(_ => _.MoveNextAsync(It.IsAny<CancellationToken>()))
			.Returns(Task.FromResult(true))
			.Returns(Task.FromResult(false));
		return _cursor;
	}

	public static Mock<IMongoCollection<T>> MockCollection<T>(Mock<IAsyncCursor<T>> cursor)
	{
		var collection = new Mock<IMongoCollection<T>>();
		collection.Setup(op => op.FindAsync(It.IsAny<FilterDefinition<T>>(),
			It.IsAny<FindOptions<T, T>>(),
			It.IsAny<CancellationToken>())).ReturnsAsync(cursor.Object);
		return collection;
	}

	public static Mock<IMongoDbContext> MockContext<T>(Mock<IMongoCollection<T>> collection)
	{
		var context = new Mock<IMongoDbContext>();
		context.Setup(c => c.GetCollection<T>(typeof(T).Name)).Returns(collection.Object);

		return context;
	}
}
