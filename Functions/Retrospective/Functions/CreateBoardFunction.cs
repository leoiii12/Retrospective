using System.Threading.Tasks;
using Microsoft.Azure.WebJobs.Host;
using Retrospective.Boards.Interfaces;
using Retrospective.Common;
using Retrospective.Functions.Dtos;

namespace Retrospective.Functions
{
    public class CreateBoardFunction : IFunction<CreateBoardInput, CreateBoardOutput>
    {
        private readonly IBoardManager _boardManager;

        public CreateBoardFunction(IBoardManager boardManager)
        {
            _boardManager = boardManager;
        }

        public async Task<CreateBoardOutput> InvokeAsync(CreateBoardInput input, TraceWriter log)
        {
            var board = await _boardManager.CreateAsync(input.BoardId);
            if (board == null) throw new UserFriendlyException("Cannot create the board.");

            return new CreateBoardOutput
            {
                Board = board.ToDto()
            };
        }
    }
}