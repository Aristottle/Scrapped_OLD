using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SettingsMenu : MonoBehaviour
{
    
    #region Variables

    [Header("UI Components")]
    [SerializeField] Slider mouse_sense_slider;
    [SerializeField] TMPro.TextMeshProUGUI mouse_sense_text;

    [Header("References")]
    [SerializeField] UIManager ui_manager;

    [SerializeField] PlayerControlsData settings_so;

    #endregion


    #region Mono Callbacks

    private void Start()
    {
        // Update the UI to match the current settings
        mouse_sense_slider.value = settings_so.mouse_sensitivity;
        UpdateMouseSensitivityUI();
    }

    #endregion


    #region Private Methods

    private void UpdateMouseSensitivityUI()
    {
        mouse_sense_text.text = $"Mouse Sense = {settings_so.mouse_sensitivity.ToString("0.00")}";
    }

    #endregion


    #region Public Methods

    public void SetMouseSensitivity()
    {
        settings_so.mouse_sensitivity = mouse_sense_slider.value;

        UpdateMouseSensitivityUI();
    }

    #endregion

}
