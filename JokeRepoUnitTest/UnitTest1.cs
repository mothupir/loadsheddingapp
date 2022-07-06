using loadsheddingapp.Models;
using loadsheddingapp.Repository;
using Microsoft.EntityFrameworkCore;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;
using loadsheddingapp.Services;
using Microsoft.Extensions.Configuration;

namespace JokeRepoUnitTest
{
    public class Tests
    {
        private string connectionString = "";
        private DataContext? dataContext;
        private IJokeRepository jokeRepo;

        [SetUp]
        public void Setup()
        {

            var builder = new ConfigurationBuilder();

            builder.AddInMemoryCollection(new Dictionary<string, string>
                {
                     { "DatabaseSecretID", "dbKey" }
                });
            dataContext = new DataContext(new SecretsManagerService(), builder.Build());
            jokeRepo = new JokeRepository(dataContext);
        }

        [Test]
        public void AddJokeTest()
        {
            Task<Joke> task = null;
            var joke = new Joke()
            {
                Username = "user",
                Body = "This is my joke",
                IsApproved = true,
                TimeCreated = System.DateTime.Now
            };

            try
            {
                task = jokeRepo.AddAsync(joke);
            }
            catch (Exception e)
            {
                Assert.Fail(e.Message);
            }

            task.Wait();
            joke = task.Result;

            if (joke == null)
            {
                Assert.Fail("returned joke is null");
            }

            Assert.Pass($"joke added: {joke.ToString()}");
        }

        [Test]
        public void GetAllJokesTest()
        {
            Task<IEnumerable<Joke>> task = null;
            try
            {
                task = jokeRepo.GetAllAsync();
            }
            catch (Exception e)
            {
                Assert.Fail(e.Message);
            }

            task.Wait();
            List<Joke> list = task.Result.ToList();

            if (list == null)
            {
                Assert.Fail("List is null");
            }

            Assert.Pass($"List length {list.Count()}");
        }

        [Test]
        public void UpdateJokeTest()
        {
            Task<Joke?> task = null;
            var joke = new Joke()
            {
                Username = "updatedUser",
                Body = "This is my new joke",
                IsApproved = false,
                TimeCreated = System.DateTime.Now
            };
            int id = 1;

            try
            {
                task = jokeRepo.UpdateAsync(id, joke);
            }
            catch (Exception e)
            {
                Assert.Fail(e.Message);
            }

            task?.Wait();
            Joke? updatedJoke = task?.Result;

            if (updatedJoke == null)
            {
                Assert.Fail($"Joke with id {id} not found");
            }

            Assert.Pass($"joke updated: {updatedJoke}");
        }

        [Test]
        public void GetAllApprovedTest()
        {
            Task<IEnumerable<Joke>> task = null;

            try
            {
                task = jokeRepo.GetAllApproved();

            }
            catch (Exception e)
            {
                Assert.Fail(e.Message);
            }


            task?.Wait();
            var approvedJokes = task?.Result.ToList();

            if (approvedJokes != null && approvedJokes.Any())
            {
                approvedJokes.ForEach(joke =>
                {
                    if (!joke.IsApproved)
                    {
                        Assert.Fail("Item found which was not approved");
                    }
                });
            }

            Assert.Pass($"Jokes all approved, List size = {approvedJokes?.Count}");
        }
    }
    }
}