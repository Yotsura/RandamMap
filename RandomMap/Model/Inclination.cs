using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RandomMapTest.Model
{
    /// <summary>
    /// 1:up 2:right 3:down 4:left
    /// </summary>
    public class Inclination
    {
        public Inclination(int size)
        {
            _mapSize = size;
        }

        int _mapSize;

        int _up { get; set; }
        int _right { get; set; }
        int _down { get; set; }
        int _left { get; set; }

        public int HotDirection
            => new List<(int dir, int val)> { (1, _up), (2, _right), (3, _down), (4, _left) }
                .OrderBy(x => x.val).First().dir;
        public string Info => $"({_up}/{_right}/{_down}/{_left})";

        public void AddPosition(Vector3Int position)
        {
            if (position.z < _mapSize / 2) _up++;
            else _down++;
            if (position.x < _mapSize / 2) _left++;
            else _right++;
        }
    }
}
