using System.Collections.Generic;

namespace SpatialSubdivision
{
    public class GridCell
    {
        public float MinX;
        public float MaxX;
        public float MinY;
        public float MaxY;

        public List<Circle> Centeroids = new List<Circle>();
        public List<Circle> Colliding = new List<Circle>();

        public GridCell(float minX, float maxX, float minY, float maxY)
        {
            MinX = minX;
            MaxX = maxX;
            MinY = minY;
            MaxY = maxY;
        }
    }
}
