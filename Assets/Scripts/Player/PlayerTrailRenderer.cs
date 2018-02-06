using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerTrailRenderer : MonoBehaviour
{
    [Header("----- SETTINGS -----")]
    
    public Material mat;
    public float[] speeds;
    public Color[] colors;
    public Material[] materials;

    [Header("----- DEBUG -----")]
    public float factor;
    public float currentSpeedPsoido;
    public float currentSpeed;

    void Start ()
    {

	}
	
	void Update ()
    {
        currentSpeed = GetComponent<Rigidbody>().velocity.magnitude;
        if (speeds.Length > 0)
        {
            for (int i = 0; i < speeds.Length; i++)
            {
                if (speeds.Length == 1 || i == 0)
                {
                    mat.color = materials[0].color;
                    mat.SetColor("_EmissionColor", materials[0].GetColor("_EmissionColor"));
                }
                else if (currentSpeed > speeds[i - 1] && currentSpeed < speeds[i])
                {
                    currentSpeedPsoido = Mathf.Clamp(GetComponent<Rigidbody>().velocity.magnitude, speeds[i - 1], speeds[i]);
                    if (speeds[i - 1] < speeds[i])
                        factor = (currentSpeedPsoido - speeds[i - 1]) / (speeds[i] - speeds[i - 1]);
                    else
                        Debug.Log("MinSpeed (" + speeds[i - 1] + ") and/or MaxSpeed (" + speeds[1] + ") hasn't been set properly.");
                    mat.color = Color.Lerp(materials[i - 1].color, materials[i].color, factor);
                    mat.SetColor("_EmissionColor", Color.Lerp(materials[i - 1].GetColor("_EmissionColor"), materials[i].GetColor("_EmissionColor"), factor));
                }
                else if (currentSpeed > speeds[i])
                {
                    mat.color = materials[i].color;
                    mat.SetColor("_EmissionColor", materials[i].GetColor("_EmissionColor"));
                }
                    //mat.color = colors[i];
            }
        }

        //mat.color = new Color(factor, 1-factor, 0, 0.5f);
	}
}
