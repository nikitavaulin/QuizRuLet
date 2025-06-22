using System;
using QuizRuLet.Core.Models;
using QuizRuLet.Core.Abstractions;

namespace QuizRuLet.Application.Services;

public class CardService : ICardService
{
    private readonly ICardsRepository _cardsRepository;     // репозиторий карточек

    // Внедрение зависимостей
    public CardService(ICardsRepository cardsRepository)
    {
        _cardsRepository = cardsRepository; 
    }

    /// Получение всех карточек из базы данных
    public async Task<List<Card>> GetAllCards()
    {
        return await _cardsRepository.Get();
    }

    /// Получение всех карточек из базы данных
    public async Task<List<Card>> GetCardsByModule(Guid moduleId)
    {
        return await _cardsRepository.GetByModule(moduleId); 
    }

    /// Получение карточек с заданным флагом изученности в модуле
    public async Task<List<Card>> GetCardsByLearningFlag(Guid moduleId, bool isLearned)
    {
        return await _cardsRepository.GetByLearningFlag(moduleId, isLearned); 
    }

    /// Получение карточки по ID
    public async Task<Card?> GetCardById(Guid id)
    {
        return await _cardsRepository.GetById(id); 
    }

    /// Создание новой карточки и привязка её к модулю
    public async Task<Guid> CreateCard(Card card, Guid moduleId)
    {
        return await _cardsRepository.Create(card, moduleId); 
    }

    /// Обновление сторон карточки
    public async Task<Guid> UpdateCardSides(Guid id, string frontSide, string backSide)
    {
        return await _cardsRepository.UpdatePartly(id, frontSide, backSide);
    }

    /// Удаление карточки по ID
    public async Task<Guid> DeleteCard(Guid id)
    {
        return await _cardsRepository.Delete(id);
    }
}