using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using WSUserAccountManager.Abstractions;
using WSUserAccountManager.Database.Entities;
using WSUserAccountManager.Helpers;

namespace WSUserAccountManager.Services.UserAccount
{
    public class SaltService : ISaltService
    {
        private readonly IRepository<Salt> _repository;

        public SaltService(IRepository<Salt> repository)
        {
            _repository = repository;
        }

        public async Task<Salt> Create(int userAccountId, int validity = 300)
        {
            if (userAccountId == default(int))
            {
                return null;
            };

            var userSalt = new Salt()
            {
                UserAccountId = userAccountId,
                Value = RandomGenerator.String(64),
                Validity = validity,
                CreatedTime = DateTime.Now
            };

            return await _repository.Save(userSalt);
        }

        public async Task<Salt> GetValidSalt(string userName)
        {
            var saltList = await _repository.GetAll(
                                    s => s.UserAccount.UserName == userName ||
                                         s.UserAccount.Email == userName);

            if (!saltList.Any())
            {
                return null;
            }

            var currDateTime = DateTime.Now;
            var validSalt = saltList
                            .FirstOrDefault(s => s.CreatedTime
                                .AddSeconds(s.Validity) >= currDateTime);
            return validSalt;
        }
    }
}
