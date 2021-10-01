using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RandomMapTest.Model
{
    /// <summary>
    /// 1:uper 2:right 3:lower 4:left
    /// </summary>
    public class TileDensity
    {
        public TileDensity(int size)
        {
            _mapSize = size;
        }

        int _mapSize;
        int _upperSide;
        int _ridhtSide;
        int _lowerSide;
        int _leftSide;

        public int LowDensity
            => new List<(int dir, int val)> { (1, _upperSide), (2, _ridhtSide), (3, _lowerSide), (4, _leftSide) }
                .OrderBy(x => x.val).First().dir;
        public string Info => $"({_upperSide}/{_ridhtSide}/{_lowerSide}/{_leftSide})";

        public void AddPosition(Vector3Int position)
        {
            if (position.z < _mapSize / 2) _upperSide++;
            else _lowerSide++;
            if (position.x < _mapSize / 2) _leftSide++;
            else _ridhtSide++;
        }
    }
}
