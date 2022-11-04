using Radzen.Blazor;

namespace IssueTracker.UI.Pages;

/// <summary>
///  Categories partial class
/// </summary>
public partial class Categories
{

	private RadzenDataGrid<CategoryModel> _categoriesGrid;
	private List<CategoryModel> _categories = new();
	private CategoryModel _categoryToInsert;
	private CategoryModel _categoryToUpdate;


	/// <summary>
	///		OnInitializedAsync event.
	/// </summary>
	protected override async Task OnInitializedAsync()
	{

		_categories = await CategoryService.GetCategories();

	}

	private async Task EditRow(CategoryModel category)
	{

		_categoryToUpdate = category;

		await _categoriesGrid.EditRow(category);

	}

	private async void OnUpdateRow(CategoryModel category)
	{

		if (category == _categoryToInsert)
		{

			_categoryToInsert = null;

		}

		_categoryToUpdate = null;

		await CategoryService.UpdateCategory(category);

	}

	private async Task SaveRow(CategoryModel category)
	{

		await _categoriesGrid.UpdateRow(category);

	}

	private void CancelEdit(CategoryModel category)
	{

		if (category == _categoryToInsert)
		{

			_categoryToInsert = null;

		}

		if (category == _categoryToUpdate)
		{

			_categoryToUpdate = null;

		}

		_categoriesGrid.CancelEditRow(category);

	}

	private async Task DeleteRow(CategoryModel category)
	{

		if (category == _categoryToInsert)
		{

			_categoryToInsert = null;

		}

		if (category == _categoryToUpdate)
		{

			_categoryToUpdate = null;

		}

		if (_categories.Contains(category))
		{

			_categories.Remove(category);

			await CategoryService.DeleteCategory(category);

			await _categoriesGrid.Reload();

		}
		else
		{

			_categoriesGrid.CancelEditRow(category);

			await CategoryService.DeleteCategory(category);

			await _categoriesGrid.Reload();

		}

	}


	private async Task InsertRow()
	{

		_categoryToInsert = new CategoryModel();

		await _categoriesGrid.InsertRow(_categoryToInsert);

	}

	private async void OnCreateRow(CategoryModel category)
	{

		if (category == _categoryToInsert) { _categoryToInsert = null; }

		await CategoryService.CreateCategory(category);

		_categories.Add(category);

		await _categoriesGrid.Reload();

	}

	/// <summary>
	///		ClosePage method.
	/// </summary>
	private void ClosePage()
	{

		NavManager.NavigateTo("/");

	}

}