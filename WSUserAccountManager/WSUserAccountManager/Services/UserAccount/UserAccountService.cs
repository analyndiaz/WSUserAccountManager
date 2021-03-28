using System;
using System.Threading.Tasks;
using AutoMapper;
using WSUserAccountManager.Abstractions;
using WSUserAccountManager.Extensions;
using WSUserAccountManager.Models;

namespace WSUserAccountManager.Services.UserAccount
{
    public class UserAccountService : IUserAccountService
    {
        private readonly IRepository<Database.Entities.UserAccount> _repository;
        private readonly IMapper _mapper;
        private readonly IPasswordService _passwordService;
        private readonly IVerificationService _verificationService;
        private readonly ISaltService _saltService;

        private readonly ISessionService _sessionService;

        public UserAccountService(
            IRepository<Database.Entities.UserAccount> repository,
            IMapper mapper,
            IPasswordService passwordService,
            IVerificationService verificationService,
            ISaltService saltService,
            ISessionService sessionService)
        {
            _repository = repository;
            _mapper = mapper;
            _passwordService = passwordService;
            _verificationService = verificationService;
            _saltService = saltService;
            _sessionService = sessionService;
        }

        public async Task<OperationResult> Login(string userNameOrEmail, string challenge)
        {
            var operationResult = new OperationResult();
            var primaryPwd = true;

            operationResult.AddResult("username", userNameOrEmail);
            operationResult.AddResult("sessionID", null);
            operationResult.AddResult("validity", 0);

            if (string.IsNullOrEmpty(userNameOrEmail))
            {
                operationResult.AddError("Username cannot be undefined.");
                Console.WriteLine("Username cannot be undefined." + userNameOrEmail);
                return operationResult;
            }

            // check if login by userName
            var userAcctModel = await _repository.Get(u => u.UserName == userNameOrEmail);
            if (userAcctModel == null)
            {
                // check if login by email
                userAcctModel = await _repository.Get(u => u.Email == userNameOrEmail);
                if (userAcctModel == null) 
                {
                    operationResult.AddError("User account does not exists.");
                    return operationResult;
                }

                primaryPwd = false;
            }

            var userHashedPwd = await _passwordService.GetChallenge(userNameOrEmail, primaryPwd);
            if (!userHashedPwd.Equals(challenge))
            {
                operationResult.AddError("Challenge is incorrect.");
                return operationResult;
            }

            var session = await _sessionService.Create(userAcctModel.UserAccountId, 300);
            if (session == null)
            {
                operationResult.AddError("Failed creating session.");
                Console.WriteLine("Failed creating session.");
                return operationResult;
            }

            operationResult.Result["sessionID"] = session.Value;
            operationResult.Result["validity"] = session.Validity;

            operationResult.AddResult("userID", userAcctModel.UserId);
            return operationResult;
        }

        public async Task<OperationResult> LoginSalt(string userName)
        {
            var operationResult = new OperationResult();
            operationResult.AddResult(nameof(Models.UserAccount.UserName), userName);

            if (string.IsNullOrEmpty(userName))
            {
                operationResult.AddError("Username cannot be undefined.");
                return operationResult;
            }

            var userAcctModel = await _repository.Get(u => u.UserName == userName);
            if (userAcctModel == null)
            {
                operationResult.AddError("User account does not exists.");
                return operationResult;
            }
            var salt = await _saltService.Create(userAcctModel.UserAccountId);
            if (salt == null)
            {
                operationResult.AddError("Failed creating salt for " + userAcctModel.UserName);
                return operationResult;
            }

            operationResult.AddResult(nameof(salt.Validity), salt.Validity);
            operationResult.AddResult(nameof(salt), salt.Value);
            return operationResult;
        }

        public async Task<OperationResult> Register(Models.UserAccount userAccountModel)
        {
            var operationResult = new OperationResult();
            if (userAccountModel == null)
            {
                operationResult.AddError("User account model cannot be null.");
                return operationResult;
            }

            var dbUserAcct = await _repository.Get(u => u.UserName == userAccountModel.UserName);
            if (dbUserAcct != null)
            {
                operationResult.AddError("UserName already exists.");
                operationResult.AddResult(nameof(Models.UserAccount.UserName), userAccountModel.UserName);
                return operationResult;
            }

            var userAccount = _mapper.Map<Database.Entities.UserAccount>(userAccountModel);
            var result = await _repository.Save(userAccount);

            if (result == null)
            {
                operationResult.AddError("Failed adding UserName : " + userAccount.UserName);
                operationResult.AddResult(nameof(Models.UserAccount.UserName), userAccount.UserName);
                return operationResult;
            }

            await _passwordService.Save(result.UserAccountId, userAccountModel);
            await _verificationService.SaveCode(result.UserAccountId, userAccountModel);

            operationResult.AddResult(nameof(Models.UserAccount.UserName), userAccount.UserName);
            return operationResult;
        }

        public async Task<OperationResult> CheckAccount(string property, object value)
        {
            var result = new OperationResult();
            result.AddResult(property, value);

            var available = false;

            var dbAccount = await _repository.Get(u => u.GetProperty(property) == value.ToString());
            if (dbAccount != null)
            {
                available = true;
            }

            result.AddResult(nameof(available), available);
            return result;
        }

        public async Task<OperationResult> VerifyAccount(string userName, string email)
        {
            var operationResult = new OperationResult();
            if (string.IsNullOrEmpty(userName) || string.IsNullOrEmpty(email))
            {
                operationResult.AddError("Username cannot be undefined.");
                return operationResult;
            }

            var userAcctModel = await _repository.Get(u => u.UserName == userName && u.Email == email);
            if (userAcctModel == null)
            {
                operationResult.AddError("User account does not exists.");
                return operationResult;
            }

            var result = await _verificationService.Verify(userAcctModel);
            if (!result)
            {
                operationResult.AddError("Failed generating verification code on " + userAcctModel.UserName);
                return operationResult;
            }

            return operationResult;
        }
    }
}
