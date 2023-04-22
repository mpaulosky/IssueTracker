namespace IssueTracker.UseCases.Solution.Interfaces;

public interface IViewSolutionByIdUseCase
{
	Task<SolutionModel?> ExecuteAsync(string? solutionId);

}
