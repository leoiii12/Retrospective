using System.Threading.Tasks;

namespace Retrospective.Boards
{
    public interface IBoardManager
    {
        Task<Board> CreateAsync(string boardId);
        Task<Board> GetAsync(string boardId, string password);
    }
}