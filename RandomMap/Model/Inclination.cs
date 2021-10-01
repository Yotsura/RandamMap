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
        //左上始まりなのではじめは右下行きやすく。
        int _up { get; set; } = 10;
        int _right { get; set; } = 0;
        int _down { get; set; } = 0;
        int _left { get; set; } = 10;

        public int HotDirection
            => new List<(int dir, int val)> { (1, _up), (2, _right), (3, _down), (4, _left) }
                .OrderBy(x => x.val).First().dir;
        public string Info => $"({_up}/{_right}/{_down}/{_left})";

        public void Up()
        {
            _up++;
        }
        public void Right()
        {
            _right++;
        }
        public void Down()
        {
            _down++;
        }
        public void Left()
        {
            _left++;
        }

    }
}
