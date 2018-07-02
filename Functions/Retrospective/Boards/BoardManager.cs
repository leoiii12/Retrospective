using System;
using System.Threading.Tasks;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Table;
using Retrospective.Boards.Interfaces;

namespace Retrospective.Boards
{
    public class BoardManager : IBoardManager
    {
        private readonly CloudTable _table;
        private readonly CloudTableClient _tableClient;

        public BoardManager(string connectionString)
        {
            _tableClient = CloudStorageAccount.Parse(connectionString).CreateCloudTableClient();
            _table = _tableClient.GetTableReference("boards");

            _table.CreateIfNotExistsAsync().Wait();
        }

        public async Task<Board> CreateAsync(string boardId)
        {
            var board = new Board(boardId);
            var insertOperation = TableOperation.Insert(board);

            var result = await _table.ExecuteAsync(insertOperation);
            if (result.HttpStatusCode != 204)
                throw new Exception("Cannot create a new board.");

            return board;
        }

        public async Task<Board> GetAsync(string boardId, string password)
        {
            var result = await _table.ExecuteAsync(TableOperation.Retrieve<Board>(boardId, password));

            return (Board) result.Result;
        }
    }
}