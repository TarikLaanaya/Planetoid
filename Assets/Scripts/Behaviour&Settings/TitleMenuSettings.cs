using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class TitleMenuSettings : MonoBehaviour
{
    public PlayerSettings PlayerSettings;
    public Slider WeaponSensitivitySlider;
    public TMP_InputField WeaponSensitivityInputField;

    void Start()
    {
        WeaponSensitivitySlider.minValue = 1;
        WeaponSensitivitySlider.maxValue = 1000;
        WeaponSensitivitySlider.wholeNumbers = true; // Ensure the slider only accepts whole numbers
        WeaponSensitivityInputField.text = PlayerSettings.WeaponSensitivity.ToString();
        WeaponSensitivitySlider.value = PlayerSettings.WeaponSensitivity;
    }

    void Update()
    {
        if (WeaponSensitivityInputField.isFocused)
        {
            if (int.TryParse(WeaponSensitivityInputField.text, out int value))
            {
                WeaponSensitivitySlider.value = value;
            }
        }
        else
        {
            WeaponSensitivityInputField.text = Mathf.RoundToInt(WeaponSensitivitySlider.value).ToString();
        }
    }

    public void SaveButton()
    {
        PlayerSettings.WeaponSensitivity = Mathf.RoundToInt(float.Parse(WeaponSensitivityInputField.text));
        PlayerSettings.SaveScore();
    }
}
