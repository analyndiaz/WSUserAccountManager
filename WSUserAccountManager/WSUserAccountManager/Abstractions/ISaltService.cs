using System.Threading.Tasks;
using WSUserAccountManager.Database.Entities;

namespace WSUserAccountManager.Abstractions
{
    public interface ISaltService
    {
        Task<Salt> Create(int userAccountId, int validity = 300);

        Task<Salt> GetValidSalt(string userName);
    }
}
