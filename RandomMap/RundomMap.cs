using System;
using System.Collections.Generic;
using System.Linq;
using RandomMapTest.Model;

namespace RandomMapTest
{
    public class RandomMap
    {
        Dictionary<Vector3Int, int> MapDic
        {
            get
            {
                var temp = new Dictionary<Vector3Int, int>();
                for (var z = 0; z < _size; z++)
                {
                    for (var x = 0; x < _size; x++)
                    {
                        temp[new Vector3Int { x = x, z = z }] = MapData[z][x];
                    }
                }
                return temp;
            }
        }

        public List<List<int>> MapData { get; set; }
        readonly int _size;
        readonly int _popRate;
        int _minTile = 0;
        int TileCount => MapData.Sum(x => x.Count(y => y == 0));
        readonly Vector3Int _startPos = new Vector3Int { x = 0, z = 0 };
        Vector3Int _goalPos = new Vector3Int { x = 0, z = 0 };
        Inclination Inclination;

        int _seed;
        static Random _random = new Random();

        public RandomMap(int size, int tileRate, int popRate)
        {
            _size = size;
            _popRate = popRate;
            _minTile = size * size * tileRate / 100;
        }

        public void CreateMap(int seed)
        {
            _seed = seed;
            _random = new Random(_seed);
            Mapping();
        }
        public void CreateMap()
        {
            _seed = new Random().Next();
            _random = new Random(_seed);
            Mapping();
        }

        void Mapping()
        {
            MapData = Enumerable.Range(0, _size).Select(z => Enumerable.Repeat(-1, _size).ToList()).ToList();
            Inclination = new Inclination();
            var nowPos = _startPos;
            //指定タイルに満たなかった場合はランダムな1:wallから再スタート
            while (TileCount < _minTile)
            {
                MapData[nowPos.z][nowPos.x] = 0;
                while (MoveNextPos(ref nowPos)) MapData[nowPos.z][nowPos.x] = 0;    //次のposition着色
                _goalPos = nowPos;
                nowPos = GetRandamWallPos();
                IndicateMap();
            }
        }

        Vector3Int GetRandamWallPos()
        {
            var wallKeys = MapDic.Where(cell => cell.Value == 1).ToList();
            var idx = _random.Next(0, wallKeys.Count - 1);
            return new Vector3Int { x = wallKeys[idx].Key.x, z = wallKeys[idx].Key.z };
        }

        bool MoveNextPos(ref Vector3Int position)
        {
            //-1:none/0:tile/1:wall
            var options = new List<(int direction, Vector3Int vector)>();
            //4方向のうち範囲内で0:tile以外のマスを追加
            //up z--
            if (0 < position.z && 0 != MapData[position.z - 1][position.x])
                options.Add((1, new Vector3Int { z = position.z - 1, x = position.x }));
            //right x++
            if (position.x + 1 < _size && 0 != MapData[position.z][position.x + 1])
                options.Add((2, new Vector3Int { z = position.z, x = position.x + 1 }));
            //down z++
            if (position.z + 1 < _size && 0 != MapData[position.z + 1][position.x])
                options.Add((3, new Vector3Int { z = position.z + 1, x = position.x }));
            //left x--
            if (0 < position.x && 0 != MapData[position.z][position.x - 1])
                options.Add((4, new Vector3Int { z = position.z, x = position.x - 1 }));

            //四方に壁設置
            options.ForEach(pos => MapData[pos.vector.z][pos.vector.x] = 1);
            if (options.Count < 1) return false;

            //inclinationに応じてoption調整
            var aditional = options.FirstOrDefault(x => x.direction == Inclination.HotDirection);
            if (aditional.direction > 0)
                options.Add(aditional);

            var temp = options[_random.Next(0, options.Count)];
            switch(temp.direction)
            {
                case 1:
                    Inclination.Up();
                    break;
                case 2:
                    Inclination.Right();
                    break;
                case 3:
                    Inclination.Down();
                    break;
                case 4:
                    Inclination.Left();
                    break;
            }
            position = temp.vector;
            return true;
        }

        public void IndicateMap()
        {
            Console.Clear();
            Console.WriteLine($"\r\nsize:{_size},poprate:{_popRate},tile:{TileCount}\r\nseed：{_seed}");
            Console.WriteLine($"inclination:{Inclination.HotDirection}{Inclination.Info}");
            for (var z = 0; z < _size; z++)
            {
                var line = "";
                Enumerable.Range(0, _size).ToList().ForEach(x =>
                {
                    line += z == _startPos.z && x == _startPos.x ? "◎" :
                            z == _goalPos.z && x == _goalPos.x ? "×" :
                               MapData[z][x] == 0 ? TrueFromRate(_popRate) ? TrueFromRate(5) ? "★" : "◆" : "■" :
                                    MapData[z][x] == 1 ? "◇" : "□";
                });
                Console.WriteLine(line);
            }
            Console.ReadKey();
        }

        static bool TrueFromRate(int rate)
        {
            var rates = Enumerable.Range(0, 100).ToDictionary(x => x, y => false);
            Enumerable.Range(0, rate).ToList().ForEach(x => rates[x] = true);
            return rates[_random.Next(0, 100)];
        }
    }
}
