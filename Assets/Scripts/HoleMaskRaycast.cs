using UnityEngine;
using UnityEngine.UI;

public class HoleMaskRaycastFilter : MonoBehaviour, ICanvasRaycastFilter
{
    public Material holeMaterial;

    public bool IsRaycastLocationValid(Vector2 screenPos, Camera eventCamera)
    {
        if (holeMaterial == null)
            return true;

        // 获取 hole 参数
        Vector4 center = holeMaterial.GetVector("_HoleCenter");
        Vector4 size = holeMaterial.GetVector("_HoleSize");

        // 屏幕空间坐标转 UV（0~1）
        Vector2 screenSize = new Vector2(Screen.width, Screen.height);
        Vector2 uv = new Vector2(screenPos.x / screenSize.x, screenPos.y / screenSize.y);

        // 镂空区域判断
        if (Mathf.Abs(uv.x - center.x) < size.x / 2f &&
            Mathf.Abs(uv.y - center.y) < size.y / 2f)
        {
            // 如果点击在洞内 → 不拦截（false）
            return false;
        }

        return true;
    }
}
