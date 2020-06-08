using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using TestCodes.Models;

namespace TestCodes
{
    public class RandomTown
    {
        public List<List<int>> _mapList;
        int _mapSizeW;
        int _mapSizeH;
        int _wayW;
        Vector3Int _buildingMin;
        Vector3Int _buildingMax;
        int _id = 0;
        System.Random _random;
        public RandomTown(int sizeH, int sizeW, int wayW, int seed, Vector3Int builMin, Vector3Int builMax)
        {
            _mapSizeW = sizeW;
            _mapSizeH = sizeH;
            _wayW = wayW;
            _random = new System.Random(seed);
            _buildingMin = builMin;
            _buildingMax = builMax;
            //判定用マップ初期化
            _mapList = Enumerable.Range(0, _mapSizeW).Select(x
                => new List<int>(Enumerable.Range(0, _mapSizeH).Select(y => -1).ToList())).ToList();
        }

        public void SplitMap(int streetCntX, int streetCntZ, int delimiterW)
        {
            if (_mapSizeW - delimiterW * streetCntX < (streetCntX + 1) * _buildingMax.x) return;
            if (_mapSizeH - delimiterW * streetCntZ < (streetCntX + 1) * _buildingMax.z) return;
            var xlist = GetStreetPos(streetCntX, _buildingMax.x, delimiterW);
            var zlist = GetStreetPos(streetCntZ, _buildingMax.z, delimiterW);

            //道幅分0埋めで返す

            foreach (var w in Enumerable.Range(0, delimiterW))
            {
                foreach (var z in zlist)
                {
                    _mapList[z + w - 1] = _mapList[z + w - 1].Select(x => x = -2).ToList();
                }
                foreach (var x in xlist)
                {
                    _mapList.ForEach(z => z[x + w - 1] = -2);
                }
            }
            //IndicateMap();
            return;
        }

        List<int> GetStreetPos(int streetCnt, int buildingMax, int delimiterW)
        {
            var list = new List<int>();
            var rearLength = _mapSizeW;
            var pos = 0;
            while (list.Count < streetCnt)
            {
                var street = _random.Next(buildingMax, rearLength - ((buildingMax + delimiterW) * (streetCnt - list.Count)));
                list.Add(street + pos);
                pos = street + delimiterW;
                rearLength -= pos;
            }
            return list;
        }

        public List<BuildingObject> GetBuildings()
        {
            var result = new List<BuildingObject>();
            foreach (var mapZ in Enumerable.Range(0, _mapSizeH))
            {
                foreach (var mapX in Enumerable.Range(0, _mapSizeW))
                {
                    BuildingObject randmObj = RandamBuilding();
                    var dummyScale =
                        new Vector3Int
                        {
                            x = randmObj.Scale.x + _wayW,
                            y = randmObj.Scale.y,
                            z = randmObj.Scale.z + _wayW
                        };
                    if (!CheckEmpty(mapZ, mapX, dummyScale)) continue;
                    //一回り大きく場所を取る
                    FillZeroInMap(mapZ, mapX, dummyScale);
                    //objのpositionとid情報更新してオブジェクトのリストに追加
                    _id++;
                    randmObj.Id = _id;
                    randmObj.Position = new Vector3Int { x = mapX + _wayW, y = 0, z = mapZ + _wayW };
                    result.Add(randmObj);
                    FillIdInMap(randmObj);
                }
            }
            //IndicateMap();
            Console.ReadKey();
            return result;
        }

        //ランダムサイズのビル生成
        public BuildingObject RandamBuilding()
        {
            return new BuildingObject
            {
                Position = new Vector3Int { x = 0, y = 0, z = 0 },
                Scale = new Vector3Int
                {
                    x = _random.Next(_buildingMin.x, _buildingMax.x),
                    y = _random.Next(_buildingMin.y, _buildingMax.y),
                    z = _random.Next(_buildingMin.z, _buildingMax.z)
                }
            };
        }

        bool CheckEmpty(int mapZ, int mapX, Vector3Int scale)
        {
            if (mapX + scale.x >= _mapSizeW || mapZ + scale.z >= _mapSizeH) return false;

            //横サイズが連続で空いているか
            for (var x = mapX; x < mapX + scale.x; x++)
            {
                if (_mapList[mapZ][x] != -1) return false;
            }
            ////最終座標が入るか？
            //if (_mapList[mapZ + scale.z - 1][mapX + scale.x - 1] != -1)
            //    return false;
            //最終列が入るか
            for (var z = mapZ; z < mapZ + scale.z; z++)
            {
                if (_mapList[z][mapX + scale.x] != -1) return false;
            }
            return true;
        }

        void FillZeroInMap(int mapZ, int mapX, Vector3Int scale)
        {
            for (var z = mapZ; z < mapZ + scale.z; z++)
            {
                for (var x = mapX; x < mapX + scale.x; x++)
                {
                    _mapList[z][x] = 0;
                }
            }
        }

        //以下デバッグ用
        void FillIdInMap(BuildingObject obj)
        {
            //_mapの指定座標から指定サイズをidで埋める
            for (var z = obj.Position.z; z < obj.Position.z + obj.Scale.z; z++)
            {
                for (var x = obj.Position.x; x < obj.Position.x + obj.Scale.x; x++)
                {
                    _mapList[z][x] = obj.Id;
                }
            }
        }

        public void IndicateMap()
        {
            Console.WriteLine();
            _mapList.ForEach(z =>
            {
                var line = "";
                z.ForEach(x =>
                {
                    line += x == -1 ? "  " : x == -2 ? "--" : x.ToString().PadLeft(2, '_');
                    line += ",";
                });
                Console.WriteLine(line);
            });
        }
    }
}
