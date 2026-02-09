// ============================================
// Copyright (c) 2023. All rights reserved.
// File Name :     CategoryRepositoryTests.cs
// Company :       mpaulosky
// Author :        Matthew Paulosky
// Solution Name : IssueTracker
// Project Name :  IssueTracker.PlugIns.Tests.Unit
// =============================================

namespace IssueTracker.PlugIns.DataAccess;

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

	[Fact(DisplayName = "Archive Category")]
	public async Task ArchiveCategory_With_Valid_Category_Should_Archive_the_Category_TestAsync()
	{
		// Arrange
		CategoryModel archivedCategory = FakeCategory.GetCategories(1).First();
		archivedCategory.Archived = true;

		SetupMongoCollection(archivedCategory);

		CategoryRepository sut = CreateRepository();

		// Act
		await sut.ArchiveAsync(archivedCategory);

		// Assert
		//Verify if ReplaceOneAsync is called once
		_mockCollection.Verify(c => c.ReplaceOneAsync(
			It.IsAny<FilterDefinition<CategoryModel>>(),
			archivedCategory,
			It.IsAny<ReplaceOptions>(), It.IsAny<CancellationToken>()), Times.Once);
	}

	[Fact(DisplayName = "Create Category")]
	public async Task Create_With_Valid_Category_Should_Insert_A_New_Category_TestAsync()
	{
		// Arrange
		CategoryModel newCategory = FakeCategory.GetNewCategory();

		SetupMongoCollection(null);

		CategoryRepository sut = CreateRepository();

		// Act
		await sut.CreateAsync(newCategory);

		// Assert
		//Verify if InsertOneAsync is called once 
		_mockCollection.Verify(c => c
			.InsertOneAsync(
				newCategory,
				null,
				default), Times.Once);
	}

	[Fact(DisplayName = "Get Category With a Valid Id")]
	public async Task GetCategory_With_Valid_Id_Should_Returns_One_Category_Test()
	{
		// Arrange
		CategoryModel expected = FakeCategory.GetNewCategory(true);

		_list = new List<CategoryModel> { expected };

		_cursor.Setup(_ => _.Current).Returns(_list);

		_mockContext.Setup(c => c.GetCollection<CategoryModel>(It.IsAny<string>())).Returns(_mockCollection.Object);

		CategoryRepository sut = CreateRepository();

		//Act
		CategoryModel result = await sut.GetAsync(expected.Id);

		//Assert 
		result.Should().NotBeNull();
		result.Should().BeEquivalentTo(expected);

		//Verify if InsertOneAsync is called once
		_mockCollection
			.Verify(c => c
				.FindAsync(
					It.IsAny<FilterDefinition<CategoryModel>>(),
					It.IsAny<FindOptions<CategoryModel>>(),
					It.IsAny<CancellationToken>()), Times.Once);
	}

	[Fact(DisplayName = "Get Categories")]
	public async Task GetCategories_With_Valid_Context_Should_Return_A_List_Of_Categories_Test()
	{
		// Arrange
		const int expectedCount = 5;
		List<CategoryModel> expected = FakeCategory.GetCategories().ToList();

		_list = new List<CategoryModel>(expected);

		_cursor.Setup(_ => _.Current).Returns(_list);

		_mockContext.Setup(c => c.GetCollection<CategoryModel>(It.IsAny<string>())).Returns(_mockCollection.Object);

		CategoryRepository sut = CreateRepository();

		// Act
		List<CategoryModel> results = (await sut.GetAllAsync()).ToList();

		// Assert
		results.Should().NotBeNull();
		results.Should().HaveCount(expectedCount);

		_mockCollection.Verify(c => c
			.FindAsync(
				It.IsAny<FilterDefinition<CategoryModel>>(),
				It.IsAny<FindOptions<CategoryModel>>(),
				It.IsAny<CancellationToken>()), Times.Once);
	}

	[Fact(DisplayName = "Update Category")]
	public async Task UpdateCategory_With_A_Valid_Id_And_Category_Should_UpdateCategory_Test()
	{
		// Arrange
		CategoryModel expected = FakeCategory.GetNewCategory(true);

		CategoryModel updatedCategory =
			FakeCategory.GetNewCategory(true);
		updatedCategory.Id = expected.Id;
		updatedCategory.CategoryDescription = "Updated New";

		await _mockCollection.Object.InsertOneAsync(expected);

		_list = new List<CategoryModel> { updatedCategory };

		_cursor.Setup(_ => _.Current).Returns(_list);

		_mockContext.Setup(c => c
			.GetCollection<CategoryModel>(It.IsAny<string>())).Returns(_mockCollection.Object);

		CategoryRepository sut = CreateRepository();

		// Act
		await sut.UpdateAsync(updatedCategory.Id, updatedCategory);

		// Assert
		_mockCollection.Verify(
			c => c
				.ReplaceOneAsync(
					It.IsAny<FilterDefinition<CategoryModel>>(),
					updatedCategory,
					It.IsAny<ReplaceOptions>(),
					It.IsAny<CancellationToken>()), Times.Once);
	}

	private void SetupMongoCollection(CategoryModel? category)
	{
		if (category is not null)
		{
			_list = new List<CategoryModel> { category };

			_cursor.Setup(_ => _.Current).Returns(_list);
		}

		_mockContext
			.Setup(c => c.GetCollection<CategoryModel>(It.IsAny<string>()))
			.Returns(_mockCollection.Object);
	}
}