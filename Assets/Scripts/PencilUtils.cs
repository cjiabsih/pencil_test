using UnityEngine;

namespace DefaultNamespace
{
    public static class PencilUtils
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