using System.Collections.Generic;
using System.Numerics;

namespace SpatialSubdivision
{
    class Program
    {
        static void Main(string[] args)
        {
            List<Circle> entities = new List<Circle>();

            entities.Add(new Circle(new Vector2(-1.10f,  0.6f), 0.25f));
            entities.Add(new Circle(new Vector2(-0.60f,  0.1f), 0.40f));
            entities.Add(new Circle(new Vector2( 0.25f, -0.5f), 0.40f));
            entities.Add(new Circle(new Vector2( 0.60f , 0.4f), 0.30f));


            Grid grid = new Grid(3, 2, 1);
            
            foreach(var entity in entities)
            {
                grid.AddCircle(entity);
            }

        }
    }
}
