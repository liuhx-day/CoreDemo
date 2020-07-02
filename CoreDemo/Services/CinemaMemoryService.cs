﻿using CoreDemo.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CoreDemo.Services
{
    public class CinemaMemoryService:ICinemaService
    {
        private readonly List<Cinema> _cinemas = new List<Cinema>();
        public CinemaMemoryService()
        {
            _cinemas.Add(new Cinema
            {
                Id = 1,
                Name = "City Cinema",
                Location = "Road ABC, NO.123",
                Capacity = 1000
            }); 
            _cinemas.Add(new Cinema
            {
                Id=2,
                Name = "Fly Cinema",
                Location = "Road Hello, No.1024",
                Capacity = 500
            });
        }
        public Task<IEnumerable<Cinema>> GetllAllAsync()
        {
            return Task.Run(() => _cinemas.AsEnumerable());
        }
        public Task<Cinema> GetByIdAsync(int id)
        {
            return Task.Run(() => _cinemas.FirstOrDefault(x => x.Id == id));
        }
/*        public Task<Sales> GetSalesAsync()
        {
            throw new NotImplementedException();
        }*/
        public Task AddAsync(Cinema model)
        {
            var maxId = _cinemas.Max(x => x.Id);
            model.Id = maxId + 1;
            _cinemas.Add(model);
            return Task.CompletedTask;
        }
        public async Task<bool> DeleteByIdAsync(int id)
        {
            Cinema cimema = await GetByIdAsync(id);
            return await Task.Run(() =>
            {
                _cinemas.Remove(cimema);
                return true;
            });
        }

    }
}
