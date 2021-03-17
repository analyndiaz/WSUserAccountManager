using System;
using System.Linq;
using System.Threading.Tasks;
using WSUserAccountManager.Abstractions;
using WSUserAccountManager.Database.Entities;
using WSUserAccountManager.Helpers;

namespace WSUserAccountManager.Services.UserAccount
{
    public class VerificationService : IVerificationService
    {
        private const int MaxNoCodesPerDay = 5;

        private readonly IRepository<VerificationCode> _repository;

        public VerificationService(IRepository<VerificationCode> repository)
        {
            _repository = repository;
        }

        public async Task SaveCode(int userAcctId, Models.UserAccount userAccount)
        {
            if (userAccount == null)
            {
                throw new ArgumentNullException(nameof(userAccount));
            }

            await SaveNewCode(userAcctId, userAccount.VerificationCode);
        }

        public async Task<bool> Verify(Database.Entities.UserAccount userAccount)
        {
            var existingCodes = await _repository.GetAll(c => c.UserAccount.UserName == userAccount.UserName
                                                && c.UserAccount.Email == userAccount.Email);

            if (existingCodes.Count() > MaxNoCodesPerDay)
            {
                return false;
            }

            var newCode = await SaveNewCode(userAccount.UserAccountId, RandomGenerator.Code(6));
            if (newCode == null)
            {
                return false;
            }

            return true;
        }

        private async Task<VerificationCode> SaveNewCode(int userAcctId, string code)
        {
            var userVerificationCodeModel = new VerificationCode()
            {
                UserAccountId = userAcctId,
                Code = code
            };

            return await _repository.Save(userVerificationCodeModel);
        }
    }
}
