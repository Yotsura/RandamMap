using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RandomMapTest
{
    class Program
    {
        static void Main(string[] args)
        {
            var map = new RandomMap(30, 50, 10);
            for (; ; )
            {
                map.CreateMap();
                map.IndicateMap();

                foreach (var pos in map.MapDic)
                {
                    switch(pos.stat)
                    {
                        case -1:
                            //床なし
                            break;
                        case 0:
                            //床あり
                            break;
                        case 1:
                            //壁あり
                            break;

                    }
                }

                //int.TryParse(DateTime.Now.ToString("mmssfff"), out var seed);
                //Console.WriteLine($"seed:{seed}");
                //var test2 = new RandomTown(25, 25, 1, seed
                //    , new Vector3Int { x = 2, y = 4, z = 2 }, new Vector3Int { x = 5, y = 7, z = 5 });
                //test2.SplitMap(2, 2, 1);
                //var result = test2.GetBuildings();
            }
        }
    }
}
