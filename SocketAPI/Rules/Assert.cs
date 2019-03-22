using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SocketAPI.Rules
{
    public static class Assert
    {
        /// <summary>
        /// 判断整个棋盘的胜负（12轴遍历）
        /// </summary>
        /// <param name="control"></param>
        /// <param name="checkerboard"></param>
        /// <returns></returns>
        public static bool AssertChessboard1(string control, List<List<ChessPieces>> checkerboard)
        {
            var current = checkerboard.SelectMany(x => x) // 当前人全部的落子
                .Where(x => x.Role == control);

            for (var i = 0; i < checkerboard.Count; i++)
            {
                if (current.Where(x => x.Item[0] == i).MaxContinuous(ChessPiecesForm.X轴) >= 5) // x轴超5子
                {
                    return true;
                }
                if (current.Count(x => x.Item[1] == i) >= 5) // y轴超5子
                {
                    return true;
                }
                if (current.Count(x => x.Item[0] - x.Item[1] == i) >= 5)
                {
                    return true;
                }
                if (current.Count(x => x.Item[0] + x.Item[1] == i) >= 5)
                {
                    return true;
                }
            }


            return false;
        }

        /// <summary>
        /// 判断整个棋盘的胜负(棋子遍历)
        /// </summary>
        /// <param name="control"></param>
        /// <param name="checkerboard"></param>
        /// <returns></returns>
        public static bool AssertChessboard2(string control, List<List<ChessPieces>> checkerboard)
        {
            return false;
        }

        /// <summary>
        /// 最大连续性
        /// </summary>
        /// <param name="checkerboard">连续的棋子</param>
        /// <param name="from">棋子形态</param>
        /// <returns></returns>
        private static int MaxContinuous(this IEnumerable<ChessPieces> checkerboard, ChessPiecesForm from)
        {

            switch (from)
            {
                case ChessPiecesForm.X轴:
                    checkerboard = checkerboard.OrderBy(x => x.Item[1]);
                    while (true)
                    {
                        //checkerboard.
                    }
                    break;
            }
            return 0;
        }
    }

    /// <summary>
    /// 棋子形态
    /// </summary>
    public enum ChessPiecesForm
    {
        X轴,
        Y轴,
        左斜,
        右斜
    }

    /// <summary>
    /// 棋子
    /// </summary>
    public class ChessPieces
    {
        /// <summary>
        /// 坐标
        /// </summary>
        [JsonProperty("c")]
        [Obsolete("使用Item强类型属性，而不是字符串")]
        public string Coordinate { get; set; }

        /// <summary>
        /// 坐标
        /// </summary>
#pragma warning disable 618
        public int[] Item => Coordinate.Split(',').Select(x => Convert.ToInt32(x)).ToArray();
#pragma warning restore 618

        /// <summary>
        /// 角色（落子人）
        /// </summary>
        [JsonProperty("s")]
        public string Role { get; set; }
        /// <summary>
        /// 占位
        /// </summary>
        [JsonProperty("r")]
        public string Place { get; set; }
    }
}
