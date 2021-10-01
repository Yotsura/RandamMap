using System;
using System.Collections.Generic;
using System.Linq;
using TestCodes.Models;

namespace TestCodes
{
    public class RandomMap
    {
        public List<List<int>> MapList { get; set; }
        readonly int _size;
        readonly int _popRate;
        int _minTile = 0;
        int _maxTile = 0;
        int _tileCount = 0;
        readonly Vector3Int _startPos = new Vector3Int { x = 0, z = 0 };
        Vector3Int _goalPos = new Vector3Int { x = 0, z = 0 };
        static Random random = new Random();

        public RandomMap(int size, int minTile, int maxTile, int popRate)
        {
            _size = size;
            _popRate = popRate;
            _minTile = minTile;
            _maxTile = maxTile;
            GenerateMap();
        }

        public void GenerateMap()
        {
            //指定タイル数になるまでループ
            while (_tileCount < _minTile || _maxTile < _tileCount)
            {
                MapList = Enumerable.Range(0, _size).Select(z => Enumerable.Repeat(-1, _size).ToList()).ToList();
                MapList[_startPos.z][_startPos.x] = 0;
                _tileCount = 1;
                _goalPos = new Vector3Int { x = 0, z = 0 };
                GetTilePositions();
            }
        }

        void GetTilePositions()
        {
            Vector3Int position = _startPos;
            while (true)
            {
                position = DecideNextPositoin(position);   //次のposition確定と候補着色
                if (_goalPos.x != 0 || _goalPos.z != 0) break;
                MapList[position.z][position.x] = 0;       //次のposition着色
                _tileCount++;
            }
        }

        Vector3Int DecideNextPositoin(Vector3Int position)
        {
            //-1:none/0:tile/1:wall
            var options = new List<Vector3Int>();
            //4方向のうち範囲内で0:tile以外のマスを追加
            //right
            if (position.x + 1 < _size && 0 != MapList[position.z][position.x + 1])
                options.Add(new Vector3Int { z = position.z, x = position.x + 1 });
            //left
            if (0 < position.x && 0 != MapList[position.z][position.x - 1])
                options.Add(new Vector3Int { z = position.z, x = position.x - 1 });
            //up
            if (position.z + 1 < _size && 0 != MapList[position.z + 1][position.x])
                options.Add(new Vector3Int { z = position.z + 1, x = position.x });
            //down
            if (0 < position.z && 0 != MapList[position.z - 1][position.x])
                options.Add(new Vector3Int { z = position.z - 1, x = position.x });


            if (options.Count < 1)
            {
                _goalPos = new Vector3Int { x = -1, z = -1 };
                return _goalPos;
            }
            options.ForEach(pos => MapList[pos.z][pos.x] = 1);
            var idx = random.Next(0, options.Count);
            return options[idx];
        }

        public void IndicateMap()
        {
            Console.WriteLine($"\r\nsize:{_size},poprate:{_popRate},tile:{_tileCount}");
            //for(var z=_size-1;z>=0;z--)
            for (var z = 0; z < _size; z++)
            {
                var line = "";
                Enumerable.Range(0, _size).ToList().ForEach(x =>
                {
                    line += z == _startPos.z && x == _startPos.z ? "◎" :
                            z == _goalPos.z && x == _goalPos.z ? "×" :
                               MapList[z][x] == 0 ? TrueFromRate(_popRate) ? TrueFromRate(5) ? "★" : "◆" : "■" :
                                    MapList[z][x] == 1 ? "◇" : "□";
                });
                Console.WriteLine(line);
            }
            Console.ReadKey();
        }

        public static bool TrueFromRate(int rate)
        {
            var rates = Enumerable.Range(0, 100).ToDictionary(x => x, y => false);
            Enumerable.Range(0, rate).ToList().ForEach(x => rates[x] = true);
            return rates[random.Next(0, 100)];
        }
    }
}
