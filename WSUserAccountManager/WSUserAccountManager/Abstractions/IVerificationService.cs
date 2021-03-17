using System.Threading.Tasks;
using WSUserAccountManager.Database.Entities;

namespace WSUserAccountManager.Abstractions
{
    public interface IVerificationService
    {
        Task SaveCode(int userAcctId, Models.UserAccount userAccount);

        Task<bool> Verify(UserAccount userAccount);
    }
}
