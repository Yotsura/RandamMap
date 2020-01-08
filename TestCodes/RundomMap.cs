using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TestCodes
{
    public class RandomMap
    {
        public Dictionary<string, bool> MapBase;
        public List<string> Root;
        readonly int Size;
        readonly int PopRate;
        int tileCount = 0;
        readonly string StartPos = "0:0";
        string GoalPos;

        public RandomMap(int size, int range1, int range2, int popRate)
        {
            Size = size;
            PopRate = popRate;
            //レンジ内のタイル数になるまでループ
            while (tileCount < range1 || range2 < tileCount)
            {
                Root = new List<string>();
                MapBase = new Dictionary<string, bool>();
                Enumerable.Range(0, size).ToList().ForEach(x => Enumerable.Range(0, size).ToList()
                    .ForEach(y => MapBase[x + ":" + y] = false));
                tileCount = 0;
                GetTilePositions(size);
            }
            IndicateMap();
        }

        void GetTilePositions(int size)
        {
            var positionStr = StartPos;
            var position = positionStr.Split(':').Select(x => int.Parse(x)).ToArray();

            while (position[0] > -1 && position[1] > -1)
            {
                MapBase[positionStr] = true;
                Root.Add(positionStr);
                tileCount++;
                positionStr = DecideNextPositoin(positionStr, size);
                position = positionStr.Split(':').Select(x => int.Parse(x)).ToArray();
            }
        }

        string DecideNextPositoin(string positionStr, int size)
        {
            var position = positionStr.Split(':').Select(x => int.Parse(x)).ToArray();

            var options = new List<string>();
            //4方向のうち範囲内でfalseのマスを追加
            if (position[0] - 1 >= 0 && !MapBase[(position[0] - 1) + ":" + (position[1])])
                options.Add((position[0] - 1) + ":" + (position[1]));  //up
            if (position[0] + 1 < size && !MapBase[(position[0] + 1) + ":" + (position[1])])
                options.Add((position[0] + 1) + ":" + (position[1]));  //down
            if (position[1] - 1 >= 0 && !MapBase[(position[0]) + ":" + (position[1] - 1)])
                options.Add((position[0]) + ":" + (position[1] - 1));  //left
            if (position[1] + 1 < size && !MapBase[(position[0]) + ":" + (position[1] + 1)])
                options.Add((position[0]) + ":" + (position[1] + 1));  //right

            if (options.Count < 1)
            {
                GoalPos = positionStr;
                return "-1:-1";
            }
            var idx = random.Next(0, options.Count);
            return options[idx];
        }
        static System.Random random = new System.Random();

        public static bool TrueFromRate(int rate)
        {
            var rates = Enumerable.Range(0, 100).ToDictionary(x => x, y => false);
            Enumerable.Range(0, rate).ToList().ForEach(x => rates[x] = true);
            return rates[random.Next(0, 100)];
        }

        void IndicateMap()
        {
            Console.WriteLine($"\r\nsize:{Size},poprate:{PopRate},tile:{tileCount}");

            Enumerable.Range(0, Size).ToList().ForEach(x =>
            {
                var line = "";
                Enumerable.Range(0, Size).ToList().ForEach(y =>
                {
                    line += x + ":" + y == StartPos ? "◎" :
                            x + ":" + y == GoalPos ? "×" :
                                !MapBase[x + ":" + y] ? "　" :
                                    TrueFromRate(PopRate) ? TrueFromRate(5) ? "★" : "■" : "□";
                });
                Console.WriteLine(line);
            });

            Console.ReadKey();
        }
    }

    public class RandomMap2
    {
        public Dictionary<string, bool> MapBase;
        public List<string> Root;
        readonly int Size;
        readonly int PopRate;
        int tileCount = 0;
        readonly string StartPos = "0:0";
        string GoalPos;

        public RandomMap2(int size, int range1, int range2, int popRate)
        {
            Size = size;
            PopRate = popRate;
            //レンジ内のタイル数になるまでループ
            while (tileCount < range1 || range2 < tileCount)
            {
                Root = new List<string>();
                MapBase = new Dictionary<string, bool>();
                Enumerable.Range(0, size).ToList().ForEach(x => Enumerable.Range(0, size).ToList()
                    .ForEach(y => MapBase[x + ":" + y] = false));
                tileCount = 0;
                GetTilePositions(size);
            }
            IndicateMap();
        }

        void GetTilePositions(int size)
        {
            var positionStr = StartPos;
            var position = positionStr.Split(':').Select(x => int.Parse(x)).ToArray();

            while (position[0] > -1 && position[1] > -1)
            {
                MapBase[positionStr] = true;
                Root.Add(positionStr);
                tileCount++;
                positionStr = DecideNextPositoin(positionStr, size);
                position = positionStr.Split(':').Select(x => int.Parse(x)).ToArray();
            }
        }

        string DecideNextPositoin(string positionStr, int size)
        {
            var position = positionStr.Split(':').Select(x => int.Parse(x)).ToArray();

            var options = new List<string>();
            //4方向のうち範囲内でfalseのマスを追加
            if (position[0] - 1 >= 0 && !MapBase[(position[0] - 1) + ":" + (position[1])])
                options.Add((position[0] - 1) + ":" + (position[1]));  //up
            if (position[0] + 1 < size && !MapBase[(position[0] + 1) + ":" + (position[1])])
                options.Add((position[0] + 1) + ":" + (position[1]));  //down
            if (position[1] - 1 >= 0 && !MapBase[(position[0]) + ":" + (position[1] - 1)])
                options.Add((position[0]) + ":" + (position[1] - 1));  //left
            if (position[1] + 1 < size && !MapBase[(position[0]) + ":" + (position[1] + 1)])
                options.Add((position[0]) + ":" + (position[1] + 1));  //right

            if (options.Count < 1)
            {
                GoalPos = positionStr;
                return "-1:-1";
            }
            var idx = random.Next(0, options.Count);
            return options[idx];
        }
        static System.Random random = new System.Random();

        public static bool TrueFromRate(int rate)
        {
            var rates = Enumerable.Range(0, 100).ToDictionary(x => x, y => false);
            Enumerable.Range(0, rate).ToList().ForEach(x => rates[x] = true);
            return rates[random.Next(0, 100)];
        }

        void IndicateMap()
        {
            Console.WriteLine($"\r\nsize:{Size},poprate:{PopRate},tile:{tileCount}");

            Enumerable.Range(0, Size).ToList().ForEach(x =>
            {
                var line = "";
                Enumerable.Range(0, Size).ToList().ForEach(y =>
                {
                    line += x+":"+y == StartPos ? "◎" :
                            x + ":" + y == GoalPos ? "×" :
                                !MapBase[x + ":" + y] ? "　" :
                                    TrueFromRate(PopRate) ? TrueFromRate(5) ? "★" : "■" : "□";
                });
                Console.WriteLine(line);
            });

            Console.ReadKey();
        }
    }
    
    public class RandomMap3
    {
        public List<List<int>> _mapList;
        readonly int _size;
        readonly int PopRate;
        int _minTile = 0;
        int _maxTile = 0;
        int _tileCount = 0;
        readonly Vector3Int _startPos = new Vector3Int { x = 0, z = 0 };
        Vector3Int _goalPos;
        static System.Random random = new System.Random();

        public RandomMap3(int size, int minTile, int maxTile, int popRate)
        {
            _size = size;
            PopRate = popRate;
            _minTile = minTile;
            _maxTile = maxTile;
        }

        public List<List<int>> RandomWall()
        {
            //指定タイル数になるまでループ
            while (_tileCount < _minTile || _maxTile < _tileCount)
            {
                _mapList = Enumerable.Range(0, _size).Select(z => new List<int>(Enumerable.Range(0, _size).Select(x => -1).ToList())).ToList();
                _mapList[_startPos.z][_startPos.x]=0;
                _tileCount = 1;
                _goalPos = new Vector3Int { x = 0, z = 0 };
                GetTilePositions2();
            }
            Console.ReadKey();
            return _mapList;
        }

        void GetTilePositions2()
        {
            Vector3Int position = _startPos;
            while (true)
            {
                position = DecideNextPositoin2(position);   //次のposition確定と候補着色
                if (_goalPos.x != 0 || _goalPos.z != 0) break;
                _mapList[position.z][position.x] = 0;       //次のposition着色
                _tileCount++;
                //IndicateMap();
            }
            IndicateMap();
        }

        Vector3Int DecideNextPositoin2(Vector3Int position)
        {
            //-1:none/0:tile/1:wall
            var options = new List<Vector3Int>();
            //4方向のうち範囲内でnoneマスを追加
            //right
            if (position.x+1 < _size && 0 != _mapList[position.z][position.x + 1])
                options.Add(new Vector3Int { z = position.z, x = position.x + 1 });
            //left
            if (0 < position.x && 0 != _mapList[position.z][position.x - 1])
                options.Add(new Vector3Int { z = position.z, x = position.x - 1 });
            //up
            if (position.z+1 < _size && 0 != _mapList[position.z + 1][position.x])
                options.Add(new Vector3Int { z = position.z + 1, x = position.x });
            //down
            if (0 < position.z && 0 != _mapList[position.z - 1][position.x])
                options.Add(new Vector3Int { z = position.z - 1, x = position.x });


            if (options.Count < 1)
            {
                _goalPos = new Vector3Int { x = -1, z = -1 };
                return _goalPos;
            }
            options.ForEach(pos => _mapList[pos.z][pos.x] = 1);
            var idx = random.Next(0, options.Count);
            return options[idx];
        }

        void IndicateMap()
        {
            Console.WriteLine($"\r\nsize:{_size},poprate:{PopRate},tile:{_tileCount}");
            for(var z=_size-1;z>=0;z--)
            {
                var line = "";
                Enumerable.Range(0, _size).ToList().ForEach(x =>
                {
                    line += z == _startPos.z && x == _startPos.z ? "◎" :
                            z == _goalPos.z && x == _goalPos.z ? "×" :
                               _mapList[z][x] == 0 ? TrueFromRate(PopRate) ? TrueFromRate(5) ? "★" : "■" : "□" :
                                    _mapList[z][x] == 1 ? "＿" : "";
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
