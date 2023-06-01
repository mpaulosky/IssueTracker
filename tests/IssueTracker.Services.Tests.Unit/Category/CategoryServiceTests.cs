namespace IssueTracker.Services.Category;

[ExcludeFromCodeCoverage]
public class CategoryServiceTests
{
	private readonly Mock<ICategoryRepository> _categoryRepositoryMock;

	private readonly Mock<IMemoryCache> _memoryCacheMock;
	private readonly Mock<ICacheEntry> _mockCacheEntry;

	public CategoryServiceTests()
	{
		_categoryRepositoryMock = new Mock<ICategoryRepository>();
		_memoryCacheMock = new Mock<IMemoryCache>();
		_mockCacheEntry = new Mock<ICacheEntry>();
	}

	private CategoryService UnitUnderTest()
	{
		return new CategoryService(_categoryRepositoryMock.Object, _memoryCacheMock.Object);
	}

	[Fact(DisplayName = "Create Category With Valid Values")]
	public async Task CreateCategory_With_Valid_Values_Should_Return_Test()
	{
		// Arrange
		var sut = UnitUnderTest();
		var expected = FakeCategory.GetNewCategory();

		// Act
		await sut.CreateCategory(expected);

		// Assert
		sut.Should().NotBeNull();
		expected.Id.Should().NotBeNull();

		_categoryRepositoryMock
			.Verify(x =>
				x.CreateAsync(It.IsAny<CategoryModel>()), Times.Once);
	}

	[Fact(DisplayName = "Create Category With Invalid Category Throws Exception")]
	public async Task Create_With_Invalid_Category_Should_Return_ArgumentNullException_TestAsync()
	{
		// Arrange
		var sut = UnitUnderTest();

		// Act
		var act = async () => { await sut.CreateCategory(null!); };

		// Assert
		await act.Should()
			.ThrowAsync<ArgumentNullException>()
			.WithParameterName("category")
			.WithMessage("Value cannot be null. (Parameter 'category')");
	}

	[Fact(DisplayName = "Delete Category")]
	public async Task DeleteCategory_With_Valid_Values_Should_Delete_The_Category_TestAsync()
	{
		// Arrange
		var sut = UnitUnderTest();
		var expected = FakeCategory.GetNewCategory(true);

		// Act
		await sut.DeleteCategory(expected);

		// Assert
		sut.Should().NotBeNull();

		_categoryRepositoryMock
			.Verify(x =>
				x.ArchiveAsync(It.IsAny<CategoryModel>()), Times.Once);
	}

	[Fact(DisplayName = "Get Category With Valid Id")]
	public async Task GetCategory_With_Valid_Id_Should_Return_Expected_Category_Test()
	{
		//Arrange
		var sut = UnitUnderTest();
		var expected = FakeCategory.GetNewCategory(true);

		_categoryRepositoryMock.Setup(x => x.GetAsync(It.IsAny<string>())).ReturnsAsync(expected);

		//Act
		var result = await sut.GetCategory(expected.Id);

		//Assert
		result.Should().NotBeNull();
		result.Id.Should().Be(expected.Id);
	}

	[Theory(DisplayName = "Get Category With Invalid Id")]
	[InlineData(null, "categoryId", "Value cannot be null.?*")]
	[InlineData("", "categoryId", "The value cannot be an empty string.?*")]
	public async Task GetCategory_With_Invalid_Id_Should_Return_An_ArgumentException_TestAsync(string value,
		string expectedParamName, string expectedMessage)
	{
		// Arrange
		var sut = UnitUnderTest();

		// Act
		var act = async () => { await sut.GetCategory(value); };

		// Assert
		await act.Should()
			.ThrowAsync<ArgumentException>()
			.WithParameterName(expectedParamName)
			.WithMessage(expectedMessage);
	}

	[Fact(DisplayName = "Get Categories")]
	public async Task GetCategories_Should_Return_A_List_Of_Categories_Test()
	{
		//Arrange
		var sut = UnitUnderTest();
		const int expectedCount = 5;

		var expected = FakeCategory.GetCategories();

		_categoryRepositoryMock.Setup(x => x.GetAllAsync()).ReturnsAsync(expected);

		_memoryCacheMock
			.Setup(mc => mc.CreateEntry(It.IsAny<object>()))
			.Callback((object k) => _ = (string)k)
			.Returns(_mockCacheEntry.Object);

		//Act
		var results = await sut.GetCategories();

		//Assert
		results.Should().NotBeNull();
		results.Count.Should().Be(expectedCount);
	}

	[Fact(DisplayName = "Get Categories with cache")]
	public async Task GetCategories_With_Memory_Cache_Should_A_List_Of_Categories_Test()
	{
		//Arrange
		var sut = UnitUnderTest();
		const int expectedCount = 5;

		var expected = FakeCategory.GetCategories();

		_memoryCacheMock
			.Setup(mc => mc.CreateEntry(It.IsAny<object>()))
			.Callback((object k) => _ = k as string)
			.Returns(_mockCacheEntry.Object);

		object whatever = expected;

		_memoryCacheMock
			.Setup(mc => mc.TryGetValue(It.IsAny<object>(), out whatever!))
			.Callback(new OutDelegate<object, object>((object _, out object v) =>
				v = whatever)) // mocked value here (and/or breakpoint)
			.Returns(true);

		//Act
		var results = await sut.GetCategories();

		//Assert
		results.Should().NotBeNull();
		results.Count.Should().Be(expectedCount);
	}

	[Fact(DisplayName = "Update Category With Valid Category")]
	public async Task UpdateCategory_With_A_Valid_Category_Should_Succeed_Test()
	{
		// Arrange
		var sut = UnitUnderTest();
		var updatedCategory = FakeCategory.GetNewCategory(true);

		// Act
		await sut.UpdateCategory(updatedCategory);

		// Assert
		sut.Should().NotBeNull();

		_categoryRepositoryMock
			.Verify(x =>
				x.UpdateAsync(It.IsAny<string>(), It.IsAny<CategoryModel>()), Times.Once);
	}

	[Fact(DisplayName = "Update With Invalid Category")]
	public async Task UpdateCategory_With_Invalid_Category_Should_Return_ArgumentNullException_Test()
	{
		// Arrange
		var sut = UnitUnderTest();
		const string expectedPramName = "category";
		const string expectedMessage = "Value cannot be null.?*";

		// Act
		var act = async () => { await sut.UpdateCategory(null!); };

		// Assert
		await act.Should()
			.ThrowAsync<ArgumentNullException>()
			.WithParameterName(expectedPramName)
			.WithMessage(expectedMessage);
	}

	private delegate void OutDelegate<in TIn, TOut>(TIn input, out TOut output);
}
