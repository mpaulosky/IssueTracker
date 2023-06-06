namespace IssueTracker.UseCases.Solution.Interfaces;

public interface IViewSolutionUseCase
{
	Task<SolutionModel?> ExecuteAsync(string solutionId);

}
