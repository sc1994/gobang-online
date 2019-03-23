using System;
using System.Collections.Generic;
using System.Linq;

namespace SocketAPI.Rules
{
    public static class Compute
    {
        /// <summary>
        /// 连续性
        /// </summary>
        /// <param name="checkerboard">连续的棋子</param>
        /// <param name="from">棋子形态</param>
        /// <returns></returns>
        public static List<Feasibility> ContinuouInfo(this IEnumerable<ChessPieces> checkerboard, ChessPiecesForm from)
        {
            List<Feasibility> countList = null;
            var count = 0;
            switch (from)
            {
                case ChessPiecesForm.X轴:
                    checkerboard = checkerboard.OrderBy(x => x.Item[1]);
                    for (var i = 0; i < checkerboard.Count(); i++)
                    {
                        if (checkerboard.ElementAt(i).Item[1] - checkerboard.ElementAtOrDefault(i + 1)?.Item[1] == -1)
                        {
                            ++count;
                        }
                        else
                        {
                            if (countList == null) countList = new List<Feasibility>();
                            countList.Add(new Feasibility
                            {
                                Continuou = ++count
                            });
                            count = 0;
                        }
                    }
                    return countList;
                case ChessPiecesForm.Y轴:
                    checkerboard = checkerboard.OrderBy(x => x.Item[0]);
                    for (var i = 0; i < checkerboard.Count(); i++)
                    {
                        if (checkerboard.ElementAt(i).Item[0] - checkerboard.ElementAtOrDefault(i + 1)?.Item[0] == -1)
                        {
                            ++count;
                        }
                        else
                        {
                            if (countList == null) countList = new List<Feasibility>();
                            countList.Add(new Feasibility
                            {
                                Continuou = ++count
                            });
                            count = 0;
                        }
                    }
                    return countList;
                case ChessPiecesForm.左斜:
                    checkerboard = checkerboard.OrderBy(x => x.Item[0]);
                    for (var i = 0; i < checkerboard.Count(); i++)
                    {
                        if (checkerboard.ElementAt(i).Item[0] - checkerboard.ElementAtOrDefault(i + 1)?.Item[0] == -1
                            && checkerboard.ElementAt(i).Item[1] - checkerboard.ElementAtOrDefault(i + 1)?.Item[1] == -1)
                        {
                            ++count;
                        }
                        else
                        {
                            if (countList == null) countList = new List<Feasibility>();
                            countList.Add(new Feasibility
                            {
                                Continuou = ++count
                            });
                            count = 0;
                        }
                    }
                    return countList;
                case ChessPiecesForm.右斜:
                    checkerboard = checkerboard.OrderBy(x => x.Item[0]);
                    for (var i = 0; i < checkerboard.Count(); i++)
                    {
                        if (checkerboard.ElementAt(i).Item[0] - checkerboard.ElementAtOrDefault(i + 1)?.Item[0] == -1
                            && checkerboard.ElementAt(i).Item[1] - checkerboard.ElementAtOrDefault(i + 1)?.Item[1] == 1)
                        {
                            ++count;
                        }
                        else
                        {
                            if (countList == null) countList = new List<Feasibility>();
                            countList.Add(new Feasibility
                            {
                                Continuou = ++count
                            });
                            count = 0;
                        }
                    }
                    return countList;
                default: throw new NotImplementedException();
            }
        }
    }

    /// <summary>
    /// 可行性落子
    /// </summary>
    public class Feasibility
    {
        /// <summary>
        /// 连续落子个数
        /// </summary>
        public int Continuou { get; set; }

        /// <summary>
        /// 附近的可行性坐标
        /// </summary>
        public List<int[]> NextContinuous { get; set; } = new List<int[]>();
    }
}
