using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;

namespace OgreMaze
{
    internal class Node : IComparable
    {
        internal Space Space;
        internal Node Parent;

        internal double F, G, H;

        public Node(Space space)
        {
            Space = space;
            Parent = null;

            F = G = H = 0.0;
        }

        public bool Goal
        {
            get
            {
                return Space.OgreCanFindTheGoldHere;
            }
        }

        public uint GetDistanceTo(Node dest)
        {
            uint ret = 0;

            ret += (uint)Math.Abs(Space.X - dest.Space.X);
            ret += (uint)Math.Abs(Space.Y - dest.Space.Y);

            return ret;
        }

        public uint GetDistanceTo(Space dest)
        {
            uint ret = 0;

            ret += (uint)Math.Abs(Space.X - dest.X);
            ret += (uint)Math.Abs(Space.Y - dest.Y);

            return ret;
        }


        public int CompareTo(Object obj)
        {
            if (obj == null)
            {
                return 1;
            }

            Node node = obj as Node;
            if (node == null)
            {
                throw new ArgumentException("Object is not a Node");
            }

            if (F < node.F)
            {
                return -1;
            }
            if (F > node.F)
            {
                return 1;
            }
            return 1;
        }
    }

    public class GoldFinder
    {
        private Swamp Map;
        private Space StartPosition;
        private Space EndPosition;
        private SortedList<Node, Space> Open;
        private List<Space> Closed;

        public GoldFinder(char[][] map)
        {
            Map = new Swamp(map); ;
            StartPosition = Map.OgrePosition;
            EndPosition = Map.GoldPosition;
            Open = new SortedList<Node,Space>();
            Closed = new List<Space>();
        }

        void printPath(Node end)
        {
            char[][] map = Map.Map;

            Node cur = end;
            while (true)
            {
                for (int y = 0; y < 2 && cur.Space.Y + y < map.Length; y++)
                {
                    for (int x = 0; x < 2 && cur.Space.X + x < map.Length; x++)
                    {
                        map[cur.Space.Y + y][cur.Space.X + x] = '&';
                    }
                }

                if (cur.Space == StartPosition)
                {
                    break;
                }
                cur = cur.Parent;
            }

            Swamp.PrintMap(map);
        }

        void ProcessNeighbor(Node cur, Node neighbor)
        {
            if (neighbor.Space == null || Closed.Contains(neighbor.Space))
            {
                return;
            }

            if (!neighbor.Space.OgreCanStandHere)
            {
                Closed.Add(neighbor.Space);
                return;
            }

            double g = cur.G + cur.GetDistanceTo(neighbor);

            if (!Open.ContainsValue(neighbor.Space) || g < neighbor.G)
            {
                if (Open.ContainsValue(neighbor.Space))
                {
                    Open.RemoveAt(Open.IndexOfValue(neighbor.Space));
                }

                neighbor.Parent = cur;
                neighbor.G = g;
                neighbor.H = neighbor.GetDistanceTo(EndPosition);
                neighbor.F = neighbor.G + neighbor.H;

                Open.Add(neighbor, neighbor.Space);
            }
        }

        public void FindTheGold()
        {
            Open.Add(new Node(StartPosition), StartPosition);

            while (Open.Count > 0)
            {
                Node cur = Open.Keys.Min();

                if (cur.Goal)
                {
                    Console.WriteLine("Found a path:");
                    printPath(cur);
                    return;
                }

                Open.RemoveAt(Open.IndexOfKey(cur));

                Closed.Add(cur.Space);

                ProcessNeighbor(cur, new Node(cur.Space.North));
                ProcessNeighbor(cur, new Node(cur.Space.East));
                ProcessNeighbor(cur, new Node(cur.Space.South));
                ProcessNeighbor(cur, new Node(cur.Space.West));

            }

            Console.WriteLine("Could not find a path.");
        }
    }
}
