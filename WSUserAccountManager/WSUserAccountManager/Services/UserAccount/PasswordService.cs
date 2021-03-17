using System;
using System.Linq;
using System.Threading.Tasks;
using WSUserAccountManager.Abstractions;
using WSUserAccountManager.Database.Entities;

namespace WSUserAccountManager.Services.UserAccount
{
    public class PasswordService : IPasswordService
    {
        private readonly IHashFunction _hashFunction;
        private readonly IRepository<Password> _repository;
        private readonly ISaltService _saltService;

        public PasswordService(IHashFunction hashFunction,
            IRepository<Password> repository,
            ISaltService saltService)
        {
            _hashFunction = hashFunction;
            _repository = repository;
            _saltService = saltService;
        }

        public async Task<string> GetChallenge(string userName)
        {
            var validSalt = await _saltService.GetValidSalt(userName);
            if (validSalt == null)
            {
                return string.Empty;
            }

            var passwordList = await _repository.GetAll(
                                    u => u.UserAccount.UserName == userName ||
                                         u.UserAccount.Email == userName);

            var password = passwordList.OrderByDescending(p => p.IsPrimary).FirstOrDefault();

            return _hashFunction.GetHashValue(validSalt.Value, password.Value);
        }

        public async Task Save(int userAcctId, Models.UserAccount userAccount)
        {
            if (userAccount == null)
            {
                throw new ArgumentNullException(nameof(userAccount));
            }

            var userPwdModel = new Password()
            {
                UserAccountId = userAcctId,
                Value = _hashFunction.GetHashValue("superSecretKey", userAccount.Password),
                IsPrimary = true
            };
            await _repository.Save(userPwdModel);

            var userSecPwcModel = new Password()
            {
                UserAccountId = userAcctId,
                Value = _hashFunction.GetHashValue("superSecretKey", userAccount.SecondaryPassword),
                IsPrimary = false
            };

            await _repository.Save(userSecPwcModel);
        }
    }
}
