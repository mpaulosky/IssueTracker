
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

		_categoryRepositoryMock.Setup(x => x.GetAllAsync(false))
			.ReturnsAsync(FakeCategory.GetCategories(1));

		return new ViewCategoriesUseCase(_categoryRepositoryMock.Object);

	}

	[Fact(DisplayName = "ViewCategoryUseCase With Valid Data Test")]
	public async Task ExecuteAsync_WithValidData_ShouldReturnValidData_TestAsync()
	{

		// Arrange
		var sut = CreateUseCase();

		// Act
		CategoryModel result = (await sut.ExecuteAsync())!.First();

		// Assert
		result.Should().NotBeNull();
		result.Id.Should().NotBeNull();
		result.CategoryName.Should().NotBeNull();
		result.CategoryDescription.Should().NotBeNull();

		_categoryRepositoryMock.Verify(x =>
				x.GetAllAsync(false), Times.Once);

	}

}
