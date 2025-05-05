namespace QuizRuLet.Core.Abstractions;

public interface IModuleProgressService
{
    Task<int> GetCountCardsByLearningFlagInModule(Guid moduleId, bool isLearned);
    Task<int> GetModuleProgressPercent(Guid moduleId);
}
