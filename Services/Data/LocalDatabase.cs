using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using SQLite;
using Pathway.Models;

namespace Pathway.Services.Data
{
    public class LocalDatabase : ILocalDatabase
    {
        private readonly SQLiteAsyncConnection _database;

        public LocalDatabase(string dbPath)
        {
            _database = new SQLiteAsyncConnection(dbPath);
            InitializeAsync().ConfigureAwait(false);
        }

        private async Task InitializeAsync()
        {
            // Create tables for all our models
            await _database.CreateTableAsync<Plant>();
            await _database.CreateTableAsync<Area>();
            await _database.CreateTableAsync<MaintenanceTask>();
            await _database.CreateTableAsync<PlantSpecies>();
            await _database.CreateTableAsync<WorkLog>();
            await _database.CreateTableAsync<User>();
        }

        #region Plant Methods

        public async Task<List<Plant>> GetAllPlantsAsync()
        {
            return await _database.Table<Plant>().ToListAsync();
        }

        public async Task<Plant> GetPlantByIdAsync(string id)
        {
            return await _database.Table<Plant>().Where(p => p.Id == id).FirstOrDefaultAsync();
        }

        public async Task<List<Plant>> GetPlantsByAreaIdAsync(string areaId)
        {
            return await _database.Table<Plant>().Where(p => p.AreaId == areaId).ToListAsync();
        }

        public async Task<List<Plant>> GetPlantsModifiedSinceAsync(DateTime since)
        {
            return await _database.Table<Plant>()
                .Where(p => p.LastModifiedAt > since || !p.IsSynced)
                .ToListAsync();
        }

        public async Task<int> AddPlantAsync(Plant plant)
        {
            if (string.IsNullOrEmpty(plant.Id))
            {
                plant.Id = Guid.NewGuid().ToString();
            }

            if (plant.CreatedAt == default)
            {
                plant.CreatedAt = DateTime.UtcNow;
            }

            plant.LastModifiedAt = DateTime.UtcNow;

            return await _database.InsertAsync(plant);
        }

        public async Task<int> UpdatePlantAsync(Plant plant)
        {
            plant.LastModifiedAt = DateTime.UtcNow;
            return await _database.UpdateAsync(plant);
        }

        public async Task<int> DeletePlantAsync(Plant plant)
        {
            return await _database.DeleteAsync(plant);
        }

        #endregion

        #region Area Methods

        public async Task<List<Area>> GetAllAreasAsync()
        {
            return await _database.Table<Area>().ToListAsync();
        }

        public async Task<Area> GetAreaByIdAsync(string id)
        {
            return await _database.Table<Area>().Where(a => a.Id == id).FirstOrDefaultAsync();
        }

        public async Task<List<Area>> GetAreasModifiedSinceAsync(DateTime since)
        {
            return await _database.Table<Area>()
                .Where(a => a.LastModifiedAt > since || !a.IsSynced)
                .ToListAsync();
        }

        public async Task<int> AddAreaAsync(Area area)
        {
            if (string.IsNullOrEmpty(area.Id))
            {
                area.Id = Guid.NewGuid().ToString();
            }

            if (area.CreatedAt == default)
            {
                area.CreatedAt = DateTime.UtcNow;
            }

            area.LastModifiedAt = DateTime.UtcNow;

            return await _database.InsertAsync(area);
        }

        public async Task<int> UpdateAreaAsync(Area area)
        {
            area.LastModifiedAt = DateTime.UtcNow;
            return await _database.UpdateAsync(area);
        }

        public async Task<int> DeleteAreaAsync(Area area)
        {
            return await _database.DeleteAsync(area);
        }

        #endregion

        #region MaintenanceTask Methods

        public async Task<List<MaintenanceTask>> GetAllTasksAsync()
        {
            return await _database.Table<MaintenanceTask>().ToListAsync();
        }

        public async Task<List<MaintenanceTask>> GetTasksByAssigneeIdAsync(string assigneeId)
        {
            return await _database.Table<MaintenanceTask>().Where(t => t.AssigneeId == assigneeId).ToListAsync();
        }

        public async Task<List<MaintenanceTask>> GetTasksByAreaIdAsync(string areaId)
        {
            return await _database.Table<MaintenanceTask>().Where(t => t.AreaId == areaId).ToListAsync();
        }

        public async Task<List<MaintenanceTask>> GetTasksByPlantIdAsync(string plantId)
        {
            return await _database.Table<MaintenanceTask>().Where(t => t.PlantId == plantId).ToListAsync();
        }

        public async Task<List<MaintenanceTask>> GetTasksModifiedSinceAsync(DateTime since)
        {
            return await _database.Table<MaintenanceTask>()
                .Where(t => t.LastModifiedAt > since || !t.IsSynced)
                .ToListAsync();
        }

        public async Task<MaintenanceTask> GetTaskByIdAsync(string id)
        {
            return await _database.Table<MaintenanceTask>().Where(t => t.Id == id).FirstOrDefaultAsync();
        }

        public async Task<int> AddTaskAsync(MaintenanceTask task)
        {
            if (string.IsNullOrEmpty(task.Id))
            {
                task.Id = Guid.NewGuid().ToString();
            }

            if (task.CreatedAt == default)
            {
                task.CreatedAt = DateTime.UtcNow;
            }

            task.LastModifiedAt = DateTime.UtcNow;

            return await _database.InsertAsync(task);
        }

