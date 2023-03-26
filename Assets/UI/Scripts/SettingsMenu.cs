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
    [SerializeField] Slider master_volume_slider;
    [SerializeField] TMPro.TextMeshProUGUI master_volume_text;

    [Header("References")]
    [SerializeField] UIManager ui_manager;

    [Header("Data")]
    [SerializeField] PlayerControlsData controls_so;
    [SerializeField] AudioSettingsData audio_so;

    #endregion


    #region Mono Callbacks

    private void Awake()
    {
        // Mouse sensitivity
        mouse_sense_slider.value = controls_so.mouse_sensitivity;
        UpdateMouseSensitivityUI();

        // Master volume
        mouse_sense_slider.value = audio_so.master_volume;
        SetMasterVolume();
    }

    #endregion


    #region Private Methods

    private void UpdateMouseSensitivityUI()
    {
        mouse_sense_text.text = $"Mouse Sense = {controls_so.mouse_sensitivity.ToString("0.00")}";
    }

    private void UpdateMasterVolumeUI()
    {
        master_volume_text.text = $"Master Volume = {audio_so.master_volume}";
    }

    #endregion


    #region Public Methods

    public void SetMouseSensitivity()
    {
        controls_so.mouse_sensitivity = mouse_sense_slider.value;

        UpdateMouseSensitivityUI();
    }
    
    public void SetMasterVolume()
    {
        audio_so.master_volume = Mathf.RoundToInt(master_volume_slider.value);

        // Actually set the volume
        AudioListener.volume = audio_so.master_volume / 100f;

        UpdateMasterVolumeUI();
    }

    #endregion

}
