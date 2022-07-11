using loadsheddingapp.Models;
using Microsoft.EntityFrameworkCore;

namespace loadsheddingapp.Repository
{
    public class JokeRepository : IJokeRepository
    {
        private readonly DataContext _dataContext;

        public JokeRepository(DataContext dataContext)
        {
            _dataContext = dataContext;
        }
        public async Task<Joke> AddAsync(Joke joke)
        {
            await _dataContext.Jokes.AddAsync(joke);
            await _dataContext.SaveChangesAsync();
            return joke;
        }

        public async Task<IEnumerable<Joke>> GetAllApproved()
        {
            return await _dataContext.Jokes.Where(x => x.IsApproved).OrderByDescending(x => x.TimeCreated).ToListAsync();
        }

        public async Task<IEnumerable<Joke>> GetAllAsync()
        {
            return await _dataContext.Jokes.ToListAsync();
        }

        public async Task<Joke?> UpdateAsync(int id, Joke joke)
        {
            var foundJoke = await _dataContext.Jokes.FindAsync(id);
            if (foundJoke == null)
            {
                return null;
            }

            foundJoke.Username = joke.Username;
            foundJoke.Body = joke.Body;
            foundJoke.TimeCreated = joke.TimeCreated;
            foundJoke.IsApproved = joke.IsApproved;

            await _dataContext.SaveChangesAsync();
            return foundJoke;
        }
    }
}