        public async Task<int> UpdateTaskAsync(MaintenanceTask task)
        {
            task.LastModifiedAt = DateTime.UtcNow;
            return await _database.UpdateAsync(task);
        }

        public async Task<int> DeleteTaskAsync(MaintenanceTask task)
        {
            return await _database.DeleteAsync(task);
        }

        #endregion

        #region PlantSpecies Methods

        public async Task<List<PlantSpecies>> GetAllPlantSpeciesAsync()
        {
            return await _database.Table<PlantSpecies>().ToListAsync();
        }

        public async Task<PlantSpecies> GetPlantSpeciesByIdAsync(string id)
        {
            return await _database.Table<PlantSpecies>().Where(ps => ps.Id == id).FirstOrDefaultAsync();
        }

        public async Task<int> AddPlantSpeciesAsync(PlantSpecies species)
        {
            if (string.IsNullOrEmpty(species.Id))
            {
                species.Id = Guid.NewGuid().ToString();
            }

            if (species.CreatedAt == default)
            {
                species.CreatedAt = DateTime.UtcNow;
            }

            species.LastModifiedAt = DateTime.UtcNow;

            return await _database.InsertAsync(species);
        }

        public async Task<int> UpdatePlantSpeciesAsync(PlantSpecies species)
        {
            species.LastModifiedAt = DateTime.UtcNow;
            return await _database.UpdateAsync(species);
        }

        #endregion

        #region WorkLog Methods

        public async Task<List<WorkLog>> GetWorkLogsByTaskIdAsync(string taskId)
        {
            return await _database.Table<WorkLog>().Where(w => w.TaskId == taskId).ToListAsync();
        }

        public async Task<int> AddWorkLogAsync(WorkLog workLog)
        {
            if (string.IsNullOrEmpty(workLog.Id))
            {
                workLog.Id = Guid.NewGuid().ToString();
            }

            if (workLog.CreatedAt == default)
            {
                workLog.CreatedAt = DateTime.UtcNow;
            }

            workLog.LastModifiedAt = DateTime.UtcNow;

            return await _database.InsertAsync(workLog);
        }

        #endregion

        #region User Methods

        public async Task<User> GetUserByIdAsync(string id)
        {
            return await _database.Table<User>().Where(u => u.Id == id).FirstOrDefaultAsync();
        }

        public async Task<User> GetUserByUsernameAsync(string username)
        {
            return await _database.Table<User>().Where(u => u.Username == username).FirstOrDefaultAsync();
        }

        public async Task<int> AddUserAsync(User user)
        {
            if (string.IsNullOrEmpty(user.Id))
            {
                user.Id = Guid.NewGuid().ToString();
            }

            if (user.CreatedAt == default)
            {
                user.CreatedAt = DateTime.UtcNow;
            }

            user.LastModifiedAt = DateTime.UtcNow;

            return await _database.InsertAsync(user);
        }

        public async Task<int> UpdateUserAsync(User user)
        {
            user.LastModifiedAt = DateTime.UtcNow;
            return await _database.UpdateAsync(user);
        }

        #endregion
    }

    public interface ILocalDatabase
    {
        Task<List<Plant>> GetAllPlantsAsync();
        Task<Plant> GetPlantByIdAsync(string id);
        Task<List<Plant>> GetPlantsByAreaIdAsync(string areaId);
        Task<List<Plant>> GetPlantsModifiedSinceAsync(DateTime since);
        Task<int> AddPlantAsync(Plant plant);
        Task<int> UpdatePlantAsync(Plant plant);
        Task<int> DeletePlantAsync(Plant plant);

        Task<List<Area>> GetAllAreasAsync();
        Task<Area> GetAreaByIdAsync(string id);
        Task<List<Area>> GetAreasModifiedSinceAsync(DateTime since);
        Task<int> AddAreaAsync(Area area);
        Task<int> UpdateAreaAsync(Area area);
        Task<int> DeleteAreaAsync(Area area);

        Task<List<MaintenanceTask>> GetAllTasksAsync();
        Task<List<MaintenanceTask>> GetTasksByAssigneeIdAsync(string assigneeId);
        Task<List<MaintenanceTask>> GetTasksByAreaIdAsync(string areaId);
        Task<List<MaintenanceTask>> GetTasksByPlantIdAsync(string plantId);
        Task<List<MaintenanceTask>> GetTasksModifiedSinceAsync(DateTime since);
        Task<MaintenanceTask> GetTaskByIdAsync(string id);
        Task<int> AddTaskAsync(MaintenanceTask task);
        Task<int> UpdateTaskAsync(MaintenanceTask task);
        Task<int> DeleteTaskAsync(MaintenanceTask task);

        Task<List<PlantSpecies>> GetAllPlantSpeciesAsync();
        Task<PlantSpecies> GetPlantSpeciesByIdAsync(string id);
        Task<int> AddPlantSpeciesAsync(PlantSpecies species);
        Task<int> UpdatePlantSpeciesAsync(PlantSpecies species);

        Task<List<WorkLog>> GetWorkLogsByTaskIdAsync(string taskId);
        Task<int> AddWorkLogAsync(WorkLog workLog);

        Task<User> GetUserByIdAsync(string id);
        Task<User> GetUserByUsernameAsync(string username);
        Task<int> AddUserAsync(User user);
        Task<int> UpdateUserAsync(User user);
    }
}