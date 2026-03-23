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
            await _database.CreateTableAsync<Tag>();

        }

        public async Task<List<Dream>> GetDreams()
        {
            await Init();
            return await _database.Table<Dream>().ToListAsync();
        }

        public async Task<List<Tag>> GetTags()
        {
            await Init();
            return await _database.Table<Tag>().ToListAsync();
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

        public async Task AddTag(Tag tag)
        {
            await Init();
            if (tag.Id != 0)
            {
                await _database.UpdateAsync(tag);
            }
            else
            {
                await _database.InsertAsync(tag);
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

        public async Task GenerateDefaultTags()
        {
            await Init();

            var count = await _database.Table<Tag>().CountAsync();

            if (count == 0)
            {
                await _database.InsertAsync(new Tag { Name = "Nightmare", ColorHex = "#FF0000", IsActive = true });
                await _database.InsertAsync(new Tag { Name = "Vivid", ColorHex = "#FFFF00", IsActive = true });
                await _database.InsertAsync(new Tag { Name = "False Awakening", ColorHex = "#FFFFFF", IsActive = true });

            }
        }

        public async Task DeleteAllDreams()
        {
            await Init();

            await _database.DropTableAsync<Dream>();

            await _database.CreateTableAsync<Dream>();
        }

        public async Task<Dream> GetSpecificDream(int Dreamid)
        {

            await Init();

            return await _database.Table<Dream>()
                                  .Where(d => d.Id == Dreamid)
                                  .FirstOrDefaultAsync();
                
        }

        public async Task UpdateDream(Dream dream)
        {
            await Init();

            await _database.UpdateAsync(dream);
        }
    }
}
