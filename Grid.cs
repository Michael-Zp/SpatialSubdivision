using System;
using System.Collections.Generic;

namespace SpatialSubdivision
{
    public class Grid
    {
        public GridCell[,] cells;
        public List<GridCell>[] CollisionGroups;
        public float MaxObjectSize;
        public float HalfObjectSize;
        public float CenterXTranslation;
        public float CenterYTranslation;

        public Grid(uint xDimensions, uint yDimensions, IList<Circle> circles)
        {
            CollisionGroups = new List<GridCell>[4]
            {
                new List<GridCell>(),
                new List<GridCell>(),
                new List<GridCell>(),
                new List<GridCell>()
            };

            cells = new GridCell[xDimensions, yDimensions];

            MaxObjectSize = 0;

            for (int i = 0; i < circles.Count; i++)
            {
                circles[i].Radius *= (float)Math.Sqrt(2);
                MaxObjectSize = Math.Max(MaxObjectSize, circles[i].Radius * 1.5f);
            }

            HalfObjectSize = MaxObjectSize / 2;

            CenterXTranslation = cells.GetLength(0) * HalfObjectSize;
            CenterYTranslation = cells.GetLength(1) * HalfObjectSize;

            for (int x = 0; x < cells.GetLength(0); x++)
            {
                for (int y = 0; y < cells.GetLength(1); y++)
                {
                    float centerX = x * MaxObjectSize + HalfObjectSize - CenterXTranslation;
                    float centerY = y * MaxObjectSize + HalfObjectSize - CenterYTranslation;

                    cells[x, y] = new GridCell(centerX - HalfObjectSize, centerX + HalfObjectSize, centerY - HalfObjectSize, centerY + HalfObjectSize);
                    CollisionGroups[GetCollisionGroupIndex(x, y)].Add(cells[x, y]);
                }
            }

            for (int i = 0; i < circles.Count; i++)
            {
                AddCircle(circles[i]);
            }
        }


        private void AddCircle(Circle circle)
        {
            int xCoord = (int)Math.Floor((circle.Center.X + CenterXTranslation) / MaxObjectSize);
            int yCoord = (int)Math.Floor((circle.Center.Y + CenterYTranslation) / MaxObjectSize);


            for (int x = -1; x <= 1; x++)
            {
                for (int y = -1; y <= 1; y++)
                {
                    int currX = xCoord + x;
                    int currY = yCoord + y;

                    bool isCenteroid = y == 0 && x == 0;

                    if (IsCollding(circle, currX, currY))
                    {
                        circle.AddCellType(GetCellType(currX, currY), isCenteroid);
                        circle.CollidingCellIDs.Add(GetCellType(currX, currY), cells[currX, currY].ID);
                        cells[currX, currY].Colliding.Add(new CircleInGridCell(circle, isCenteroid));
                    }
                }
            }
        }

        private bool IsCollding(Circle circle, int xCenter, int yCenter)
        {
            if (xCenter < 0 || xCenter > cells.GetLength(0) - 1 || yCenter < 0 || yCenter > cells.GetLength(1) - 1)
            {
                return false;
            }

            bool xIntersect = (circle.MaxX > cells[xCenter, yCenter].MinX) && (circle.MinX < cells[xCenter, yCenter].MaxX);
            bool yIntersect = (circle.MaxY > cells[xCenter, yCenter].MinY) && (circle.MinY < cells[xCenter, yCenter].MaxY);

            return xIntersect && yIntersect;
        }


        public List<Tuple<Circle, Circle>> GetCollisions()
        {
            List<Tuple<Circle, Circle>> collisions = new List<Tuple<Circle, Circle>>();
            for (int g = 0; g < CollisionGroups.Length; g++)
            {
                byte currentCellType = (byte)Math.Pow(2, g);

                foreach (var cell in CollisionGroups[g])
                {
                    for (int i = 0; i < cell.Colliding.Count; i++)
                    {
                        for (int k = i + 1; k < cell.Colliding.Count; k++)
                        {
                            if(i == k)
                            {
                                continue;
                            }

                            if (!(cell.Colliding[i].IsCenteroid || cell.Colliding[k].IsCenteroid))
                            {
                                continue;
                            }

                            if (SkipTestOne(cell.Colliding[i].Circle, cell.Colliding[k].Circle, currentCellType))
                            {
                                Console.WriteLine("Skipped test (type 1) between: " + cell.Colliding[i].Circle.ID + " , " + cell.Colliding[k].Circle.ID);
                                continue;
                            }

                            if (SkipTestTwo(cell.Colliding[i].Circle, cell.Colliding[k].Circle))
                            {
                                Console.WriteLine("Skipped test (type 2) between: " + cell.Colliding[i].Circle.ID + " , " + cell.Colliding[k].Circle.ID);
                                continue;
                            }

                            float distance2 = (cell.Colliding[i].Circle.Center - cell.Colliding[k].Circle.Center).LengthSquared();
                            float radius2Cell1 = (float)Math.Pow(cell.Colliding[i].Circle.Radius, 2);
                            float radius2Cell2 = (float)Math.Pow(cell.Colliding[k].Circle.Radius, 2);

                            if (distance2 < (radius2Cell1 + radius2Cell2))
                            {
                                collisions.Add(Tuple.Create(cell.Colliding[i].Circle, cell.Colliding[k].Circle));
                            }
                        }
                    }
                }
            }

            return collisions;
        }

        private bool SkipTestOne(Circle circleA, Circle circleB, byte currentCellType)
        {
            if (circleA.CenteroidCellType < currentCellType && ((circleA.CenteroidCellType & circleB.OtherCellTypes) != 0))
            {
                return true;
            }

            return false;
        }

        private bool SkipTestTwo(Circle circleA, Circle circleB)
        {
            byte sharedTypes = (byte)(circleA.OtherCellTypes & circleB.OtherCellTypes);
            if (sharedTypes != 0 && circleA.CenteroidCellType != circleB.CenteroidCellType)
            {
                bool shareDifferentCells = true;

                for (byte i = 1; i < 0b10000; i *= 2)
                {
                    if ((sharedTypes & i) != 0 && circleA.CenteroidCellType != i)
                    {
                        shareDifferentCells &= (circleA.CollidingCellIDs[i] != circleB.CollidingCellIDs[i]);
                    }
                }

                return shareDifferentCells;
            }

            return false;
        }

        private byte GetCellType(int x, int y)
        {
            return (byte)Math.Pow(2, GetCollisionGroupIndex(x, y));
        }

        private int GetCollisionGroupIndex(int x, int y)
        {
            return (byte)(x % 2 + (y % 2) * 2);
        }
    }
}
