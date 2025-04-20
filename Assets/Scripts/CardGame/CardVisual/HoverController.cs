using CardGame.Core;
using CardGame.Data;
using UnityEngine;

namespace CardGame.CardVisual
{
    public class HoverController : MonoBehaviour
    {
        private VisualController visual;
        private bool isHovered;

        private float swingTime;
        private Quaternion originalRotation;
        private SettingsData settingsData;
        private RectTransform rectTransform;

        private void Awake()
        {
            settingsData = GameManager.instance.settingsData;
        }

        private void Start()
        {
            visual = GetComponentInParent<VisualController>();
            originalRotation = transform.localRotation;
            rectTransform = GetComponent<RectTransform>();
        }

        private void Update()
        {
            isHovered = visual.targetCard.IsHovered;
            
            Quaternion targetRotation;

            if (isHovered)
            {
                RectTransformUtility.ScreenPointToLocalPointInRectangle(rectTransform, Input.mousePosition, null, out var localMousePos);

                // 归一化坐标（-1 ~ 1）
                Vector2 normalizedPos = new Vector2(
                    Mathf.Clamp(localMousePos.x / (rectTransform.rect.width * 0.5f), -1f, 1f),
                    Mathf.Clamp(localMousePos.y / (rectTransform.rect.height * 0.5f), -1f, 1f)
                );
                // 使用方向控制倾斜（乘角度上限）
                float tiltX = normalizedPos.y * settingsData.maxTiltAngle ;  // 上下
                float tiltY = -normalizedPos.x * settingsData.maxTiltAngle ;  // 左右
                targetRotation = Quaternion.Euler(tiltX,  tiltY, 0);
            }
            else
            {
                swingTime += Time.deltaTime * settingsData.swingSpeed;
                float xAngle = Mathf.Sin(swingTime) * settingsData.xSwingAmplitude;
                float yAngle = Mathf.Cos(swingTime) * settingsData.ySwingAmplitude;
                targetRotation = originalRotation * Quaternion.Euler(xAngle, yAngle, 0f);
            }
            // ✅ 插值时统一用 localRotation
            transform.localRotation = Quaternion.Lerp(transform.localRotation, targetRotation, Time.deltaTime * settingsData.rotateLerpSpeed/100);
        }
    }
}
