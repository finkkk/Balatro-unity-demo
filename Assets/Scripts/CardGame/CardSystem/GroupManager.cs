using System.Collections.Generic;
using System.Linq;
using CardGame.CardVisual;
using CardGame.Core;
using CardGame.Data;
using UnityEngine;

namespace CardGame.CardSystem
{
    public class GroupManager : MonoBehaviour
    {
        [HideInInspector] public CardController selectedCard;
        [HideInInspector] public CardController hoveredCard;
        [HideInInspector] public List<CardController> cards;
        [HideInInspector] public int cardsToSpawn;

        public bool isCurve;
        public bool IsSorting {set; get;}
        private List<SlotController> slots;
        private SettingsData settingsData;

        private void Awake()
        {
            settingsData = GameManager.instance.settingsData;
        }

        private void Start()
        {
            for (int i = 0; i < cardsToSpawn; i++)
            {
                Instantiate(settingsData.slotPrefab, transform);
            }
            
            cards = GetComponentsInChildren<CardController>().ToList();
            slots = GetComponentsInChildren<SlotController>().ToList();

            int cardCount = 1;
            

            foreach (CardController card in cards)
            {
                card.SelectedEvent.AddListener(OnSelected);
                card.PointerEnterEvent.AddListener(CardPointerEnter);
                card.PointerExitEvent.AddListener(CardPointerExit);
                card.OnBeginDragEvent.AddListener(BeginDrag);
                card.OnEndDragEvent.AddListener(EndDrag);
                card.OnDraggedEvent.AddListener(OnDrag);
                card.name = "Card" + cardCount;
                VisualController visual = transform.parent.Find("CardVisuals").GetComponent<VisualManager>().InstantiateVisual();
                visual.Init(card); // 只允许通过 Init 设置和启动逻辑
                card.visual = visual;
                cardCount++;
            }
            int slotCount = 1;
            foreach (SlotController slot in slots)
            {
                slot.name = "Slot" + slotCount;
                slot.slotIndex = slotCount;
                slotCount++;
            }

            if (isCurve)
            {
                ApplyCurveOffset();
            }
        }

        private void BeginDrag(CardController card)
        {
            selectedCard = card;
        }

        private void OnDrag(CardController card)
        {
            selectedCard = card;
        }
        
        private void EndDrag(CardController card)
        {
            selectedCard = null;
        }
        
        private void CardPointerEnter(CardController card)
        {
            hoveredCard = card;
            card.visual.PlayHoverEffect(); // 💡 悬浮时播放动画
        }
        
        private void CardPointerExit(CardController card)
        {
            hoveredCard = null;
            card.visual.StopHoverEffect(); // 💡 离开时停止动画
        }

        private void OnSelected(CardController card, bool selected)
        {
            
        }

        private void Update()
        {
            if ( !selectedCard) return;
            for (int i = 0; i < cards.Count; i++)
            {
                if (selectedCard.transform.position.x > cards[i].transform.position.x)
                {
                    if (selectedCard.GetSlotIndex() < cards[i].GetSlotIndex())
                    {
                        // 左移
                        MoveCard(i);
                        break;
                    }
                }

                if (selectedCard.transform.position.x < cards[i].transform.position.x)
                {
                    
                    if (selectedCard.GetSlotIndex() > cards[i].GetSlotIndex())
                    {
                        // 右移
                        MoveCard(i);
                        break;
                    }
                }
            }
        }

        private void MoveCard(int index)
        {
            IsSorting = true;

            Transform selectedCardParent = selectedCard.transform.parent;
            Transform moveCardParent = cards[index].transform.parent;

            cards[index].transform.SetParent(selectedCardParent);
            cards[index].transform.localPosition = new Vector3(0,cards[index].Selected ? settingsData.selectOffset : 0f ,0) ;
            selectedCard.transform.SetParent(moveCardParent);
            IsSorting = false;
        }
        
        private void ApplyCurveOffset()
        {
            int count = slots.Count;
            if (count == 0) return;
            for (int i = 0; i < count; i++)
            {
                SlotController slot = slots[i];
                float t = count == 1 ? 0.5f : (float)i / (count - 1); // 线性分布到 [0,1]
                // Y 方向位移
                float curveOffset = settingsData.positionCurve.Evaluate(t) * settingsData.yOffsetMultiplier;
                slot.YSlotOffset = curveOffset;
                Vector3 originalPos = slot.transform.localPosition;
                
                // 旋转角度（假设 rotationCurve 的输出是 -1~1，对应角度范围）
                float rotValue = settingsData.rotationCurve.Evaluate(t);
                float angle = rotValue * 30f; // 比如 maxRotationAngle = 30°
                // 设置位置
                slot.transform.localPosition = new Vector3(originalPos.x, curveOffset, originalPos.z);
                // 设置旋转（绕 Z 轴旋转，比如像花瓣）
                slot.transform.localRotation = Quaternion.Euler(0f, 0f, angle);
            }
        }
    }
}
