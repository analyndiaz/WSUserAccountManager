using System.Threading.Tasks;
using WSUserAccountManager.Models;

namespace WSUserAccountManager.Abstractions
{
    public interface IUserAccountService
    {
        Task<OperationResult> Register(UserAccount userAccountModel);

        Task<OperationResult> LoginSalt(string userName);

        Task<OperationResult> Login(string userName, string challenge);

        Task<OperationResult> CheckAccount(string property, object value);

        Task<OperationResult> VerifyAccount(string userName, string email);
    }
}
