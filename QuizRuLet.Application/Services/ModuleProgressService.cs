using System;
using QuizRuLet.Core.Abstractions;
using QuizRuLet.Core.Models;

namespace QuizRuLet.Application.Services;


public class ModuleProgressService
{
    private readonly IModulesRepository _modulesRepository;
    private readonly ICardsRepository _cardsRepository;
    
    public ModuleProgressService(IModulesRepository modulesRepository, ICardsRepository cardsRepository)
    {
        _modulesRepository = modulesRepository;
        _cardsRepository = cardsRepository;
    }
    
    public async Task<int> GetModuleProgressPercent(Guid moduleId)
    {
        double countCards = await GetCountCards(moduleId);
        double countLearned = await GetCountCardsByLearningFlagInModule(moduleId, true);
        
        double progressPercent = countCards / 100 * countLearned;
        
        
        return (int)(progressPercent * 100);
    }   
    
    public async Task<int> GetCountCards(Guid moduleId)
    {
        return await _cardsRepository.GetCountCardsInModule(moduleId);
    }
    
    public async Task<int> GetCountCardsByLearningFlagInModule(Guid moduleId, bool isLearned)
    {
        return await _cardsRepository.GetCountCardsByLearningFlagInModule(moduleId, isLearned);
    }
}
