using System;
using System.Reflection;
using QuizRuLet.Core.Abstractions;
using QuizRuLet.Core;
using System.Globalization;
using QuizRuLet.Core.Models;

namespace QuizRuLet.Application.Services;

public class ModuleService : IModuleService
{
    private readonly IModulesRepository _modulesRepository;     // Репозиторий для работы с модулями
    private readonly ICardsRepository _cardsRepository;         // Репозиторий для работы с карточками

    // Внедрение зависимостей
    public ModuleService(IModulesRepository modulesRepository, ICardsRepository cardsRepository)
    {
        _cardsRepository = cardsRepository;
        _modulesRepository = modulesRepository;
    }

    // Получение всех модулей из БД
    public async Task<List<Core.Models.Module>> GetAllModules()
    {
        return await _modulesRepository.Get(); 
    }

    // Получение всех модулей конкретного пользователя
    public async Task<List<Core.Models.Module>> GetUserModules(Guid userId)
    {
        return await _modulesRepository.GetByUser(userId); 
    }

    // Получение модуля по ID
    public async Task<Core.Models.Module?> GetModuleById(Guid id)
    {
        return await _modulesRepository.GetById(id);
    }

    // Создание нового модуля
    public async Task<Guid> CreateModule(Core.Models.Module module, Guid userId)
    {
        return await _modulesRepository.Create(module, userId);
    }

    // Обновление названия модуля
    public async Task<Guid> UpdateModuleName(Guid id, string name)  
    {
        var res = await _modulesRepository.UpdateName(id, name);
        return res;
    }
    
    // Обновление описания модуля
    public async Task<Guid> UpdateModuleDescription(Guid id, string description)
    {
        return await _modulesRepository.UpdateDescription(id, description);
    }
    
    // Обновление названия и описания модуля
    public async Task<Guid> UpdateModule(Guid id, string name, string description)
    {
        return await _modulesRepository.Update(id, name, description);
    }

    // Добавление набора карточек в модуль
    public async Task<Guid> AddCardsToModule(List<Card> cards, Guid moduleId)
    {
        foreach (var card in cards)
        {
            await AddCardToModule(card, moduleId); 
        }
        return moduleId; 
    }

    // Добавление одной карточки в модуль
    public async Task<Guid> AddCardToModule(Card card, Guid moduleId)
    {
        return await _cardsRepository.Create(card, moduleId);
    }

    // Удаление модуля и его карточек
    public async Task<Guid> DeleteModule(Guid moduleId)
    {
        await _cardsRepository.DeleteByModule(moduleId);        // удаление карточек из БД
        return await _modulesRepository.Delete(moduleId);       // удаление модуля из БД
    }

    // Получение количества карточек в модуле
    public async Task<int> GetCountCards(Guid moduleId)
    {
        return await _cardsRepository.GetCountCardsInModule(moduleId);
    }
}