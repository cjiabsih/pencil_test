using UnityEngine;

namespace DefaultNamespace
{
    public static class DrawingUtils
    {
        public static bool[] CreateDotMask(RectInt rect)
        {
            bool[] mask = new bool[rect.width * rect.height];
            for (int j = 0; j < rect.height; j++)
            {
                for (int i = 0; i < rect.width; i++)
                {
                    mask[i + j * rect.width] = true;
                }
            }

            return mask;
        }

        public static bool[] CreateLineMask(IntVector2 p1, IntVector2 p2, RectInt rect, IntVector2 textureSize, int brushSize)
        {
            bool[] mask = new bool[rect.width * rect.height];

            int lineWidth = Mathf.Abs(p2.x - p1.x);
            int lineHeight = Mathf.Abs(p2.y - p1.y);
            int size = Mathf.Max(lineWidth, lineHeight);

            //Drawing dot for each pixel on line
            for (int dotsCount = 0; dotsCount <= size; dotsCount++)
            {
                int xCoord = Mathf.Min(p1.x, p2.x);
                int yCoord = Mathf.Min(p1.y, p2.y); //Drawing from left to right, bottom to top

                if (p1.x == p2.x)
                {
                    yCoord += dotsCount;
                }
                else if (p1.y == p2.y)
                {
                    xCoord += dotsCount;
                }
                else if (lineWidth > lineHeight)
                {
                    xCoord += dotsCount;
                    yCoord = Mathf.RoundToInt((p1.y - p2.y) * xCoord / (1f * (p1.x - p2.x)) +
                                              p2.y - p2.x * (p1.y - p2.y) / (1f * (p1.x - p2.x)));
                }
                else
                {
                    yCoord += dotsCount;
                    xCoord = Mathf.RoundToInt((p1.x - p2.x) * yCoord / (1f * (p1.y - p2.y)) +
                                              p2.x - p2.y * (p1.x - p2.x) / (1f * (p1.y - p2.y)));
                }

                RectInt dotRect = CalculateFillRect(new IntVector2(xCoord, yCoord), textureSize, brushSize);
                bool[] dotMask = CreateDotMask(dotRect);
                for (int j = 0; j < dotRect.height; j++)
                {
                    for (int i = 0; i < dotRect.width; i++)
                    {
                        int x = dotRect.x - rect.x + i;
                        int y = dotRect.y - rect.y + j;
                        mask[x + y * rect.width] = mask[x + y * rect.width] || dotMask[i + j * dotRect.width];
                    }
                }
            }

            return mask;
        }


        public static RectInt CalculateFillRect(IntVector2 position, IntVector2 textureSize, int brushSize)
        {
            int offset = (int) ((brushSize - 1) / 2f);

            int xMin = Mathf.Max(0, position.x - offset);
            int xMax = Mathf.Min(textureSize.x, position.x + brushSize - offset);

            int yMin = Mathf.Max(0, position.y - offset);
            int yMax = Mathf.Min(textureSize.y, position.y + brushSize - offset);

            var rect = new RectInt(xMin, yMin, xMax - xMin, yMax - yMin);

            return rect;
        }

        public static RectInt CalculateFillRect(IntVector2 p1, IntVector2 p2, IntVector2 textureSize, int brushSize)
        {
            int offset = (int) ((brushSize - 1) / 2f);

            int xMin = Mathf.Max(0, Mathf.Min(p1.x, p2.x) - offset);
            int xMax = Mathf.Min(textureSize.x, Mathf.Max(p1.x, p2.x) + brushSize - offset);

            int yMin = Mathf.Max(0, Mathf.Min(p1.y, p2.y) - offset);
            int yMax = Mathf.Min(textureSize.y, Mathf.Max(p1.y, p2.y) + brushSize - offset);

            var rect = new RectInt(xMin, yMin, xMax - xMin, yMax - yMin);

            return rect;
        }

        public static void ModifyColors(ref Color[] colors, bool[] mask, RectInt rect, Color brushColor)
        {
            for (int j = 0; j < rect.height; j++)
            {
                for (int i = 0; i < rect.width; i++)
                {
                    if (mask[i + j * rect.width])
                    {
                        colors[i + j * rect.width] = brushColor;
                    }
                }
            }
        }
        
        
    }
}