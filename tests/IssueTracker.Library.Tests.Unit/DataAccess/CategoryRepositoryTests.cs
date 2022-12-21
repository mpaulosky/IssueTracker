namespace IssueTracker.Library.DataAccess;

[ExcludeFromCodeCoverage]
public class CategoryRepositoryTests
{
	private readonly Mock<IAsyncCursor<CategoryModel>> _cursor;
	private readonly Mock<IMongoCollection<CategoryModel>> _mockCollection;
	private readonly Mock<IMongoDbContextFactory> _mockContext;
	private List<CategoryModel> _list = new();
	private CategoryRepository _sut;

	public CategoryRepositoryTests()
	{
		_cursor = TestFixtures.GetMockCursor(_list);

		_mockCollection = TestFixtures.GetMockCollection(_cursor);

		_mockContext = TestFixtures.GetMockContext();

		_sut = new CategoryRepository(_mockContext.Object);
	}

	[Fact(DisplayName = "Create Category")]
	public async Task Create_With_Valid_Category_Should_Insert_A_New_Category_TestAsync()
	{

		// Arrange

		var newCategory = TestCategories.GetKnownCategory();

		_mockContext.Setup(c => c.GetCollection<CategoryModel>(It.IsAny<string>())).Returns(_mockCollection.Object);

		_sut = new CategoryRepository(_mockContext.Object);

		// Act

		await _sut.CreateCategory(newCategory);

		// Assert

		//Verify if InsertOneAsync is called once 
		_mockCollection.Verify(c => c.InsertOneAsync(newCategory, null, default), Times.Once);

	}

	[Fact(DisplayName = "Delete Category")]
	public async Task DeleteCategory_With_Valid_Category_Should_Delete_the_Category_TestAsync()
	{

		// Arrange

		var category = TestCategories.GetKnownCategory();

		_mockContext.Setup(c => c.GetCollection<CategoryModel>(It.IsAny<string>())).Returns(_mockCollection.Object);

		_sut = new CategoryRepository(_mockContext.Object);

		// Act

		await _sut.DeleteCategory(category);

		// Assert

		_mockCollection.Verify(c => c.DeleteOneAsync(It.IsAny<FilterDefinition<CategoryModel>>(), It.IsAny<CancellationToken>()), Times.Once);

	}

	[Fact(DisplayName = "Get Category With a Valid Id")]
	public async Task GetCategory_With_Valid_Id_Should_Returns_One_Category_Test()
	{
		// Arrange

		var expected = TestCategories.GetKnownCategory();

		_list = new List<CategoryModel> { expected };

		_cursor.Setup(_ => _.Current).Returns(_list);

		_mockContext.Setup(c => c.GetCollection<CategoryModel>(It.IsAny<string>())).Returns(_mockCollection.Object);

		_sut = new CategoryRepository(_mockContext.Object);

		//Act

		var result = await _sut.GetCategory(expected.Id);

		//Assert 

		result.Should().NotBeNull();

		//Verify if InsertOneAsync is called once

		_mockCollection.Verify(c => c.FindAsync(It.IsAny<FilterDefinition<CategoryModel>>(),
			It.IsAny<FindOptions<CategoryModel>>(),
			It.IsAny<CancellationToken>()), Times.Once);

		result.Should().BeEquivalentTo(expected);
		result.CategoryName.Length.Should().BeGreaterThan(1);
	}

	[Fact(DisplayName = "Get Categories")]
	public async Task GetCategories_With_Valid_Context_Should_Return_A_List_Of_Categories_Test()
	{
		// Arrange
		const int expectedCount = 5;
		var expected = TestCategories.GetCategories().ToList();

		_list = new List<CategoryModel>(expected);

		_cursor.Setup(_ => _.Current).Returns(_list);

		_mockContext.Setup(c => c.GetCollection<CategoryModel>(It.IsAny<string>())).Returns(_mockCollection.Object);

		_sut = new CategoryRepository(_mockContext.Object);

		// Act

		var result = await _sut.GetCategories().ConfigureAwait(false);

		// Assert

		_mockCollection.Verify(c => c.FindAsync(It.IsAny<FilterDefinition<CategoryModel>>(),
			It.IsAny<FindOptions<CategoryModel>>(),
			It.IsAny<CancellationToken>()), Times.Once);

		var items = result.ToList();
		items.ToList().Should().NotBeNull();
		items.ToList().Should().HaveCount(expectedCount);
	}

	[Fact(DisplayName = "Update Category")]
	public async Task UpdateCategory_With_A_Valid_Id_And_Category_Should_UpdateCategory_Test()
	{
		// Arrange

		var expected = TestCategories.GetKnownCategory();

		var updatedCategory =
			TestCategories.GetCategory(expected.Id, expected.CategoryDescription, "Updated New");

		await _mockCollection.Object.InsertOneAsync(expected);

		_list = new List<CategoryModel> { updatedCategory };

		_cursor.Setup(_ => _.Current).Returns(_list);

		_mockContext.Setup(c => c.GetCollection<CategoryModel>(It.IsAny<string>())).Returns(_mockCollection.Object);

		_sut = new CategoryRepository(_mockContext.Object);

		// Act

		await _sut.UpdateCategory(updatedCategory.Id, updatedCategory);

		// Assert

		_mockCollection.Verify(
			c => c.ReplaceOneAsync(It.IsAny<FilterDefinition<CategoryModel>>(), updatedCategory,
				It.IsAny<ReplaceOptions>(),
				It.IsAny<CancellationToken>()), Times.Once);
	}
}