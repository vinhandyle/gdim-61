using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Defines a volume slider.
/// </summary>
public class VolumeSlider : MonoBehaviour
{
    [SerializeField] private Slider slider;
    [SerializeField] private string channel;

    private void OnEnable()
    {
        channel = GetComponentInChildren<Text>().text;
        slider.value = AudioController.Instance.GetVolume(channel);
    }

    /// <summary>
    /// Called when the slider is moved.
    /// </summary>
    public void OnValueChanged()
    {
        AudioController.Instance.ChangeVolume(channel, slider.value);
    }
}
