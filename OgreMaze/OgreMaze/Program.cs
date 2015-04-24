using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OgreMaze
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Example input 1:");
            Swamp.PrintMap(ExampleMaps.Example1);
            GoldFinder goldFinder = new GoldFinder(ExampleMaps.Example1);
            goldFinder.FindTheGold();

            Console.WriteLine("Example input 2:");
            Swamp.PrintMap(ExampleMaps.Example2);
            goldFinder = new GoldFinder(ExampleMaps.Example2);
            goldFinder.FindTheGold();
        }
    }
}
