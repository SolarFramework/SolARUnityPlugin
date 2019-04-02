using UnityEngine;

namespace SolAR
{
    public static class RectUtility
    {
        public static Rect RectLeft(ref Rect rect, float width, float margin = 0)
        {
            var res = new Rect(rect.xMin, rect.yMin, width, rect.height);
            rect.xMin += width + margin;
            return res;
        }

        public static Rect RectRight(ref Rect rect, float width, float margin = 0)
        {
            var res = new Rect(rect.xMax - width, rect.yMin, width, rect.height);
            rect.xMax -= width + margin;
            return res;
        }

        public static Rect RectBottom(ref Rect rect, float height, float margin = 0)
        {
            var res = new Rect(rect.xMin, rect.yMin, rect.width, height);
            rect.yMax -= height + margin;
            return res;
        }

        public static Rect RectTop(ref Rect rect, float height, float margin = 0)
        {
            var res = new Rect(rect.xMin, rect.yMin, rect.width, height);
            rect.yMin += height + margin;
            return res;
        }
    }
}
