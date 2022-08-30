namespace IssueTracker.Library.Models;

[Serializable]
public class BasicCategoryModel
{
	public BasicCategoryModel()
	{
	}

	public BasicCategoryModel(CategoryModel category)
	{
		CategoryName = category?.CategoryName;
		CategoryDescription = category?.CategoryDescription;
	}
	
	public string CategoryName { get; init; }

	public string CategoryDescription { get; init; }
}