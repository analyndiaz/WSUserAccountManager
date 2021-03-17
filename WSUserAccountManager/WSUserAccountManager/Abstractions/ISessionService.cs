using System.Threading.Tasks;
using WSUserAccountManager.Database.Entities;

namespace WSUserAccountManager.Abstractions
{
    public interface ISessionService
    {
        Task<Session> Create(int userAccountId, int validity);
    }
}
