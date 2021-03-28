using System.Threading.Tasks;

namespace WSUserAccountManager.Abstractions
{
    public interface IPasswordService
    {
        Task Save(int userAcctId, Models.UserAccount userAccount);

        Task<string> GetChallenge(string userName, bool isPrimary);
    }
}
