using System.Collections.Generic;
using System.Linq;
using CardGame.Core;
using CardGame.Data;
using UnityEngine;

namespace CardGame.CardVisual
{
    public class VisualManager : MonoBehaviour
    {
        private SettingsData settingsData;
        private GameObject visualPrefab;

        private List<VisualController> visuals;
        
        private void Awake()
        {
            settingsData = GameManager.instance.settingsData;
        }

        private void Start()
        {
            visuals = GetComponentsInChildren<VisualController>().ToList();
        }

        public VisualController InstantiateVisual()
        {
            GameObject visual = Instantiate(settingsData.visualPrefab, transform);
            return visual.GetComponent<VisualController>();
        }
    }
}
