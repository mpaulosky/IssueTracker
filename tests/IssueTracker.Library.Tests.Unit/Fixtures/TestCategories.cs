namespace IssueTracker.Library.Tests.Unit.Fixtures;

[ExcludeFromCodeCoverage]
public static class TestCategories
{
	public static CategoryModel GetKnownCategory()
	{
		var status = new CategoryModel
		{
			Id = "5dc1039a1521eaa36835e541", CategoryDescription = "New Category", CategoryName = "New"
		};

		return status;
	}

	public static CategoryModel GetCategory(string id, string statusDescription, string statusName)
	{
		var status = new CategoryModel { Id = id, CategoryDescription = statusDescription, CategoryName = statusName };

		return status;
	}

	public static IEnumerable<CategoryModel> GetCategories()
	{
		var statuses = new List<CategoryModel>
		{
			new() { Id = Guid.NewGuid().ToString(), CategoryDescription = "New Category", CategoryName = "New" },
			new() { Id = Guid.NewGuid().ToString(), CategoryDescription = "New Category", CategoryName = "New" },
			new() { Id = Guid.NewGuid().ToString(), CategoryDescription = "New Category", CategoryName = "New" }
		};

		return statuses;
	}

	public static CategoryModel GetNewCategory()
	{
		var status = new CategoryModel { CategoryDescription = "New Category", CategoryName = "New" };

		return status;
	}

	public static CategoryModel GetUpdatedCategory()
	{
		var status = new CategoryModel
		{
			Id = "5dc1039a1521eaa36835e541", CategoryDescription = "Updated New Category", CategoryName = "New"
		};

		return status;
	}}