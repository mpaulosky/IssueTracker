namespace IssueTracker.Library.Fixtures;

[ExcludeFromCodeCoverage]
public static class TestCategories
{
	public static CategoryModel GetKnownCategory()
	{
		var status = new CategoryModel
		{
			Id = "5dc1039a1521eaa36835e541",
			CategoryDescription = "New Category",
			CategoryName = "New"
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
			new() { Id = "5dc1039a1521eaa36835e541", CategoryDescription = "An Issue with the design.", CategoryName = "Design" },
			new() { Id = "5dc1039a1521eaa36835e542", CategoryDescription = "An Issue with the documentation.", CategoryName = "Documentation" },
			new() { Id = "5dc1039a1521eaa36835e543", CategoryDescription = "An Issue with the implementation.", CategoryName = "Implementation" },
			new() { Id = "5dc1039a1521eaa36835e544", CategoryDescription = "A quick Issue with a general question.", CategoryName = "Clarification" },
			new() { Id = "5dc1039a1521eaa36835e545", CategoryDescription = "Not sure where this fits.", CategoryName = "Miscellaneous" }
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
			Id = "5dc1039a1521eaa36835e541",
			CategoryDescription = "Updated New Category",
			CategoryName = "New"
		};

		return status;
	}
}