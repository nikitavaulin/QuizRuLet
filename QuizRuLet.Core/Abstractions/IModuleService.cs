using QuizRuLet.Core.Models;

namespace QuizRuLet.Core.Abstractions;

public interface IModuleService
{
    Task<Guid> AddCardsToModule(List<Card> cards, Guid moduleId);
    Task<Guid> AddCardToModule(Card card, Guid moduleId);
    Task<Guid> CreateModule(Module module, Guid userId);
    Task<Guid> DeleteModule(Guid moduleId);
    Task<List<Module>> GetAllModules();
    Task<int> GetCountCards(Guid moduleId);
    Task<Module?> GetModuleById(Guid id);
    Task<List<Module>> GetUserModules(Guid userId);
    Task<Guid> UpdateModule(Guid id, string name, string description);
}
