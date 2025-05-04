using System;
using System.Collections;
using QuizRuLet.Core.Abstractions;
using QuizRuLet.Core.Models;


namespace QuizRuLet.Application.Services;

public class LearningModuleService
{
    private readonly IModulesRepository _modulesRepository;
    private readonly IModuleService _moduleService;
    private readonly ICardsRepository _cardsRepository;
    
    public LearningModuleService(IModulesRepository modulesRepository, ICardsRepository cardsRepository, IModuleService moduleService)
    {
        _modulesRepository = modulesRepository;
        _cardsRepository = cardsRepository;
        _moduleService = moduleService;
    }
    
    public async Task<Guid> UpdateLearningFlag(Guid cardId, bool isLearned)
    {
        return await _cardsRepository.UpdateLearningFlag(cardId, isLearned);
    }
        
    public async Task<List<Card>> GetCards(Guid moduleId, bool isLearned)
    {
        return await _cardsRepository.GetByLearningFlag(moduleId, isLearned);
    }
    
    public async Task<List<Card>> GetAllCards(Guid moduleId)
    {
        return await _cardsRepository.GetByModule(moduleId);
    }
    
}
