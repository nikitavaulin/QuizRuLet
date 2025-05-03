using System;
using System.Reflection;
using QuizRuLet.Core.Abstractions;
using QuizRuLet.Core;
using System.Globalization;

namespace QuizRuLet.Application.Services;

public class ModuleService
{
    private readonly IModulesRepository _modulesRepository;

    public ModuleService(IModulesRepository modulesRepository)
    {
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
    
    public async Task<Guid> UpdateModule(Guid id, string name, string description, Guid userId)
    {
        return await _modulesRepository.Update(id, name, description, userId);
    }

    public async Task<Guid> DeleteModule(Guid id)
    {
        return await _modulesRepository.Delete(id);
    }


}
