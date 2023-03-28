using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class HUDTimer : MonoBehaviour
{
	
    #region Variables
    
    TextMeshProUGUI text;
    Image panel;

    float time_elapsed = 0;
    bool is_timing = false;
     
    #endregion

    
    #region Events
    
     
     
    #endregion


    #region Mono Callbacks
    
    private void Awake() 
    {
        text = GetComponentInChildren<TextMeshProUGUI>();
        panel = GetComponent<Image>();

        text.enabled = false;
        panel.enabled = false;
    }

    private void Update() 
    {
        if (is_timing)
            TimerUpdate();
    }
     
    #endregion


    #region Private Methods
    
    private void TimerUpdate()
    {
        time_elapsed += Time.deltaTime;

        text.text = time_elapsed.ToString("0.00");
    }
     
    #endregion


    #region Public Methods
    
    public void StartTimer()
    {
        text.enabled = true;
        panel.enabled = true;

        time_elapsed = 0;
        is_timing = true;
    }

    public float StopTimer()
    {
        text.enabled = false;
        panel.enabled = false;

        is_timing = false;
        return time_elapsed;
    }
     
    #endregion
    
}
