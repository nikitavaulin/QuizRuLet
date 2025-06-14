using System;
using System.Reflection;
using QuizRuLet.Core.Abstractions;
using QuizRuLet.Core;
using System.Globalization;
using QuizRuLet.Core.Models;

namespace QuizRuLet.Application.Services;

public class ModuleService : IModuleService
{
    private readonly IModulesRepository _modulesRepository;
    private readonly ICardsRepository _cardsRepository;

    public ModuleService(IModulesRepository modulesRepository, ICardsRepository cardsRepository)
    {
        _cardsRepository = cardsRepository;
        _modulesRepository = modulesRepository;
    }

    public async Task<List<Core.Models.Module>> GetAllModules()
    {
        return await _modulesRepository.Get();
    }

    public async Task<List<Core.Models.Module>> GetUserModules(Guid userId)
    {
        return await _modulesRepository.GetByUser(userId);
    }

    public async Task<Core.Models.Module?> GetModuleById(Guid id)
    {
        return await _modulesRepository.GetById(id);
    }

    public async Task<Guid> CreateModule(Core.Models.Module module, Guid userId)
    {
        return await _modulesRepository.Create(module, userId);
    }

    public async Task<Guid> UpdateModuleName(Guid id, string name)  // VALID TODO
    {
        var res = await _modulesRepository.UpdateName(id, name);
        return res;
    }
    
    public async Task<Guid> UpdateModuleDescription(Guid id, string description)
    {
        return await _modulesRepository.UpdateDescription(id, description);
    }
    
    public async Task<Guid> UpdateModule(Guid id, string name, string description)
    {
        return await _modulesRepository.Update(id, name, description);
    }

    // добавление сета карточек в модуль
    public async Task<Guid> AddCardsToModule(List<Card> cards, Guid moduleId)
    {
        foreach (var card in cards)
        {
            await AddCardToModule(card, moduleId);
        }

        return moduleId;
    }

    // добавление карточки в модуль
    public async Task<Guid> AddCardToModule(Card card, Guid moduleId)
    {
        return await _cardsRepository.Create(card, moduleId);
    }

    // удаление модуля
    public async Task<Guid> DeleteModule(Guid moduleId)
    {
        await _cardsRepository.DeleteByModule(moduleId);
        return await _modulesRepository.Delete(moduleId);
    }

    // количество всех карточек
    public async Task<int> GetCountCards(Guid moduleId)
    {
        return await _cardsRepository.GetCountCardsInModule(moduleId);
    }


}
