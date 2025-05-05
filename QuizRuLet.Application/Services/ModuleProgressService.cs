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

        double progressPercent = countCards / 100 * countLearned;


        return (int)(progressPercent * 100);
    }

    public async Task<int> GetCountCardsByLearningFlagInModule(Guid moduleId, bool isLearned)
    {
        return await _cardsRepository.GetCountCardsByLearningFlagInModule(moduleId, isLearned);
    }
}
