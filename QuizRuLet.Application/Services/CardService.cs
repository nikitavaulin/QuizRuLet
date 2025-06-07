using System;
using QuizRuLet.Core.Models;
using QuizRuLet.Core.Abstractions;

namespace QuizRuLet.Application.Services;

public class CardService : ICardService
// удалить
{
    private readonly ICardsRepository _cardsRepository;

    public CardService(ICardsRepository cardsRepository)
    {
        _cardsRepository = cardsRepository;
    }

    public async Task<List<Card>> GetAllCards()
    {
        return await _cardsRepository.Get();
    }

    public async Task<List<Card>> GetCardsByModule(Guid moduleId)
    {
        return await _cardsRepository.GetByModule(moduleId);
    }

    public async Task<List<Card>> GetCardsByLearningFlag(Guid moduleId, bool isLearned)
    {
        return await _cardsRepository.GetByLearningFlag(moduleId, isLearned);
    }

    public async Task<Card?> GetCardById(Guid id)
    {
        return await _cardsRepository.GetById(id);  // VALIDATION?
    }

    public async Task<Guid> CreateCard(Card card, Guid moduleId)
    {
        return await _cardsRepository.Create(card, moduleId);
    }

    public async Task<Guid> UpdateCard(Guid id, string frontSide, string backSide, bool isLearned, Guid moduleId)
    {
        return await _cardsRepository.Update(id, frontSide, backSide, isLearned, moduleId);
    }

    public async Task<Guid> DeleteCard(Guid id)
    {
        return await _cardsRepository.Delete(id);
    }



}
