using Chirp.Core;

namespace Chirp.Infrastructure.Chirp.Repositories;

public interface ILikeRepository
{
    public Task AddLike(string authorName, int cheepId);
    public Task RemoveLike(string authorName, int cheepId);
}