using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GeoSOS.ArcMapAddIn.Utility.Optimization
{
    public class Ant
    {
        private int x;      //行号
        private int y;      //列号

        public int X
        {
            get
            { return x; }
            set
            { x = value; }
        }

        public int Y
        {
            get
            { return y; }
            set
            { y = value; }
        }

        public void SetXY(int newX, int newY)
        {
            x = newX;
            y = newY;
        }

        /// <summary>
        ///  随机打乱智能体的顺序
        /// </summary>
        /// <param name="agents"></param>
        /// <returns></returns>
        public static Ant[] DisturbAgentsOrder(Ant[] agents)
        {
            int count = agents.Length;
            Ant[] newAgents = new Ant[count];
            for (int i = 0; i < count; i++)
            {
                Random random = new Random();
                int pos = random.Next(0, count - 1 - i);
                newAgents[i] = agents[pos];
                agents[pos] = agents[count - 1 - i];
            }
            return newAgents;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="agents"></param>
        /// <returns></returns>
        public static Ant[] DuplicateAgents(Ant[] agents)
        {
            int count = agents.Length;
            Ant[] newAgents = new Ant[count];
            for (int i = 0; i < count; i++)
            {
                newAgents[i] = new Ant();
                newAgents[i].SetXY(agents[i].X, agents[i].Y);
            }
            return newAgents;
        }
    }
}
