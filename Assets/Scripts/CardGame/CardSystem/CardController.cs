using System;
using CardGame.CardVisual;
using CardGame.Core;
using CardGame.Data;
using DG.Tweening;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

namespace CardGame.CardSystem
{
    public class CardController : MonoBehaviour,IEndDragHandler,IBeginDragHandler,IDragHandler,IPointerEnterHandler,IPointerExitHandler,IPointerUpHandler,IPointerDownHandler,IPointerClickHandler
    {
        [HideInInspector] public UnityEvent<CardController> OnDraggedEvent;
        [HideInInspector] public UnityEvent<CardController> OnEndDragEvent;
        [HideInInspector] public UnityEvent<CardController> OnBeginDragEvent;
        [HideInInspector] public UnityEvent<CardController> PointerExitEvent;
        [HideInInspector] public UnityEvent<CardController> PointerEnterEvent;
        [HideInInspector] public UnityEvent<CardController,bool> SelectedEvent;
        [HideInInspector] public SlotController currentSlot;
        [HideInInspector] public SlotController originSlot;
        [HideInInspector] public VisualController visual;
        public bool IsDragging { get; set; }
        public bool IsHovered { get; set; }
        public bool Selected { get; set; }
        
        private SettingsData settingsData;
        private Tween selectTween;

        private void Awake()
        {
            settingsData = GameManager.instance.settingsData;
        }

        private void Start()
        {
            originSlot = transform.parent.GetComponent<SlotController>();
        }

        public void OnEndDrag(PointerEventData eventData)
        {
            transform.localPosition = Vector3.zero;
            IsDragging = false;
        }

        public void OnBeginDrag(PointerEventData eventData)
        {
            OnBeginDragEvent.Invoke(this);
            // 提到最上层
            visual.transform.SetAsLastSibling();
            IsDragging = true;
        }

        public void OnDrag(PointerEventData eventData)
        {
            transform.position= eventData.position;
            OnDraggedEvent.Invoke(this);
        }

        public void OnPointerEnter(PointerEventData eventData)
        {
            IsHovered = true;
            PointerEnterEvent.Invoke(this);
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            IsHovered = false;
            PointerExitEvent.Invoke(this);
        }
        
        public void OnPointerClick(PointerEventData eventData)
        {
            if (IsDragging) return;
            
            Selected = !Selected;
            SelectedEvent.Invoke(this,Selected);
            if (Selected)
            {
                transform.localPosition += new Vector3(0, settingsData.selectOffset, 0); 
                visual.SelectedAnimation();
            }
            else
            {
                transform.localPosition = Vector3.zero; 
                visual.UnSelectedAnimation();
            }
        }

        public void OnPointerUp(PointerEventData eventData)
        {
            visual.OnPointerUpAnimation();
        }

        public void OnPointerDown(PointerEventData eventData)
        {
            visual.OnPointerDownAnimation();
        }

        public int GetSlotIndex()
        {
            currentSlot = transform.parent.GetComponent<SlotController>();
            return transform.parent.CompareTag("Slot") ? currentSlot.slotIndex : 0;
        }


    }
}
