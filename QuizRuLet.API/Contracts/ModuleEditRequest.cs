namespace QuizRuLet.API.Contracts
{
    public record ModuleNameEditRequest 
    (
        string Name
    );
    
    public record ModuleDescriptionEditRequest 
    (
        string Description
    );
    
    public record ModuleEditRequest 
    (
        string Name,
        string Description
    );
}
