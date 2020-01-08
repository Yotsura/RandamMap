using System;
using System.Collections.Generic;
using System.Linq;

namespace TestCodes
{
    class Program
    {
        static void Main(string[] args)
        {
            for (; ; )
            {
                //int.TryParse(DateTime.Now.ToString("mmssfff"), out var seed);
                //Console.WriteLine($"seed:{seed}");
                //var test = new GenerateRandomTown(25, 25, 1, seed
                //    ,new Vector3Int {x=2,y=4,z=2 },new Vector3Int {x=5,y=7,z=5 });
                //test.SplitMap(2, 2, 1);
                //var result = test.GetBuildings();
                var test = new RandomMap3(20, 200, 250, 10).RandomWall();
                //_ = new RandomMap2(30, 200, 300, 10);
            }
        }
    }
}
