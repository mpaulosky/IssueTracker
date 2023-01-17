//-----------------------------------------------------------------------
// <copyright file="MyInputRadioGroup.cs" company="mpaulosky">
//		Author:  Matthew Paulosky
//		Copyright (c) 2022. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

using Microsoft.AspNetCore.Components.Forms;

namespace IssueTracker.UI.Components;

public class MyInputRadioGroup<TValue> : InputRadioGroup<TValue>
{
	private string _fieldClass;
	private string _name;

	protected override void OnParametersSet()
	{
		var fieldClass = EditContext?.FieldCssClass(FieldIdentifier) ?? string.Empty;

		if (fieldClass == _fieldClass && Name == _name) return;
		_fieldClass = fieldClass;
		_name = Name;
		base.OnParametersSet();
	}
}