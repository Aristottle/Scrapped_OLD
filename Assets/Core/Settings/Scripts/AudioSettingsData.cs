using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "AudioSettingsData", menuName = "Settings/AudioSettingsData")]

public class AudioSettingsData : ScriptableObject
{
	
    #region Variables
    
    [Header("Volumes")] // All are percentages and should thus max at 100
    public int master_volume = 100;
     
    #endregion
    
}
