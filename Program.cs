using System;
using System.Collections.Generic;
using System.Numerics;
using System.Linq;

namespace SpatialSubdivision
{
    class Program
    {
        static void Main(string[] args)
        {
            //List<Circle> entities = new List<Circle>
            //{
            //    new Circle(new Vector2(-1.10f, 0.6f), 0.25f),
            //    new Circle(new Vector2(-0.60f, 0.1f), 0.40f),
            //    new Circle(new Vector2(-0.25f, -0.5f), 0.30f),
            //    new Circle(new Vector2(0.60f,  0.4f), 0.30f),
            //    new Circle(new Vector2(0.60f, -0.5f), 0.20f)
            //};


            List<Circle> entities = new List<Circle>
            {
                new Circle(new Vector2(-0.3f, 0.4f), 0.3f),
                new Circle(new Vector2( 0.4f, 0.4f), 0.3f)
            };


            Grid grid = new Grid(3, 2, entities);
            

            List<Tuple<Circle, Circle>> collisions = grid.GetCollisions();

        }
    }
}
