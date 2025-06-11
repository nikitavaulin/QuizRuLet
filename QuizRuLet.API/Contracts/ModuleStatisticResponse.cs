namespace QuizRuLet.API.Contracts
{
    public record ModuleStatisticResponse 
    (
        int Progress,
        int CountCards,
        int CountLearned,
        int CountNotLearned
    );
    
}
