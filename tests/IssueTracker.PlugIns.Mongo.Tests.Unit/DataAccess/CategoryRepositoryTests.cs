namespace IssueTracker.PlugIns.Mongo.Tests.Unit.DataAccess;

[ExcludeFromCodeCoverage]
public class CategoryRepositoryTests
{

	private readonly Mock<IAsyncCursor<CategoryModel>> _cursor;
	private readonly Mock<IMongoCollection<CategoryModel>> _mockCollection;
	private readonly Mock<IMongoDbContextFactory> _mockContext;
	private List<CategoryModel> _list = new();

	public CategoryRepositoryTests()
	{

		_cursor = TestFixtures.GetMockCursor(_list);

		_mockCollection = TestFixtures.GetMockCollection(_cursor);

		_mockContext = TestFixtures.GetMockContext();

	}

	private CategoryRepository CreateRepository()
	{

		return new CategoryRepository(_mockContext.Object);

	}

	[Fact(DisplayName = "Archive Category Test")]
	public async Task ArchiveCategory_With_Valid_Category_Should_Archive_the_Category_TestAsync()
	{

		// Arrange
		var expected = FakeCategory.GetNewCategory(true);

		var updatedCategory = FakeCategory.GetNewCategory(true);
		updatedCategory.Archived = true;

		await _mockCollection.Object.InsertOneAsync(expected);

		//_list = new List<CategoryModel> { updatedCategory };

		//_cursor.Setup(_ => _.Current).Returns(_list);

		_mockContext.Setup(c => c
			.GetCollection<CategoryModel>(It.IsAny<string>()))
			.Returns(_mockCollection.Object);

		var sut = CreateRepository();

		// Act
		await sut.UpdateAsync(updatedCategory);

		// Assert

		_mockCollection.Verify(
			c => c.
				ReplaceOneAsync(It.IsAny<FilterDefinition<CategoryModel>>(),
				updatedCategory,
				It.IsAny<ReplaceOptions>(),
				It.IsAny<CancellationToken>()), Times.Once);

	}

	[Fact(DisplayName = "Create Category Test")]
	public async Task CreateCategoryAsync_With_Valid_Category_Should_Insert_A_New_Category_TestAsync()
	{

		// Arrange
		var newCategory = FakeCategory.GetNewCategory();

		_mockContext.Setup(c => c
			.GetCollection<CategoryModel>(It.IsAny<string>()))
			.Returns(_mockCollection.Object);

		var sut = CreateRepository();

		// Act
		await sut.CreateAsync(newCategory);

		// Assert
		//Verify if InsertOneAsync is called once 
		_mockCollection.Verify(c => c
			.InsertOneAsync(newCategory, null, default), Times.Once);

	}

	[Fact(DisplayName = "Get Category By Id")]
	public async Task GetCategoryByIdAsync_With_Valid_Id_Should_Returns_One_Category_TestAsync()
	{

		// Arrange
		var expected = FakeCategory.GetCategories(1).First();

		_list = new List<CategoryModel> { expected };

		_cursor.Setup(_ => _.Current).Returns(_list);

		_mockContext.Setup(c => c
			.GetCollection<CategoryModel>(It.IsAny<string>()))
			.Returns(_mockCollection.Object);

		var sut = CreateRepository();

		//Act
		var result = await sut.GetAsync(expected.Id);

		//Assert 
		result.Should().NotBeNull();
		result.Should().BeEquivalentTo(expected);
		result!.CategoryName.Length.Should().BeGreaterThan(1);


		//Verify if InsertOneAsync is called once
		_mockCollection.Verify(c => c
			.FindAsync(It.IsAny<FilterDefinition<CategoryModel>>(),
			It.IsAny<FindOptions<CategoryModel>>(),
			It.IsAny<CancellationToken>()), Times.Once);

	}

	[Fact(DisplayName = "Get Categories Test")]
	public async Task GetCategoriesAsync_With_Valid_Context_Should_Return_A_List_Of_Categories_Test()
	{

		// Arrange
		const int expectedCount = 5;
		var expected = FakeCategory.GetCategories(expectedCount).ToList();

		await _mockCollection.Object.InsertManyAsync(expected);

		_list = new List<CategoryModel>(expected);

		_cursor.Setup(_ => _.Current).Returns(_list);

		_mockContext.Setup(c => c
			.GetCollection<CategoryModel>(It.IsAny<string>()))
			.Returns(_mockCollection.Object);

		var sut = CreateRepository();

		// Act
		var results = (await sut.GetAllAsync())!.ToList();

		// Assert
		results.Should().NotBeNull();
		results.Should().HaveCount(expectedCount);
		results.Should().BeEquivalentTo(expected);

		_mockCollection.Verify(c => c
			.FindAsync(It.IsAny<FilterDefinition<CategoryModel>>(),
			It.IsAny<FindOptions<CategoryModel>>(),
			It.IsAny<CancellationToken>()), Times.Once);

	}

	[Fact(DisplayName = "Update Category Test")]
	public async Task UpdateCategoryAsync_With_A_Valid_Id_And_Category_Should_UpdateCategory_Test()
	{

		// Arrange
		var expected = FakeCategory.GetNewCategory();
		expected.Id = new BsonObjectId(ObjectId.GenerateNewId()).ToString();

		await _mockCollection.Object.InsertOneAsync(expected);

		var updatedCategory = FakeCategory.GetNewCategory();

		updatedCategory.Id = expected.Id;
		updatedCategory.CategoryName = expected.CategoryName;
		updatedCategory.CategoryDescription = "Updated New CategoryDescription";

		_mockContext.Setup(c => c
			.GetCollection<CategoryModel>(It.IsAny<string>()))
			.Returns(_mockCollection.Object);

		var sut = CreateRepository();

		// Act
		await sut.UpdateAsync(updatedCategory).ConfigureAwait(false);

		// Assert
		_mockCollection.Verify(
			c => c
				.ReplaceOneAsync(It.IsAny<FilterDefinition<CategoryModel>>(),
				 updatedCategory,
				It.IsAny<ReplaceOptions>(),
				It.IsAny<CancellationToken>()), Times.Once);
	}

}
