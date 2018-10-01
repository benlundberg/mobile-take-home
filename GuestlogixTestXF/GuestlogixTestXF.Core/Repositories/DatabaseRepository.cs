﻿using SQLite;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace GuestlogixTestXF.Core
{
    public class DatabaseRepository : IDatabaseRepository
    {
        public DatabaseRepository(ILocalFileSystemHelper fileHelper)
        {
            this.fileHelper = fileHelper;
        }

        public async Task<bool> ClearAsync<T>() where T : new()
        {
            await GetConnection.DropTableAsync<T>();
            await GetConnection.CreateTableAsync<T>();

            return true;
        }

        public async Task<bool> DeleteAsync<T>(List<T> list)
        {
            foreach (var item in list)
            {
                await GetConnection.DeleteAsync(item);
            }

            return true;
        }

        public async Task<bool> DeleteAsync<T>(T entity)
        {
            int rows = await GetConnection.DeleteAsync(entity);

            return rows != 0;
        }

        public async Task<IEnumerable<T>> GetAsync<T>() where T : new()
        {
            return await GetConnection.Table<T>().ToListAsync();
        }

        public async Task<T> GetAsync<T>(object id) where T : new()
        {
            return await GetConnection.GetAsync<T>(id);
        }

        public async Task<bool> InsertAsync<T>(T entity)
        {
            int rows = await GetConnection.InsertAsync(entity);

            return rows != 0;
        }

        public async Task<bool> InsertOrReplaceAsync<T>(T entity)
        {
            int row = await GetConnection.InsertOrReplaceAsync(entity);

            return row != 0;
        }

        public async Task<bool> UpdateAsync<T>(T entity)
        {
            int rows = await GetConnection.UpdateAsync(entity);

            return rows != 0;
        }

        public async Task<bool> CreateTablesAsync(List<Type> entities)
        {
            foreach (var item in entities)
            {
                await GetConnection.CreateTablesAsync(CreateFlags.None, item);
            }

            return true;
        }

        public async Task<bool> InsertAsync<T>(List<T> entities)
        {
            int rows = await GetConnection.InsertAllAsync(entities);

            return rows != 0;
        }

        private SQLiteAsyncConnection connection;
        private SQLiteAsyncConnection GetConnection => connection ?? (connection = new SQLiteAsyncConnection(fileHelper.GetLocalPath($"{AppConfig.AppName}DB.db3")));

        private ILocalFileSystemHelper fileHelper;
    }
}