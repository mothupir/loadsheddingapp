using loadsheddingapp.Models;

namespace loadsheddingapp.Repository
{
    public interface IJokeRepository
    {
        Task<IEnumerable<Joke>> GetAllAsync();
        Task<IEnumerable<Joke>> GetAllApproved();
        Task<Joke> AddAsync(Joke joke);
        Task<Joke?> UpdateAsync(int id, Joke joke);
    }
}
