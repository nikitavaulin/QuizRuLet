namespace QuizRuLet.DataAccess.Repositories
{
    public class CardRepository
    {
        private readonly QuizRuLetDbContext _dbContext;

        public CardRepository(QuizRuLetDbContext context)
        {
            _dbContext = context;
        }
        
    }
}
