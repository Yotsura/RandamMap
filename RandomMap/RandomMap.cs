using System;
using System.Collections.Generic;
using System.Linq;
using RandomMapTest.Model;
//using UnityEngine;
using Random = System.Random;

namespace RandomMapTest
{
    public class RandomMap
    {

        public List<List<int>> MapData { get; set; }
        int TileCount => MapData.Sum(x => x.Count(y => y == 0));
        public List<(Vector3Int vector, int stat)> MapDic =>
            MapData.SelectMany((z, z_idx) => z.Select((x, x_idx) => (new Vector3Int { x = x_idx, z = z_idx }, x))).ToList();

        readonly int _size;
        readonly int _popRate;
        readonly int _minTile = 0;
        readonly Vector3Int _startPos = new Vector3Int { x = 0, z = 0 };
        Vector3Int _goalPos = new Vector3Int { x = 0, z = 0 };
        TileDensity Inclination;

        public int Seed { get; set; }
        static Random _random = new Random();

        public RandomMap(int size, int tileRate, int popRate)
        {
            _size = size;
            _popRate = popRate;
            _minTile = size * size * tileRate / 100;
        }

        public void CreateMap(int seed)
        {
            Seed = seed;
            _random = new Random(Seed);
            Mapping();
        }
        public void CreateMap()
        {
            Seed = new Random().Next();
            _random = new Random(Seed);
            Mapping();
        }

        void Mapping()
        {
            //mapの値 -1:none/0:tile/1:wall
            MapData = Enumerable.Range(0, _size).Select(z => Enumerable.Repeat(-1, _size).ToList()).ToList();
            Inclination = new TileDensity(_size);
            var nowPos = _startPos;
            //指定タイルに満たなかった場合はランダムな1:wallから再スタート
            while (TileCount < _minTile)
            {
                MapData[nowPos.z][nowPos.x] = 0;
                while (LayOutAWay(ref nowPos)) MapData[nowPos.z][nowPos.x] = 0;    //次のposition着色
                _goalPos = nowPos;
                nowPos = GetRandamWallPos();
                //IndicateMap();
            }
        }

        Vector3Int GetRandamWallPos()
        {
            var wallKeys = MapDic.Where(cell => cell.stat == 1).ToList();
            return wallKeys[_random.Next(0, wallKeys.Count - 1)].vector;
        }

        bool LayOutAWay(ref Vector3Int position)
        {
            var options = new List<(int direction, Vector3Int vector)>
            {
                (1, new Vector3Int { z = position.z - 1, x = position.x }), //upper z--
                (2, new Vector3Int { z = position.z, x = position.x + 1 }), //right x++
                (3, new Vector3Int { z = position.z + 1, x = position.x }), //lower z++
                (4, new Vector3Int { z = position.z, x = position.x - 1 }), //left x--
            }.Where(pos =>
                0 <= pos.vector.x && pos.vector.x < _size &&
                0 <= pos.vector.z && pos.vector.z < _size &&
                MapData[pos.vector.z][pos.vector.x] != 0).ToList();
            if (options.Count() < 1) return false;

            //四方に壁設置
            options.ForEach(pos => MapData[pos.vector.z][pos.vector.x] = 1);

            //一番密度の低い方角へ伸びる確率を上げる
            var aditional = options.FirstOrDefault(x => x.direction == Inclination.LowDensity);
            if (aditional.direction > 0) options.Add(aditional);

            position = options[_random.Next(0, options.Count)].vector;
            Inclination.AddPosition(position);
            return true;
        }

        //unityコンソール用
        //public void IndicateInfo()
        //{
        //    Debug.Log($"size:{_size},tileTile:{_minTile},poprate:{_popRate},tile:{TileCount},seed：{Seed}");
        //}

        public void IndicateMap()
        {
            Console.Clear();
            Console.WriteLine($"\r\nsize:{_size},poprate:{_popRate},tile:{TileCount}\r\nseed：{Seed}");
            Console.WriteLine($"inclination:{Inclination.LowDensity}{Inclination.Info}");
            for (var z = 0; z < _size; z++)
            {
                var line = "";
                Enumerable.Range(0, _size).ToList().ForEach(x =>
                {
                    line += z == _startPos.z && x == _startPos.x ? "●" :
                            z == _goalPos.z && x == _goalPos.x ? "★" :
                               MapData[z][x] == 0 ? TrueFromRate(_popRate) ? TrueFromRate(5) ? "☆" : "○" : "□" :
                                    MapData[z][x] == 1 ? "■" : "◇";
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
