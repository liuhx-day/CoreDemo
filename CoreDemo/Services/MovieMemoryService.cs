﻿using CoreDemo.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CoreDemo.Services
{
    public class MovieMemoryService:IMovieService
    {
        private readonly List<Movie> _movies = new List<Movie>();
        public MovieMemoryService()
        {
            _movies.Add(new Movie
            {
                CinemaId = 1,
                Id=1,
                Name="Superman",
                ReleaseDate=new DateTime(2020,6,30),
                Starring="Nick"
            });
            _movies.Add(new Movie
            {
                CinemaId = 1,
                Id = 2,
                Name = "Ghost",
                ReleaseDate = new DateTime(1997, 5, 4),
                Starring = "Michael Jackson"
            });
            _movies.Add(new Movie
            {
                CinemaId = 2,
                Id = 3,
                Name="Fight",
                ReleaseDate=new DateTime(2020,7,1),
                Starring="Tommy"
            });
        }
        public Task AddAsync(Movie model)
        {
            var maxId = _movies.Max(x => x.Id);
            model.Id = maxId + 1;
            _movies.Add(model);
            return Task.CompletedTask;
        }
        public Task<IEnumerable<Movie>> GetByCinemaAsync(int cinemaId)
        {
            return Task.Run(() => _movies.Where(x => x.CinemaId == cinemaId));
        }
        public Task<Movie> GetByIdAsync(int id)
        {
            return Task.Run(() => _movies.FirstOrDefault(x => x.Id == id));
        }
        public async Task<bool> DeleteByIdAsync(int id)
        {
            Movie movie = await GetByIdAsync(id);
            return await Task.Run(() =>
            {
                _movies.Remove(movie);
                return true;
            });
        }
    }
}
