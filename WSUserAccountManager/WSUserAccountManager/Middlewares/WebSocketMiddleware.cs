using System;
using System.Net.WebSockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using WSUserAccountManager.Handlers;

namespace WSUserAccountManager.Middlewares
{
    public class WebSocketMiddleware
    {
        private readonly RequestDelegate _next;
        private WebSocketHandlerBase _webSocketHandler { get; set; }

        public WebSocketMiddleware(RequestDelegate next, WebSocketHandlerBase webSocketHandler)
        {
            _next = next;
            _webSocketHandler = webSocketHandler;
        }

        public async Task Invoke(HttpContext context)
        {
            if (context.WebSockets.IsWebSocketRequest)
            {
                var webSocket = await context.WebSockets.AcceptWebSocketAsync();

                try
                {
                    await Receive(webSocket, async (result, buffer) =>
                   {
                       if (result.MessageType == WebSocketMessageType.Text)
                       {
                           var strBuffer = Encoding.UTF8.GetString(buffer, 0, result.Count);
                           await _webSocketHandler.OnReceiveAsync(webSocket, strBuffer);
                           return;
                       }

                       else if (result.MessageType == WebSocketMessageType.Close)
                       {
                           await _webSocketHandler.OnDisconnected(webSocket, result);
                           return;
                       }

                   });
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }
            }
            else
            {
                context.Response.StatusCode = 400;
            }
        }

        private async Task Receive(WebSocket webSocket, Action<WebSocketReceiveResult, byte[]> handleMessage)
        {
            var buffer = new byte[1024 * 4];

            while (webSocket.State == WebSocketState.Open)
            {
                var serverMsg = Encoding.UTF8.GetString(buffer);

                var result = await webSocket.ReceiveAsync(new ArraySegment<byte>(buffer), CancellationToken.None);
                handleMessage(result, buffer);
            }
        }
    }

}
