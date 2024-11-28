using Chirp.Core;

namespace Chirp.Infrastructure.Chirp.Repositories;

public interface ILikeRepository
{
    public Task AddLike(string authorName, Cheep cheep);
    public Task RemoveLike(string authorName, Cheep cheep);
}