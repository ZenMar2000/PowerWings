using TMPro;
using UnityEngine;

[RequireComponent(typeof(Oscillator))]
public class OverloadWarningBehaviour : MonoBehaviour
{
    [SerializeField] private TMP_Text tmpWarningText;
    private PlayerInfoFollowPlayerBehaviour pInfo;
    private Oscillator oscillator;
    private string overloadText = "\\\\Overload";
    private string warningText = "{Warning}";
    void Start()
    {
        oscillator = GetComponent<Oscillator>();
        pInfo = GetComponentInParent<PlayerInfoFollowPlayerBehaviour>();
    }

    // Update is called once per frame
    void Update()
    {
        SetVisibility();
        SetText();
        TextColorOscillation();
    }

    private void SetVisibility()
    {
        if (pInfo.playerProjectileEmitterBehaviour.BulletsAccumulator >= GameInfo.WarningValue && !tmpWarningText.enabled)
        {
            tmpWarningText.enabled = true;
        }
        else if (pInfo.playerProjectileEmitterBehaviour.BulletsAccumulator < GameInfo.WarningValue && tmpWarningText.enabled
            && !pInfo.playerProjectileEmitterBehaviour.isOverloaded)
        {
            tmpWarningText.enabled = false;
        }
    }

    private void SetText()
    {
        if (pInfo.playerProjectileEmitterBehaviour.isOverloaded && tmpWarningText.text != overloadText)
        {
            tmpWarningText.text = overloadText;
        }
        else if (!pInfo.playerProjectileEmitterBehaviour.isOverloaded && tmpWarningText.text != warningText)
        {
            tmpWarningText.text = warningText;
        }
    }

    private void TextColorOscillation()
    {
        if (tmpWarningText.enabled)
        {
            if (pInfo.playerProjectileEmitterBehaviour.isOverloaded && oscillator.Frequency != 10)
            {
                oscillator.Frequency = 10;
            }
            else if (!pInfo.playerProjectileEmitterBehaviour.isOverloaded && oscillator.Frequency != 5)
            {
                oscillator.Frequency = 5;
            }

            tmpWarningText.color = new Color(255, oscillator.MaxValue + oscillator.Progress, 0);
        }
    }
}
