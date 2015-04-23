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
            GoldFinder goldFinder = new GoldFinder(ExampleMaps.Example1);
            goldFinder.FindTheGold();
        }
    }
}
