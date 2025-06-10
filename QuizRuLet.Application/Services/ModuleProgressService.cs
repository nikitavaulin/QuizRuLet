using System;
using QuizRuLet.Core.Abstractions;
using QuizRuLet.Core.Models;

namespace QuizRuLet.Application.Services;

public class ModuleProgressService : IModuleProgressService
{
    private readonly IModulesRepository _modulesRepository;
    private readonly IModuleService _moduleService;
    private readonly ICardsRepository _cardsRepository;

    public ModuleProgressService(IModulesRepository modulesRepository, ICardsRepository cardsRepository, IModuleService moduleService)
    {
        _modulesRepository = modulesRepository;
        _cardsRepository = cardsRepository;
        _moduleService = moduleService;
    }

    public async Task<int> GetModuleProgressPercent(Guid moduleId)
    {
        double countCards = await _moduleService.GetCountCards(moduleId);
        double countLearned = await GetCountCardsByLearningFlagInModule(moduleId, true);

        double progressPercent = countLearned / countCards;


        return (int)(progressPercent * 100);
    }

    public async Task<int> GetCountCardsByLearningFlagInModule(Guid moduleId, bool isLearned)
    {
        return await _cardsRepository.GetCountCardsByLearningFlagInModule(moduleId, isLearned);
    }
    
    public async Task<(int Progress, int CountCards, int CountLearned)> GetModuleStatisticInfo(Guid moduleId)
    {
        var progress = await GetModuleProgressPercent(moduleId);
        var countLearned = await GetCountCardsByLearningFlagInModule(moduleId, true);
        var countNotLearned = await GetCountCardsByLearningFlagInModule(moduleId, false);

        var countCards = countLearned + countNotLearned;

        return (progress, countCards, countLearned);
    }
}
