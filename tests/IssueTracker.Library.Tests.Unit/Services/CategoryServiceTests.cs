namespace IssueTracker.CoreBusiness.Services;

[ExcludeFromCodeCoverage]
public class CategoryServiceTests
{
	private readonly Mock<IMemoryCache> _memoryCacheMock;
	private readonly Mock<ICacheEntry> _mockCacheEntry;
	private readonly Mock<ICategoryRepository> _categoryRepositoryMock;
	private CategoryService _sut;

	public CategoryServiceTests()
	{
		_categoryRepositoryMock = new Mock<ICategoryRepository>();
		_memoryCacheMock = new Mock<IMemoryCache>();
		_mockCacheEntry = new Mock<ICacheEntry>();
		_sut = new CategoryService(_categoryRepositoryMock.Object, _memoryCacheMock.Object);
	}

	[Fact(DisplayName = "Create Category With Valid Values")]
	public async Task CreateCategory_With_Valid_Values_Should_Return_Test()
	{

		// Arrange

		CategoryModel expected = TestCategories.GetNewCategory();

		// Act

		await _sut.CreateCategory(expected);

		// Assert

		_sut.Should().NotBeNull();

		_categoryRepositoryMock
			.Verify(x =>
				x.CreateCategoryAsync(It.IsAny<CategoryModel>()), Times.Once);

	}

	[Fact(DisplayName = "Create Category With Invalid Category Throws Exception")]
	public async Task Create_With_Invalid_Category_Should_Return_ArgumentNullException_TestAsync()
	{
		// Arrange

		_sut = new CategoryService(_categoryRepositoryMock.Object, _memoryCacheMock.Object);

		// Act

		// Assert

		_ = await Assert.ThrowsAsync<ArgumentNullException>(() => _sut.CreateCategory(null));
	}

	[Fact(DisplayName = "Delete Category")]
	public async Task DeleteCategory_With_Valid_Values_Should_Delete_The_Category_TestAsync()
	{

		// Arrange

		CategoryModel expected = TestCategories.GetKnownCategory();

		_sut = new CategoryService(_categoryRepositoryMock.Object, _memoryCacheMock.Object);

		// Act

		await _sut.DeleteCategory(expected);

		// Assert

		_sut.Should().NotBeNull();

		_categoryRepositoryMock
			.Verify(x =>
				x.ArchiveCategoryAsync(It.IsAny<CategoryModel>()), Times.Once);

	}

	[Fact(DisplayName = "Get Category With Valid Id")]
	public async Task GetCategory_With_Valid_Id_Should_Return_Expected_Category_Test()
	{
		//Arrange

		CategoryModel expected = TestCategories.GetKnownCategory();

		_categoryRepositoryMock.Setup(x => x.GetCategoryAsync(It.IsAny<string>())).ReturnsAsync(expected);

		_sut = new CategoryService(_categoryRepositoryMock.Object, _memoryCacheMock.Object);

		//Act

		CategoryModel result = await _sut.GetCategory(expected.Id);

		//Assert

		result.Should().NotBeNull();
		result.Id.Should().Be(expected.Id);
	}

	[Fact(DisplayName = "Get Category With Empty String Id")]
	public async Task GetCategory_With_Empty_String_Id_Should_Return_An_ArgumentException_TestAsync()
	{
		// Arrange

		_sut = new CategoryService(_categoryRepositoryMock.Object, _memoryCacheMock.Object);

		// Act

		// Assert

		await Assert.ThrowsAsync<ArgumentException>(() => _sut.GetCategory(""));
	}

	[Fact(DisplayName = "Get Category With Null Id")]
	public async Task GetCategory_With_Null_Id_Should_Return_An_ArgumentNullException_TestAsync()
	{
		// Arrange

		_sut = new CategoryService(_categoryRepositoryMock.Object, _memoryCacheMock.Object);

		// Act

		// Assert

		await Assert.ThrowsAsync<ArgumentNullException>(() => _sut.GetCategory(null));
	}

	[Fact(DisplayName = "Get Categories")]
	public async Task GetCategories_Should_Return_A_List_Of_Categories_Test()
	{
		//Arrange

		const int expectedCount = 5;

		IEnumerable<CategoryModel> expected = TestCategories.GetCategories();

		_categoryRepositoryMock.Setup(x => x.GetCategoriesAsync()).ReturnsAsync(expected);

		_memoryCacheMock
			.Setup(mc => mc.CreateEntry(It.IsAny<object>()))
			.Callback((object k) => _ = (string)k)
			.Returns(_mockCacheEntry.Object);

		_sut = new CategoryService(_categoryRepositoryMock.Object, _memoryCacheMock.Object);

		//Act

		List<CategoryModel> results = await _sut.GetCategories();

		//Assert

		results.Should().NotBeNull();
		results.Count.Should().Be(expectedCount);
	}

	[Fact(DisplayName = "Get Categories with cache")]
	public async Task GetCategories_With_Memory_Cache_Should_A_List_Of_Categories_Test()
	{
		//Arrange

		const int expectedCount = 5;

		IEnumerable<CategoryModel> expected = TestCategories.GetCategories();


		_memoryCacheMock
			.Setup(mc => mc.CreateEntry(It.IsAny<object>()))
			.Callback((object k) => _ = k as string)
			.Returns(_mockCacheEntry.Object);

		object whatever = expected;
		_memoryCacheMock
			.Setup(mc => mc.TryGetValue(It.IsAny<object>(), out whatever))
			.Callback(new OutDelegate<object, object>((object _, out object v) =>
				v = whatever)) // mocked value here (and/or breakpoint)
			.Returns(true);

		_sut = new CategoryService(_categoryRepositoryMock.Object, _memoryCacheMock.Object);

		//Act

		List<CategoryModel> results = await _sut.GetCategories();

		//Assert

		results.Should().NotBeNull();
		results.Count.Should().Be(expectedCount);
	}

	[Fact(DisplayName = "Update Category With Valid Category")]
	public async Task UpdateCategory_With_A_Valid_Category_Should_Succeed_Test()
	{
		// Arrange

		CategoryModel updatedCategory = TestCategories.GetUpdatedCategory();

		_sut = new CategoryService(_categoryRepositoryMock.Object, _memoryCacheMock.Object);

		// Act

		await _sut.UpdateCategory(updatedCategory);

		// Assert

		_sut.Should().NotBeNull();

		_categoryRepositoryMock
			.Verify(x =>
				x.UpdateCategoryAsync(It.IsAny<string>(), It.IsAny<CategoryModel>()), Times.Once);
	}

	[Fact(DisplayName = "Update With Invalid Category")]
	public async Task UpdateCategory_With_Invalid_Category_Should_Return_ArgumentNullException_Test()
	{
		// Arrange

		_sut = new CategoryService(_categoryRepositoryMock.Object, _memoryCacheMock.Object);

		// Act

		// Assert

		await Assert.ThrowsAsync<ArgumentNullException>(() => _sut.UpdateCategory(null));
	}

	private delegate void OutDelegate<in TIn, TOut>(TIn input, out TOut output);
}
