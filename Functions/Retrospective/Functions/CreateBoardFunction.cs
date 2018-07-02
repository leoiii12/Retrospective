using System;
using System.Threading.Tasks;
using Retrospective.Boards;
using Retrospective.Boards.Interfaces;
using Retrospective.Common;
using Retrospective.Functions.Dtos;

namespace Retrospective.Functions
{
    public class CreateBoardFunction : IFunction<CreateBoardInput, Board>
    {
        private readonly IBoardManager _boardManager;

        public CreateBoardFunction(IBoardManager boardManager)
        {
            _boardManager = boardManager;
        }

        public async Task<Board> InvokeAsync(CreateBoardInput input)
        {
            var board = await _boardManager.CreateAsync(input.BoardId);
            if (board == null) throw new Exception("Cannot create the board.");

            return board;
        }
    }
}