// Copyright (c) 2023. All rights reserved.
// File Name :     MyInputRadioGroup.cs
// Company :       mpaulosky
// Author :        Matthew Paulosky
// Solution Name : IssueTracker
// Project Name :  IssueTracker.UI

using Microsoft.AspNetCore.Components.Forms;

namespace IssueTracker.UI.Components;

public class MyInputRadioGroup<TValue> : InputRadioGroup<TValue>
{
	private string? _fieldClass;
	private string? _name;

	protected override void OnParametersSet()
	{
		string fieldClass = EditContext.FieldCssClass(FieldIdentifier);

		if (fieldClass == _fieldClass && Name == _name)
		{
			return;
		}

		_fieldClass = fieldClass;
		_name = Name;
		base.OnParametersSet();
	}
}