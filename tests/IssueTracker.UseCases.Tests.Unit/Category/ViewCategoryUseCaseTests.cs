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
	[InlineData(null, "categoryId", "Value cannot be null.?*")]
	[InlineData("", "categoryId", "The value cannot be an empty string.?*")]
	public async Task ExecuteAsync_WithInValidData_ShouldReturnArgumentException_TestAsync(string? expectedId, string expectedParamName, string expectedMessage)
	{
		// Arrange
		var sut = CreateUseCase(null);

		// Act
		Func<Task> act = async () => { await sut.ExecuteAsync(expectedId); };

		// Assert
		await act.Should()
			.ThrowAsync<ArgumentException>()
			.WithParameterName(expectedParamName)
			.WithMessage(expectedMessage);

	}

}
