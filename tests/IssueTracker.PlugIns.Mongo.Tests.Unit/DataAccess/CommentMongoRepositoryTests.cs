namespace IssueTracker.PlugIns.Mongo.Tests.Unit.DataAccess;

public class CommentMongoRepositoryTests
{
	private readonly MockRepository mockRepository;

	private readonly Mock<IMongoDbContextFactory> mockMongoDbContextFactory;

	public CommentMongoRepositoryTests()
	{
		this.mockRepository = new MockRepository(MockBehavior.Strict);

		this.mockMongoDbContextFactory = this.mockRepository.Create<IMongoDbContextFactory>();
	}

	private CommentRepository CreateCommentMongoRepository()
	{
		return new CommentRepository(
				this.mockMongoDbContextFactory.Object);
	}

	[Fact]
	public async Task CreateCommentAsync_StateUnderTest_ExpectedBehavior()
	{
		// Arrange
		var commentMongoRepository = this.CreateCommentMongoRepository();
		CommentModel? comment = null;

		// Act
		await commentMongoRepository.CreateCommentAsync(
			comment);

		// Assert
		Assert.True(false);
		this.mockRepository.VerifyAll();
	}

	[Fact]
	public async Task GetCommentByIdAsync_StateUnderTest_ExpectedBehavior()
	{
		// Arrange
		var commentMongoRepository = this.CreateCommentMongoRepository();
		string? commentId = null;

		// Act
		var result = await commentMongoRepository.GetCommentByIdAsync(
			commentId);

		// Assert
		Assert.True(false);
		this.mockRepository.VerifyAll();
	}

	[Fact]
	public async Task GetCommentsAsync_StateUnderTest_ExpectedBehavior()
	{
		// Arrange
		var commentMongoRepository = this.CreateCommentMongoRepository();

		// Act
		var result = await commentMongoRepository.GetCommentsAsync();

		// Assert
		Assert.True(false);
		this.mockRepository.VerifyAll();
	}

	[Fact]
	public async Task GetCommentsBySourceAsync_StateUnderTest_ExpectedBehavior()
	{
		// Arrange
		var commentMongoRepository = this.CreateCommentMongoRepository();
		BasicCommentOnSourceModel? source = null;

		// Act
		var result = await commentMongoRepository.GetCommentsBySourceAsync(
			source);

		// Assert
		Assert.True(false);
		this.mockRepository.VerifyAll();
	}

	[Fact]
	public async Task GetCommentsByUserIdAsync_StateUnderTest_ExpectedBehavior()
	{
		// Arrange
		var commentMongoRepository = this.CreateCommentMongoRepository();
		string? userId = null;

		// Act
		var result = await commentMongoRepository.GetCommentsByUserIdAsync(
			userId);

		// Assert
		Assert.True(false);
		this.mockRepository.VerifyAll();
	}

	[Fact]
	public async Task UpdateCommentAsync_StateUnderTest_ExpectedBehavior()
	{
		// Arrange
		var commentMongoRepository = this.CreateCommentMongoRepository();
		CommentModel? comment = null;

		// Act
		await commentMongoRepository.UpdateCommentAsync(
			comment);

		// Assert
		Assert.True(false);
		this.mockRepository.VerifyAll();
	}

	[Fact]
	public async Task UpVoteCommentAsync_StateUnderTest_ExpectedBehavior()
	{
		// Arrange
		var commentMongoRepository = this.CreateCommentMongoRepository();
		string? itemId = null;
		string? userId = null;

		// Act
		await commentMongoRepository.UpVoteCommentAsync(
			itemId,
			userId);

		// Assert
		Assert.True(false);
		this.mockRepository.VerifyAll();
	}
}
