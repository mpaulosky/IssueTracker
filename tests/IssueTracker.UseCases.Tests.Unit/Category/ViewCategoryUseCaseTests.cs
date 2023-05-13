namespace IssueTracker.UseCases.Category;

[ExcludeFromCodeCoverage]
public class ViewCategoryUseCaseTests
{

	private readonly Mock<ICategoryRepository> _categoryRepositoryMock;

	public ViewCategoryUseCaseTests()
	{

		_categoryRepositoryMock = new Mock<ICategoryRepository>();

	}

	private ViewCategoryUseCase CreateUseCase(CategoryModel? expected)
	{

		if (expected != null)
		{

			_categoryRepositoryMock.Setup(x => x.GetAsync(It.IsAny<string>()))
				.ReturnsAsync(expected);

		}

		return new ViewCategoryUseCase(_categoryRepositoryMock.Object);

	}

	[Fact(DisplayName = "ViewCategoryByIdUseCase With Valid Data Test")]
	public async Task ExecuteAsync_WithValidData_ShouldReturnValidData_TestAsync()
	{

		// Arrange
		CategoryModel expected = FakeCategory.GetCategories(1).First();

		var sut = CreateUseCase(expected);

		var categoryId = expected.Id;

		// Act
		var result = await sut.ExecuteAsync(categoryId);

		// Assert
		result.Should().NotBeNull();
		result!.Id.Should().Be(expected.Id);
		result.CategoryName.Should().Be(expected.CategoryName);
		result.CategoryDescription.Should().Be(expected.CategoryDescription);

		_categoryRepositoryMock.Verify(x =>
				x.GetAsync(It.IsAny<string>()), Times.Once);
	}

	[Theory(DisplayName = "ViewCategoryByIdUseCase With In Valid Data Test")]
	[InlineData(null)]
	[InlineData("")]
	public async Task ExecuteAsync_WithInValidData_ShouldReturnValidData_TestAsync(string? expectedId)
	{
		// Arrange
		var sut = CreateUseCase(null);

		// Act
		// Assert
		switch (expectedId)
		{
			case null:
				_ = await Assert.ThrowsAsync<ArgumentNullException>(() => sut.ExecuteAsync(expectedId!));
				break;
			case "":
				_ = await Assert.ThrowsAsync<ArgumentException>(() => sut.ExecuteAsync(expectedId));
				break;
		}
	}


}
