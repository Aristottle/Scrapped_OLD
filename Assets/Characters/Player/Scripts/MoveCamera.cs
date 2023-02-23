using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveCamera : MonoBehaviour
{

    /// <summary>
    /// This just smooths out the camera movement so it doesn't have that gross jitter.
    /// </summary>

    [SerializeField] Transform eyes_transform;
    [SerializeField] float lerp_speed = 50f;

    private void Start() {
        transform.position = eyes_transform.position;
        transform.rotation = eyes_transform.rotation;
    }

    // Update is called once per frame
    private void Update()
    {
        transform.rotation = Quaternion.Lerp(transform.rotation, eyes_transform.rotation, Time.deltaTime * lerp_speed);
        transform.position = Vector3.Lerp(transform.position, eyes_transform.position, Time.deltaTime * lerp_speed);
    }
}
