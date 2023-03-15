using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputHandler : MonoBehaviour
{
    public static Action primary_input;
    public static Action reload_input;

    // Update is called once per frame
    void Update()
    {
        // Shoot Input
        if (Input.GetButtonDown("Primary Action"))
            primary_input?.Invoke();

        // Reload Input
        if (Input.GetButtonDown("Reload"))
            reload_input?.Invoke();
    }
}
