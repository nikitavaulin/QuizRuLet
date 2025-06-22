using System;
using System.Collections;
using QuizRuLet.Core.Abstractions;
using QuizRuLet.Core.Models;

namespace QuizRuLet.Application.Services;

public class LearningModuleService : ILearningModuleService
{
    private readonly ICardsRepository _cardsRepository;      // репозиторий карточек

    // Внедрение зависимостей
    public LearningModuleService(ICardsRepository cardsRepository)
    {
        _cardsRepository = cardsRepository;
    }

    // Обновление флага изученности конкретной карточки
    public async Task<Guid> UpdateLearningFlag(Guid cardId, bool isLearned)
    {
        return await _cardsRepository.UpdateLearningFlag(cardId, isLearned);
    }
    
    // Обновление флага изученности всех карточек в модуле
    public async Task<Guid> UpdateLearningFlagInModule(Guid moduleId, bool isLearned)
    {
        return await _cardsRepository.UpdateLearningFlagInModule(moduleId, isLearned);
    }

    // Получение списка карточек с заданным флагом изученности в модуле
    public async Task<List<Card>> GetCards(Guid moduleId, bool isLearned)
    {
        return await _cardsRepository.GetByLearningFlag(moduleId, isLearned);
    }

    // Получение всех карточек в модуле
    public async Task<List<Card>> GetAllCards(Guid moduleId)
    {
        return await _cardsRepository.GetByModule(moduleId);
    }
}