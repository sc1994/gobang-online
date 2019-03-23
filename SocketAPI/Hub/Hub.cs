using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
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

        Task GameRestart(string again);

        Task DownPieceMsg(string chess);
    }

    public class Hub : Microsoft.AspNetCore.SignalR.Hub<IHub>
    {
        private readonly HttpContext _httpContext;
        private readonly RedisMethods _redis;
        private static string RoomKey(string token) => $"room:{token}";
        private static string ChessKey(string token) => $"chess:{token}";

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
        /// 落子
        /// </summary>
        /// <param name="chess"></param>
        /// <param name="token"></param>
        /// <returns></returns>
        public async Task DownPiece(string token, string chess)
        {
            _redis.StringSet(ChessKey(token), chess);
            await Clients.Group(token).DownPieceMsg(_redis.StringGet<string>(ChessKey(token)));
        }

        /// <summary>
        /// 游戏结束，重新开始
        /// </summary>
        /// <param name="msg">重新开始的原因</param>
        /// <param name="token"></param>
        /// <returns></returns>
        public async Task GameRestart(string token, string msg)
        {
            await Clients.Group(token).GameRestart(msg);
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
                _redis.ListRightPush(RoomKey(token), Context.ConnectionId);
                await Clients.Caller.ListenSelf(Context.ConnectionId);
                await Clients.Group(token).AllOnLine(_redis.ListRange<string>(RoomKey(token)));
                var init = _redis.StringGet<string>(ChessKey(token));
                if (!string.IsNullOrWhiteSpace(init))
                {
                    await Clients.Group(token).DownPieceMsg(init);
                }
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

            var sign = _redis.ListRange<string>(RoomKey(token), 0, 1);
            var isRestart = sign.Contains(Context.ConnectionId);
            if (isRestart)
            {
                await Clients.Groups(token).GameRestart("参战人员下线，游戏结束");
            }

            _redis.ListRemove(RoomKey(token), Context.ConnectionId);
            await Debug(JsonConvert.SerializeObject(new { Token = RoomKey(token), Context.ConnectionId }));
            await Clients.Group(token).AllOnLine(_redis.ListRange<string>(RoomKey(token)));
            await base.OnDisconnectedAsync(exception);
        }

        private async Task Debug(string msg)
        {
            await File.AppendAllLinesAsync("D:/log.log", new[] { msg });
        }
    }
}
