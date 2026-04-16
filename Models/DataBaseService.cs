using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using SQLite;

namespace Dream_Journal_Project.Models
{
    public class DataBaseService
    {
        SQLiteAsyncConnection _database;

        public async Task Init()
        {
            if (_database is not null)
            {
                return;
            }

            _database = new SQLiteAsyncConnection(Constants.DatabasePath, Constants.Flags);
            await _database.CreateTableAsync<Dream>();
            await _database.CreateTableAsync<Tag>();

        }

        public async Task CloseConnection()
        {
            if(_database is not null)
            {
                await _database.CloseAsync();
                _database = null;
            }
        }

        

        public async Task<bool> CheckForUpdates()
        {
            var http = "https://raw.githubusercontent.com/NikodemBilinski/Dream-Journal-Project/refs/heads/master/version.txt";

            string currentVersion = "0.7";

            try
            {

                var latestVersion = await new HttpClient().GetStringAsync(http);
                if (latestVersion.Trim() != currentVersion)
                {
                    // Notify user about the update
                    Debug.WriteLine("A new version of the app is available!");
                    
                    return true;
                }

            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Failed to check for updates: {ex.Message}");
                return false;
            }
            Debug.WriteLine("No updates available");
            return false;
        }

        public async Task<List<Dream>> GetDreams()
        {
            await Init();
            return await _database.Table<Dream>().OrderByDescending(x => x.DateCreated).ToListAsync();
        }

        public async Task<List<Tag>> GetTags()
        {
            await Init();
            return await _database.Table<Tag>().ToListAsync();
        }

        public async Task<List<Tag>> GetTagsById(string tagIds)
        {
            await Init();

            if(string.IsNullOrEmpty(tagIds))
            {
                return new List<Tag>();
            }

            var idArray = tagIds.Split(',', StringSplitOptions.TrimEntries);

            var allTags = await _database.Table<Tag>().ToListAsync();

            return allTags.Where(chuj => idArray.Contains(chuj.Id.ToString())).ToList();


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

            await _database.DeleteAsync(dream);
        }

        public async Task DeleteTag(Tag tag)
        {
            await Init();
            if (tag == null)
            {
                return;
            }
            await _database.DeleteAsync(tag);
        }

        public async Task GenerateDefaultTags()
        {
            await Init();

            var count = await _database.Table<Tag>().CountAsync();

            if(count != 0)
            {
                return;
            }

            await _database.InsertAsync(new Tag { Name = "Lucid", ColorHex = "#F5DEB3", IsActive = false });
            await _database.InsertAsync(new Tag { Name = "Nightmare", ColorHex = "#FF0000", IsActive = false });
            await _database.InsertAsync(new Tag { Name = "Vivid", ColorHex = "#FFFF00", IsActive = false });
            await _database.InsertAsync(new Tag { Name = "False Awakening", ColorHex = "#FF00FF", IsActive = false });
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


        // for settings

        public async Task DeleteAllTags()
        {
            await Init();
            await _database.DropTableAsync<Tag>();
            await _database.CreateTableAsync<Tag>();

            await GenerateDefaultTags();
        }

        public async Task DeleteAllDreams()
        {
            await Init();
            await _database.DropTableAsync<Dream>();
            await _database.CreateTableAsync<Dream>();

        }

        //debugging method, not used in the app
        public async Task temp()
        {
            await Init();
            var lucid = await _database.Table<Tag>().Where(t => t.Name == "Lucid").FirstOrDefaultAsync();

            await _database.DeleteAsync(lucid);

            await _database.InsertAsync(new Tag { Name = "Lucid", ColorHex = "#F5DEB3", IsActive = false });
        }


    }
}
