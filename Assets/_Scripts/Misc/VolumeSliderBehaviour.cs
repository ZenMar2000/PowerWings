using UnityEngine;
using UnityEngine.UI;
using static SharedLogics;

public class VolumeSliderBehaviour : MonoBehaviour
{
    [SerializeField] private SliderType sliderType;
    private void Start()
    {
        Slider slider = GetComponent<Slider>();
        switch (sliderType)
        {
            case SliderType.MUSIC:
                slider.value = GameInfo.MusicVolume;
                break;

            case SliderType.EFFECTS:
                slider.value = GameInfo.EffectsVolume;
                break;

            default:
                slider.value = 1;
                break;
        }
    }
}
