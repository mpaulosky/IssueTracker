
namespace IssueTracker.UseCases.Tests.Unit.Category;

[ExcludeFromCodeCoverage]
public class ViewCategoriesUseCaseTests
{

	private readonly Mock<ICategoryRepository> _categoryRepositoryMock;

	public ViewCategoriesUseCaseTests()
	{

		_categoryRepositoryMock = new Mock<ICategoryRepository>();

	}

	private ViewCategoriesUseCase CreateUseCase()
	{

		_categoryRepositoryMock.Setup(x => x.GetCategoriesAsync())
			.ReturnsAsync(FakeCategory.GetCategories(1));

		return new ViewCategoriesUseCase(_categoryRepositoryMock.Object);

	}

	[Fact(DisplayName = "ViewCategoryUseCase With Valid Data Test")]
	public async Task ExecuteAsync_WithValidData_ShouldReturnValidData_TestAsync()
	{

		// Arrange
		var _sut = CreateUseCase();

		// Act
		var result = await _sut.ExecuteAsync();

		// Assert
		result.Should().NotBeNull();
		result.First().Id.Should().NotBeNull();
		result.First().CategoryName.Should().NotBeNull();
		result.First().CategoryDescription.Should().NotBeNull();

		_categoryRepositoryMock.Verify(x =>
				x.GetCategoriesAsync(), Times.Once);

	}

}
