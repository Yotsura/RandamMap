using System;
using System.Collections.Generic;
using System.Text;

namespace RandomMapTest.Models
{
    public class BuildingObject
    {
        public int Id { get; set; }
        public Vector3Int Position { get; set; }
        public Vector3Int Scale { get; set; }
    }
}
