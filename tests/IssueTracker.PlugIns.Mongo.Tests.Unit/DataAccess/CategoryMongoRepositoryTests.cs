
using TestingSupport.Library.Fixtures;

namespace IssueTracker.PlugIns.Mongo.Tests.Unit.DataAccess;

[ExcludeFromCodeCoverage]
public class CategoryMongoRepositoryTests
{

	private readonly Mock<IAsyncCursor<CategoryModel>> _cursor;
	private readonly Mock<IMongoCollection<CategoryModel>> _mockCollection;
	private readonly Mock<IMongoDbContextFactory> _mockContext;
	private readonly List<CategoryModel> _list = new();
	private readonly CategoryRepository _sut;

	public CategoryMongoRepositoryTests()
	{

		_cursor = TestFixtures.GetMockCursor(_list);

		_mockCollection = TestFixtures.GetMockCollection(_cursor);

		_mockContext = TestFixtures.GetMockContext();

		_sut = new CategoryRepository(_mockContext.Object);

	}

	private CategoryRepository CreateCategoryMongoRepository()
	{
		return new CategoryRepository(_mockContext.Object);
	}

	[Fact]
	public async Task GetCategoryByIdAsync_StateUnderTest_ExpectedBehavior()
	{
		// Arrange
		var categoryMongoRepository = CreateCategoryMongoRepository();
		string? categoryId = null;

		// Act
		var result = await categoryMongoRepository.GetCategoryByIdAsync(
			categoryId);

		// Assert
		Assert.True(false);
		this.mockRepository.VerifyAll();
	}

	[Fact]
	public async Task GetCategoriesAsync_StateUnderTest_ExpectedBehavior()
	{
		// Arrange
		var categoryMongoRepository = this.CreateCategoryMongoRepository();

		// Act
		var result = await categoryMongoRepository.GetCategoriesAsync();

		// Assert
		Assert.True(false);
		this.mockRepository.VerifyAll();
	}

	[Fact]
	public async Task CreateCategoryAsync_StateUnderTest_ExpectedBehavior()
	{
		// Arrange
		var categoryMongoRepository = this.CreateCategoryMongoRepository();
		CategoryModel? category = null;

		// Act
		await categoryMongoRepository.CreateCategoryAsync(
			category);

		// Assert
		Assert.True(false);
		this.mockRepository.VerifyAll();
	}

	[Fact]
	public async Task UpdateCategoryAsync_StateUnderTest_ExpectedBehavior()
	{
		// Arrange
		var categoryMongoRepository = this.CreateCategoryMongoRepository();
		CategoryModel? category = null;

		// Act
		await categoryMongoRepository.UpdateCategoryAsync(
			category);

		// Assert
		Assert.True(false);
		this.mockRepository.VerifyAll();
	}
}
