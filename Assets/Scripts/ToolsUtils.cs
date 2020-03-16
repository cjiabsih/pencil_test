using UnityEngine;
using UnityEngine.EventSystems;

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

        public static void FixMousePositionForEditor(this IntVector2 mousePosition, IntVector2 textureSize)
        {
            if (mousePosition.x > textureSize.x)
            {
                mousePosition.x = textureSize.x;
            }
            else if (mousePosition.x < 0)
            {
                mousePosition.x = 0;
            }

            if (mousePosition.y > textureSize.y)
            {
                mousePosition.y = textureSize.y;
            }
            else if (mousePosition.y < 0)
            {
                mousePosition.y = 0;
            }
        }

        public static bool CheckIsOverGui()
        {
#if UNITY_EDITOR
            if (EventSystem.current.IsPointerOverGameObject()) return true;
#else
           for (int i = 0; i < Input.touchCount; i++)
            {
                if (Input.touches[i].phase == TouchPhase.Began &&
                    EventSystem.current.IsPointerOverGameObject(Input.touches[i].fingerId))
                {
                    return true;
                }
            }
#endif
            return false;
        }
    }
}