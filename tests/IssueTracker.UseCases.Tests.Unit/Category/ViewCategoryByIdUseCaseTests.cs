namespace IssueTracker.UseCases.Tests.Unit.Category;

[ExcludeFromCodeCoverage]
public class ViewCategoryByIdUseCaseTests
{

	private readonly Mock<ICategoryRepository> _categoryRepositoryMock;

	public ViewCategoryByIdUseCaseTests()
	{

		_categoryRepositoryMock = new Mock<ICategoryRepository>();

	}

	private ViewCategoryByIdUseCase CreateUseCase(CategoryModel? expected)
	{

		if (expected != null)
		{

			_categoryRepositoryMock.Setup(x => x.GetCategoryByIdAsync(It.IsAny<string>()))
				.ReturnsAsync(expected);

		}

		return new ViewCategoryByIdUseCase(_categoryRepositoryMock.Object);

	}

	[Fact(DisplayName = "ViewCategoryByIdUseCase With Valid Data Test")]
	public async Task ExecuteAsync_WithValidData_ShouldReturnValidData_TestAsync()
	{

		// Arrange
		CategoryModel expected = FakeCategory.GetCategories(1).First();

		var _sut = CreateUseCase(expected);

		var categoryId = expected.Id;

		// Act
		var result = await _sut.ExecuteAsync(categoryId);

		// Assert
		result.Should().NotBeNull();
		result.Id.Should().Be(expected.Id);
		result.CategoryName.Should().Be(expected.CategoryName);
		result.CategoryDescription.Should().Be(expected.CategoryDescription);

		_categoryRepositoryMock.Verify(x =>
				x.GetCategoryByIdAsync(It.IsAny<string>()), Times.Once);
	}

	[Theory(DisplayName = "ViewCategoryByIdUseCase With In Valid Data Test")]
	[InlineData(null)]
	[InlineData("")]
	public async Task ExecuteAsync_WithInValidData_ShouldReturnValidData_TestAsync(string? expectedId)
	{

		// Arrange
		var _sut = CreateUseCase(null);

		// Act
		var result = await _sut.ExecuteAsync(expectedId);

		// Assert
		result.Should().NotBeNull();

		_categoryRepositoryMock.Verify(x =>
				x.GetCategoryByIdAsync(It.IsAny<string>()), Times.Never);

	}

}
