namespace TestingSupport.Library.Fixtures;

[ExcludeFromCodeCoverage]
public static class TestFixtures
{
	public static Mock<IAsyncCursor<TEntity>> GetMockCursor<TEntity>(List<TEntity> list) where TEntity : class?
	{
		var cursor = new Mock<IAsyncCursor<TEntity>>();
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

	public static Mock<IMongoCollection<TEntity>> GetMockCollection<TEntity>(Mock<IAsyncCursor<TEntity>> cursor)
		where TEntity : class?
	{
		var collection =
			new Mock<IMongoCollection<TEntity>> { Name = GetCollectionName(nameof(TEntity)) };
		collection.Setup(op =>
				op.FindAsync
				(
					It.IsAny<FilterDefinition<TEntity>>(),
					It.IsAny<FindOptions<TEntity, TEntity>>(),
					It.IsAny<CancellationToken>()
				))
			.ReturnsAsync(cursor.Object);
		collection.Setup(op =>
			op.InsertOneAsync
			(
				It.IsAny<TEntity>(),
				It.IsAny<InsertOneOptions>(),
				It.IsAny<CancellationToken>()
			)).Returns(Task.CompletedTask);
		// collection.Setup(op =>
		// 	op.ReplaceOneAsync
		// 	(
		// 		It.IsAny<FilterDefinition<TEntity>>(), 
		// 		It.IsAny<TEntity>(), 
		// 		It.IsAny<ReplaceOptions>(),
		// 		It.IsAny<CancellationToken>()
		// 	)).ReturnsAsync();

		return collection;
	}

	public static Mock<IMongoDbContextFactory> GetMockContext()
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

	public static IOptions<DatabaseSettings> Settings()
	{
		var settings =
			new DatabaseSettings { DatabaseName = "TestDb", ConnectionString = "mongodb://tes123" };

		return Options.Create(settings);
	}
	
	public static IOptions<DatabaseSettings> Settings(string databaseName, string connectionString)
	{
		var settings =
			new DatabaseSettings { DatabaseName = databaseName, ConnectionString = connectionString };

		return Options.Create(settings);
	}
}