﻿//-----------------------------------------------------------------------
// <copyright>
//	File:		IArchiveUserUseCase.cs
//	Company:mpaulosky
//	Author:	Matthew Paulosky
//	Copyright (c) 2022. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace IssueTracker.UseCases.Users.Interfaces;

public interface IArchiveUserUseCase
{
	Task ExecuteAsync(UserModel user);
}
