using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class InGameGUIBehaviour : MonoBehaviour
{
    [SerializeField] private TMP_Text TextScore;
    [SerializeField] private TMP_Text TextThreatLevel;

    [SerializeField] private Transform FullThreatBar;

    private int threatLevel => GameInfo.ThreatLevel;
    private int currentThreat => GameInfo.ThreatAccumulator;
    private float maxThreat => GameInfo.ThreatLevelUpThreshold;
    private int score => (int)GameInfo.PlayerScore;

    private void FixedUpdate()
    {
        FullThreatBar.localScale = new Vector3(Normalize(currentThreat, maxThreat), FullThreatBar.localScale.y, FullThreatBar.localScale.z);
        TextThreatLevel.text = $"Threat Level {threatLevel}";
        TextScore.text = $"Score\n{score}";
    }

    private float Normalize(float currentVal, float maxVal)
    {
        return currentVal / maxVal;
    }
}
