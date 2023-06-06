//-----------------------------------------------------------------------
// <copyright>
//	File:		IArchiveStatusUseCase.cs
//	Company:mpaulosky
//	Author:	Matthew Paulosky
//	Copyright (c) 2022. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace IssueTracker.UseCases.Status.Interfaces;

public interface IArchiveStatusUseCase
{
	Task ExecuteAsync(StatusModel? status);

}
