using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore.Internal;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SocketAPI.Hub
{
    public interface IHub
    {
        Task Send(string message);

        Task Pong(string message);

        Task UpLine(string message);

        Task DownLine(string message);

        Task ListenSelf(string message);

        Task AllOnLine(List<string> all);

        Task GameRestart();
    }

    public class Hub : Microsoft.AspNetCore.SignalR.Hub<IHub>
    {
        private readonly HttpContext _httpContext;
        private readonly RedisMethods _redis;

        public Hub(IHttpContextAccessor hca)
        {
            _redis = new RedisMethods();
            _httpContext = hca.HttpContext;
        }

        /// <summary>
        /// 发送
        /// </summary>
        /// <param name="message">消息内容</param>
        /// <param name="token">接收token</param>
        /// <returns></returns>
        public async Task Send(string message, string token)
        {
            await Clients.Group(token).Send(message);
        }

        /// <summary>
        /// 心跳检测
        /// </summary>
        /// <returns></returns>
        public async Task Ping()
        {
            await Clients.Caller.Pong($"pong {DateTime.Now:yyyy-MM-dd HH:mm:ss}");
        }

        /// <summary>
        /// 上线拦截
        /// </summary>
        /// <returns></returns>
        public override async Task OnConnectedAsync()
        {
            var token = _httpContext.Request.Query["token"];
            if (!string.IsNullOrEmpty(token))
            {
                // 将token关联到connectionId
                await Groups.AddToGroupAsync(Context.ConnectionId, token);
                _redis.ListRightPush(token, Context.ConnectionId); ;
                await Clients.Caller.ListenSelf(Context.ConnectionId);
                await Clients.Group(token).AllOnLine(_redis.ListRange<string>(token));
            }
            await base.OnConnectedAsync();
        }

        /// <summary>
        /// 下线拦截
        /// </summary>
        /// <param name="exception"></param>
        /// <returns></returns>
        public override async Task OnDisconnectedAsync(Exception exception)
        {
            var token = _httpContext.Request.Query["token"];
            await Groups.RemoveFromGroupAsync(Context.ConnectionId, token);
            _redis.ListRemove(token, Context.ConnectionId);
            var isRestart = _redis.ListRange<string>(token, 0, 2).Any(x => x == Context.ConnectionId);
            if (isRestart)
            {
                await Clients.Groups(token).GameRestart();
            }
            await Clients.Group(token).AllOnLine(_redis.ListRange<string>(token));
            await base.OnDisconnectedAsync(exception);
        }
    }
}
