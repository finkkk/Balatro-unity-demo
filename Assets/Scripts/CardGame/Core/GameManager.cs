using System;
using System.Collections.Generic;
using CardGame.CardSystem;
using CardGame.CardVisual;
using CardGame.Data;
using DG.Tweening;
using UnityEngine;

namespace CardGame.Core
{
    public class GameManager : MonoBehaviour
    {
        public static GameManager instance;
        
        public List<int> CardToSpawns;
        public List<GroupManager> groups;
        public SettingsData settingsData;
        public RectTransform uiRootRectTransform;
        
        private void Awake()
        {
            if (instance && instance != this)
            {
                Destroy(this);
                return;
            }
            instance = this;
        }

        private void Start()
        {
            for (int i = 0; i < groups.Count; i++)
            {
                groups[i].cardsToSpawn = CardToSpawns[i];
            }
        }
    }
}
