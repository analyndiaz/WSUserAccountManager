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
    public class SessionService : ISessionService
    {
        private readonly IRepository<Session> _repository;

        public SessionService(IRepository<Session> repository)
        {
            _repository = repository;
        }

        public async Task<Session> Create(int userAccountId, int validity)
        {
            if (userAccountId == default(int))
            {
                return null;
            };

            var userSalt = new Session()
            {
                UserAccountId = userAccountId,
                Value = RandomGenerator.String(64),
                Validity = validity,
            };

            return await _repository.Save(userSalt);
        }
    }
}
