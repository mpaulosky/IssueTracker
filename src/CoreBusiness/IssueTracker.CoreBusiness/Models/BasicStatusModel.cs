//-----------------------------------------------------------------------
// <copyright File="BasicStatusModel.cs"
//	Company="mpaulosky">
//	Author: Matthew Paulosky
//	Copyright (c) 2022. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace IssueTracker.CoreBusiness.Models;

[Serializable]
public class BasicStatusModel
{

	public BasicStatusModel()
	{
	}

	public BasicStatusModel(StatusModel status)
	{

		StatusName = status.StatusName;
		StatusDescription = status.StatusDescription;

	}

	public BasicStatusModel(string statusName, string statusDescription) : this()
	{

		StatusName = statusName!;
		StatusDescription = statusDescription!;

	}

	public string StatusName { get; init; } = string.Empty;

	public string StatusDescription { get; init; } = string.Empty;

}