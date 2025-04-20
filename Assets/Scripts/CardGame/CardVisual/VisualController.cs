using CardGame.CardSystem;
using CardGame.Core;
using CardGame.Data;
using DG.Tweening;
using UnityEngine;

namespace CardGame.CardVisual
{
    public class VisualController : MonoBehaviour
    {
        [HideInInspector] public CardController targetCard;
        
        private Tween hoverTween; // 控制摇动
        private Tween scaleTween; // 控制缩放
        private Tween selectTween;

        private Transform shadow;
        private SettingsData settingsData;
        public void Init(CardController card)
        {
            targetCard = card;
            settingsData = GameManager.instance.settingsData;
            shadow = transform.Find("Shadow");
            shadow.localPosition = settingsData.shadowOffset;
        }

        private void Update()
        {
            if (!targetCard) return; // 如果你正在做选中动画，就不要干扰
            SmoothPosition();
            SmoothRotation();
        }

        private void SmoothPosition()
        {
            transform.position = Vector3.Lerp(transform.position, targetCard.transform.position, settingsData.positionSpeed/100);
        }
        private void SmoothRotation()
        {
            float offsetX = targetCard.transform.position.x - transform.position.x;

            // 如果目标在左边 → 逆时针旋转一点（正 Z）
            // 如果目标在右边 → 顺时针旋转一点（负 Z）
            float tiltZ = 0f;
            if (Mathf.Abs(offsetX) > 10f) // 加个阈值防止 jitter 抖动(只有当与目标距离大于10个像素才旋转)
            {
                tiltZ = offsetX < 0 ? settingsData.rotationOffset : -settingsData.rotationOffset;
            }

            Quaternion targetRotation = Quaternion.Euler(0, 0, tiltZ);
            transform.rotation = Quaternion.Lerp(transform.rotation, targetRotation,  settingsData.rotationSpeed/100);
        }
        
        public void PlayHoverEffect()
        {
            // 放大
            scaleTween?.Kill();
            scaleTween = transform.DOScale(1.15f, 0.2f).SetEase(Ease.OutBack);

            // 轻微左右摇摆
            hoverTween?.Kill();
            hoverTween = transform.DOShakeRotation(
                duration: 0.15f,
                strength: new Vector3(0, 0, 10f), // 只抖 Z 轴
                vibrato: 5,                      // 抖动次数
                randomness: 10,
                fadeOut: true                     // ✅关键：自动减弱
            );
        }

        public void StopHoverEffect()
        {
            scaleTween?.Kill();
            hoverTween?.Kill();

            // 还原缩放和旋转
            transform.DOScale(1f, 0.2f).SetEase(Ease.OutBack);
            transform.DORotate(Vector3.zero, 0.2f).SetEase(Ease.OutBack);
        }
        public void SelectedAnimation()
        {
            selectTween?.Kill();
            Sequence seq = DOTween.Sequence();
            //seq.Append(transform.DOScale(1.2f, 0.18f).SetEase(Ease.OutBack));
            //seq.Join(transform.DOScale(1.15f, 0.15f).SetEase(Ease.OutBack));
            seq.Join(transform.DOLocalMoveY(settingsData.selectOffset, 0.1f).SetEase(Ease.OutBack));// 回到初始位置
            selectTween = seq;
        }

        public void UnSelectedAnimation()
        {
            selectTween?.Kill();
            Sequence seq = DOTween.Sequence();
            seq.Append(transform.DOScale(1.2f, 0.18f).SetEase(Ease.OutBack));
            seq.Join(transform.DOScale(1.15f, 0.15f).SetEase(Ease.OutBack));
            seq.Join(transform.DOLocalMoveY(targetCard.originSlot.YSlotOffset, 0.2f).SetEase(Ease.OutBack));// 回到初始位置
            selectTween = seq;
        }
        
        public void OnPointerDownAnimation()
        {
            scaleTween?.Kill();
            scaleTween = transform.DOScale(1.35f, 0.15f).SetEase(Ease.OutBack);
            shadow.localPosition += new Vector3(7, -7, 0);
        }

        public void OnPointerUpAnimation()
        {
            scaleTween?.Kill();
            scaleTween = transform.DOScale(1.15f, 0.15f).SetEase(Ease.OutBack);
            shadow.localPosition = settingsData.shadowOffset;
        }

    }
}
