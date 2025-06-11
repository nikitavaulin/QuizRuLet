using System;
using System.Collections;
using QuizRuLet.Core.Abstractions;
using QuizRuLet.Core.Models;


namespace QuizRuLet.Application.Services;

public class LearningModuleService : ILearningModuleService
{
    private readonly ICardsRepository _cardsRepository;

    public LearningModuleService(ICardsRepository cardsRepository)
    {
        _cardsRepository = cardsRepository;
    }

    public async Task<Guid> UpdateLearningFlag(Guid cardId, bool isLearned)
    {
        return await _cardsRepository.UpdateLearningFlag(cardId, isLearned);
    }
    
    public async Task<Guid> UpdateLearningFlagInModule(Guid moduleId, bool isLearned)
    {
        return await _cardsRepository.UpdateLearningFlagInModule(moduleId, isLearned);
        // return Guid.NewGuid();
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
