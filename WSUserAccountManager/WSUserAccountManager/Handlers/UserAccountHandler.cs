using System;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using Newtonsoft.Json;
using WSUserAccountManager.Abstractions;
using WSUserAccountManager.Enums;
using WSUserAccountManager.Helpers;
using WSUserAccountManager.Models.Messages;
using WSUserAccountManager.Models;
using System.Net.WebSockets;

namespace WSUserAccountManager.Handlers
{
    public class UserAccountHandler : WebSocketHandlerBase
    {
        private readonly IUserAccountService _userAccountService;
        private readonly IMapper _mapper;

        public UserAccountHandler(IUserAccountService userAccountService,
            IMapper mapper)
        {
            _userAccountService = userAccountService;
            _mapper = mapper;
        }

        public override async Task OnReceiveAsync(WebSocket webSocket, string messageCmd)
        {
            var message = MessageConverter.ToModel<Message>(messageCmd);

            var result = await PerformCommand(message.Command, messageCmd);

            if (result != null)
            {
                var resultMessage = MessageConverter.ToMessage(result, message.Command);
                await base.SendMessageAsync(webSocket, resultMessage);
            }
        }

        private async Task<OperationResult> PerformCommand(string command, string message)
        {
            OperationResult result = null;

            var commandType = EnumHelper.Parse<CommandType>(command);
            switch (commandType)
            {
                case (CommandType.Register):
                    var registerMsg = MessageConverter.ToModel<Register>(message);
                    var userAcctModel = _mapper.Map<UserAccount>(registerMsg);
                    result = await _userAccountService.Register(userAcctModel);
                    break;
                case (CommandType.LoginSalt):
                    var loginModel = MessageConverter.ToModel<LoginSalt>(message);
                    result = await _userAccountService.LoginSalt(loginModel.UserName);
                    break;
                case (CommandType.Login):
                    var loginSaltModel = MessageConverter.ToModel<Login>(message);
                    result = await _userAccountService.Login(loginSaltModel.UsernameOrEmail, loginSaltModel.Challenge);
                    break;
                case (CommandType.CheckUsername):
                case (CommandType.CheckEmail):
                    var acctModel = MessageConverter.ToModel<CheckAccount>(message);
                    if (string.IsNullOrEmpty(acctModel.UserName) && !string.IsNullOrEmpty(acctModel.Email))
                        result = await _userAccountService.CheckAccount(nameof(CheckAccount.Email), acctModel.Email);
                    else
                        result = await _userAccountService.CheckAccount(nameof(CheckAccount.UserName), acctModel.UserName);
                    break;
                case (CommandType.EmailVerification):
                    var verAcctModel = MessageConverter.ToModel<CheckAccount>(message);
                    result = await _userAccountService.VerifyAccount(verAcctModel.UserName, verAcctModel.Email);
                    break;
                default:
                    break;
            }

            return result;
        }
    }
}
