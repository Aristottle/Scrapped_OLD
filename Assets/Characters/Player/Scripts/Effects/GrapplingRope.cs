using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrapplingRope : MonoBehaviour
{
    private Spring spring;
    private LineRenderer lr;
    private Vector3 grapple_position;
    public PlayerController player;
    public int quality;
    public float damper;
    public float strength;
    public float velocity;
    public float wave_count;
    public float wave_height;
    public AnimationCurve affect_curve;

    private void Awake()
    {
        lr = GetComponent<LineRenderer>();
        spring = new Spring();
        spring.SetTarget(0);
    }

    private void LateUpdate() {
        DrawRope();
    }

    public void DrawRope()
    {
        if (!player.IsGrappling()) 
        {
            grapple_position = player.grapple_firing_point.position;
            spring.Reset();
            if (lr.positionCount > 0)
                lr.positionCount = 0;
            return;
        }

        if (lr.positionCount == 0)
        {
            spring.SetVelocity(velocity);
            lr.positionCount = quality + 1;
        }

        spring.SetDamper(damper);
        spring.SetStrength(strength);
        spring.Update(Time.deltaTime);

        Vector3 up = Quaternion.LookRotation((player.grapple_point - player.grapple_firing_point.position).normalized) * Vector3.up;

        grapple_position = Vector3.Lerp(grapple_position, player.grapple_point, Time.deltaTime * 12f);

        for (int i = 0; i < quality + 1; i++)
        {
            float delta  = i / (float) quality;
            Vector3 offset = up * wave_height * Mathf.Sin(delta * wave_count * Mathf.PI) * spring.Value * affect_curve.Evaluate(delta);

            lr.SetPosition(i, Vector3.Lerp(player.grapple_firing_point.position, grapple_position, delta) + offset);
        }
    }
}
