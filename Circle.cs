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

        public Circle(Vector2 center, float radius)
        {
            Center = center;
            Radius = radius;

            MinX = center.X - radius;
            MaxX = center.X + radius;
            MinY = center.Y - radius;
            MaxY = center.Y + radius;
        }
    }
}
