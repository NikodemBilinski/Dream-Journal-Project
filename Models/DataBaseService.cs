using System;
using System.Collections.Generic;
using System.Text;
using SQLite;

namespace Dream_Journal_Project.Models
{
    public class DataBaseService
    {
        SQLiteAsyncConnection _database;

        async Task Init()
        {
            if (_database is not null)
            {
                return;
            }

            _database = new SQLiteAsyncConnection(Constants.DatabasePath, Constants.Flags);
            await _database.CreateTableAsync<Dream>();

        }

        public async Task<List<Dream>> GetDreams()
        {
            await Init();
            return await _database.Table<Dream>().ToListAsync();
        }

        public async Task AddDream(Dream dream)
        {
            await Init();
            if (dream.Id != 0)
            {
                await _database.UpdateAsync(dream);

            }
            else
            {
                await _database.InsertAsync(dream);
            }
        }

        public async Task DeleteDream(Dream dream)
        {
            await Init();
            if(dream == null)
            {
                return;
            }

            _database.DeleteAsync(dream);
        }

        public async Task DeleteAllDreams()
        {
            await Init();

            await _database.DropTableAsync<Dream>();

            await _database.CreateTableAsync<Dream>();
        }
    }
}
