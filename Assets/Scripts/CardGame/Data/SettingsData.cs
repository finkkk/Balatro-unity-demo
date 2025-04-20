using UnityEngine;
namespace CardGame.Data
{
    [CreateAssetMenu(menuName = "GameData/SettingsData", fileName = "SettingsData")]
    public class SettingsData : ScriptableObject
    {
        [Header("生成数据设置")] 
        public int cardsToSpawn = 7;
        public GameObject slotPrefab;
        [Header("视觉效果设置")]
        public float positionSpeed = 20f;
        public float rotationSpeed = 20f;
        public float rotationOffset = 30f;
        public float selectOffset = 15f;
        public Vector2 shadowOffset;
        public AnimationCurve positionCurve;
        public AnimationCurve rotationCurve;
        public float yOffsetMultiplier = 5f;
        public GameObject visualPrefab;
        [Header("3D视效设置")]
        public float xSwingAmplitude = 25f;     // X轴上下仰俯
        public float ySwingAmplitude = 30f;     // Y轴左右摆动
        public float maxTiltAngle = 60f;
        public float rotateLerpSpeed = 50f;
        public float swingSpeed = 2f;          // 摆动速度
        
    }
}
