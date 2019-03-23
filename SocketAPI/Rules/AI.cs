using System.Collections.Generic;

namespace SocketAPI.Rules
{
    /// <summary>
    /// AI
    /// </summary>
    public class AI
    {
        /// <summary>
        /// AI落子
        /// </summary>
        /// <param name="rival">对手的棋子</param>
        /// <param name="current">我的棋子</param>
        public void AIDown(IEnumerable<ChessPieces> rival, IEnumerable<ChessPieces> current)
        {
            // 对手有四子，封堵

            // 对手有三子，封堵 ，优先靠近我方棋子

            // 我有四子，跟子，win

            // 我有三子，跟子，优先靠近我方其他棋子

            // 对手两子一子，附近随机跟，优先靠近我方棋子

            // 我方两子一子，附近随机跟

            // 空盘落中
        }
    }
}

