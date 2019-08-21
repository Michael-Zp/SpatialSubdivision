using System.Collections.Generic;
using System.Numerics;

namespace SpatialSubdivision
{
    public class Circle
    {
        public Vector2 Center;
        public float Radius;

        public float MinX;
        public float MaxX;
        public float MinY;
        public float MaxY;

        public int ID;

        public byte CenteroidCellType;
        public byte OtherCellTypes;

        public Dictionary<byte, int> CollidingCellIDs = new Dictionary<byte, int>();


        private static int _nextId = 0;


        public Circle(Vector2 center, float radius)
        {
            _nextId++;
            ID = _nextId;

            Center = center;
            Radius = radius;

            MinX = center.X - radius;
            MaxX = center.X + radius;
            MinY = center.Y - radius;
            MaxY = center.Y + radius;
        }

        public void AddCellType(byte type, bool centeroid)
        {
            if(centeroid)
            {
                CenteroidCellType = type;
            }
            OtherCellTypes |= type;
        }
    }
}
