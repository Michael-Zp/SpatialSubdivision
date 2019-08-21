using System.Collections.Generic;

namespace SpatialSubdivision
{
    public struct CircleInGridCell
    {
        public Circle Circle;
        public bool IsCenteroid;

        public CircleInGridCell(Circle circle, bool isCenteroid)
        {
            Circle = circle;
            IsCenteroid = isCenteroid;
        }
    }

    public class GridCell
    {
        public float MinX;
        public float MaxX;
        public float MinY;
        public float MaxY;

        public int ID;

        private static int _nextId = 0;
        
        public List<CircleInGridCell> Colliding = new List<CircleInGridCell>();

        public GridCell(float minX, float maxX, float minY, float maxY)
        {
            _nextId++;
            ID = _nextId;

            MinX = minX;
            MaxX = maxX;
            MinY = minY;
            MaxY = maxY;
        }
    }
}
