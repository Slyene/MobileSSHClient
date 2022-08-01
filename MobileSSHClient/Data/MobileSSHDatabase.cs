using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using MobileSSHClient.Models;
using SQLite;

namespace MobileSSHClient.Data
{
    public partial class MobileSSHDatabase
    {
        public SQLiteAsyncConnection ConnectionsDb { get; }

        public MobileSSHDatabase(string dbPath)
        {
            ConnectionsDb = new SQLiteAsyncConnection(dbPath);
            ConnectionsDb.CreateTableAsync<Connection>().Wait();
            ConnectionsDb.CreateTableAsync<Snippet>().Wait();
        }

        public event EventHandler DataChanged;

        public void OnDataChanged()
        {
            DataChanged?.Invoke(this, null);
        }

        public Task<List<Connection>> GetConnectionsAsync()
        {
            return ConnectionsDb.Table<Connection>().ToListAsync();
        }

        public Task<Connection> GetConnectionAsync(int id)
        {
            // Get a specific item.
            return ConnectionsDb.Table<Connection>().Where(connection => connection.Id == id).FirstOrDefaultAsync();
        }

        public void SaveConnectionAsync(Connection connection)
        {
            if (connection.Id != 0)
            {
                // Update an existing item.
                ConnectionsDb.UpdateAsync(connection).Wait();
            }
            else
            {
                // Save a new item.
                ConnectionsDb.InsertAsync(connection).Wait();
            }
                OnDataChanged();
        }

        public void DeleteConnectionAsync(Connection connection)
        {
            // Delete an item.
            ConnectionsDb.DeleteAsync(connection).Wait();
            OnDataChanged();
        }

        /////////////////////////////////////////////////////////////////////

        public Task<List<Snippet>> GetSnippetsAsync(int connectionId)
        {
            return ConnectionsDb.Table<Snippet>().Where(snippet => snippet.ConnectionId == connectionId).ToListAsync();
        }

        public Task<Snippet> GetSnippetAsync(int snippetId)
        {
            // Get a specific item.
            return ConnectionsDb.Table<Snippet>().Where(snippet => snippet.Id == snippetId).FirstOrDefaultAsync();
        }

        public void SaveSnippetAsync(Snippet snippet)
        {
            if (snippet.Id != 0)
            {
                // Update an existing item.
                ConnectionsDb.UpdateAsync(snippet).Wait();
            }
            else
            {
                // Save a new item.
                ConnectionsDb.InsertAsync(snippet).Wait();
            }
            OnDataChanged();
        }

        public void DeleteSnippetAsync(Snippet snippet)
        {
            // Delete an item.
            ConnectionsDb.DeleteAsync(snippet).Wait();
            OnDataChanged();
        }

        public async void DeleteSnippetsByConnectionIdAsync(int id)
        {
            List<Snippet> snippets = await GetSnippetsAsync(id);
            // Delete a list of items.

            for (int i = 0; i < snippets.Count; i++)
            {
                await ConnectionsDb.DeleteAsync(snippets[i]);
            }
            OnDataChanged();
        }
    }
}
