using UnityEngine;

namespace DefaultNamespace
{
    public static class ToolsUtils
    {
        public static Color GetCosineColor(float angle)
        {
            Vector4 colorVec = new Vector4();
            var rads = Mathf.Deg2Rad * angle / (2 * Mathf.PI);
            colorVec.x = 0.5f + 0.5f * Mathf.Cos(2 * Mathf.PI * (1f * rads + 0f));
            colorVec.y = 0.5f + 0.5f * Mathf.Cos(2 * Mathf.PI * (1f * rads + 0.3333f));
            colorVec.z = 0.5f + 0.5f * Mathf.Cos(2 * Mathf.PI * (1f * rads + 0.6667f));
            colorVec.w = 1f;
            return colorVec;
        }
    }
}