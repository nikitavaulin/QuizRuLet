using System;
using QuizRuLet.Core.Abstractions;
using QuizRuLet.Core.Models;

namespace QuizRuLet.Application.Services;

public class ModuleProgressService : IModuleProgressService
{
    private readonly IModulesRepository _modulesRepository;     // репозиторий модулей
    private readonly IModuleService _moduleService;             // сервис по работе с модулями
    private readonly ICardsRepository _cardsRepository;         // репозиторий карточек

    // Внедрение зависимостей
    public ModuleProgressService(IModulesRepository modulesRepository, ICardsRepository cardsRepository, IModuleService moduleService)
    {
        _modulesRepository = modulesRepository;
        _cardsRepository = cardsRepository;
        _moduleService = moduleService; 
    }

    // Получение процента прогресса изучения модуля
    public async Task<int> GetModuleProgressPercent(Guid moduleId)
    {
        double countCards = await _moduleService.GetCountCards(moduleId);                   // общее количество карточек в модуле
        double countLearned = await GetCountCardsByLearningFlagInModule(moduleId, true);    // количество изученных карточек в модуле
        double progressPercent = countLearned / countCards;                                 // процент прогресса
        return (int)(progressPercent * 100);                                                // процент прогресса изучения в виде целого числа
    }

    // Получение количества карточек в модуле по флагу изученности
    public async Task<int> GetCountCardsByLearningFlagInModule(Guid moduleId, bool isLearned)
    {
        return await _cardsRepository.GetCountCardsByLearningFlagInModule(moduleId, isLearned);
    }
    
    // Получение статистической информации о модуле
    public async Task<(int Progress, int CountCards, int CountLearned)> GetModuleStatisticInfo(Guid moduleId)
    {
        var progress = await GetModuleProgressPercent(moduleId);                            // процент прогресса изучения модуля
        var countLearned = await GetCountCardsByLearningFlagInModule(moduleId, true);       // количество изученных карточек
        var countNotLearned = await GetCountCardsByLearningFlagInModule(moduleId, false);   // количество неизученных карточек
        var countCards = countLearned + countNotLearned;                                    // количество карточек в модуле
        return (progress, countCards, countLearned);
    }
}