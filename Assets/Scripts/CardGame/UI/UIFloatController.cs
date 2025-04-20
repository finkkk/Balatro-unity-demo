using CardGame.Core;
using UnityEngine;

namespace CardGame.UI
{
    public class UIFloatController : MonoBehaviour
    {
        public float radius = 100f;     // 抖动幅度（像素）
        public float speed = 1f;       // 转速（转一圈用几秒）
    
        private RectTransform rect;
        private Vector2 originalPos;
        private void Start()
        {
            rect = GameManager.instance.uiRootRectTransform;
            originalPos = rect.anchoredPosition;
        }
        
        private void Update()
        {
            float t = Time.time * speed;
            float x = Mathf.Cos(t) * radius;
            float y = Mathf.Sin(t) * radius * 0.6f; // 椭圆，Y稍微小一点更自然

            rect.anchoredPosition = originalPos + new Vector2(x, y);
        }
    }
}
