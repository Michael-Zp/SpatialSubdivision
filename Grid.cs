using System;

namespace SpatialSubdivision
{
    public class Grid
    {
        public GridCell[,] cells;
        public float MaxObjectSize;
        public float HalfObjectSize;
        public float CenterXTranslation;
        public float CenterYTranslation;

        public Grid(uint xDimensions, uint yDimensions, float maxObjectSize)
        {
            cells = new GridCell[xDimensions, yDimensions];
            MaxObjectSize = maxObjectSize;

            HalfObjectSize = maxObjectSize / 2;

            CenterXTranslation = cells.GetLength(0) * HalfObjectSize;
            CenterYTranslation = cells.GetLength(1) * HalfObjectSize;

            for (int x = 0; x < cells.GetLength(0); x++)
            {
                for(int y = 0; y < cells.GetLength(1); y++)
                {
                    float centerX = x * maxObjectSize + HalfObjectSize - CenterXTranslation;
                    float centerY = y * maxObjectSize + HalfObjectSize - CenterYTranslation;

                    cells[x, y] = new GridCell(centerX - HalfObjectSize, centerX + HalfObjectSize, centerY - HalfObjectSize, centerY + HalfObjectSize);
                }
            }
        }

        public void AddCircle(Circle circle)
        {
            int xCoord = (int)Math.Floor((circle.Center.X + CenterXTranslation) / MaxObjectSize);
            int yCoord = (int)Math.Floor((circle.Center.Y + CenterYTranslation) / MaxObjectSize);

            cells[xCoord, yCoord].Centeroids.Add(circle);

            for(int x = -1; x <= 1; x++)
            {
                for(int y = -1; y <= 1; y++)
                {
                    if(y == 0 && x == 0)
                    {
                        cells[xCoord, yCoord].Colliding.Add(circle);
                        continue;
                    }

                    if(IsCollding(circle, xCoord + x, yCoord + y))
                    {
                        cells[xCoord + x, yCoord + y].Colliding.Add(circle);
                    }
                }
            }
        }

        private bool IsCollding(Circle circle, int xCenter, int yCenter)
        {
            if(xCenter < 0 || xCenter > cells.GetLength(0) - 1 || yCenter < 0 || yCenter > cells.GetLength(1) - 1)
            {
                return false;
            }

            bool xIntersect = (circle.MaxX > cells[xCenter, yCenter].MinX) && (circle.MinX < cells[xCenter, yCenter].MaxX);
            bool yIntersect = (circle.MaxY > cells[xCenter, yCenter].MinY) && (circle.MinY < cells[xCenter, yCenter].MaxY);

            return xIntersect && yIntersect;
        }
    }
}
