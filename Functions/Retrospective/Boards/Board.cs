using System;
using System.Linq;
using Microsoft.WindowsAzure.Storage.Table;

namespace Retrospective.Boards
{
    public class Board : TableEntity
    {
        private const string Chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";

        public Board(string boardId)
        {
            PartitionKey = boardId.Trim().Replace(" ", "-");
            RowKey = RandomString(8);
        }

        public Board()
        {
        }

        public DateTime DateTime { get; set; } = DateTime.UtcNow;

        public BoardDto ToDto()
        {
            return new BoardDto
            {
                BoardId = PartitionKey,
                Password = RowKey
            };
        }

        private static string RandomString(int length)
        {
            var random = new Random();
            return new string(Enumerable.Repeat(Chars, length).Select(s => s[random.Next(s.Length)]).ToArray());
        }

        public override string ToString()
        {
            return $"{nameof(Board)}-{PartitionKey}-{RowKey}";
        }
    }
}