﻿namespace IssueTracker.CoreBusiness.Contracts;

public interface IDatabaseSettings
{

	string ConnectionStrings { get; set; }

	string DatabaseName { get; set; }

}
