using UnityEngine;

public class CrystalPulse : MonoBehaviour
{
    public Material mat;
    private Color baseEmissionColor;

    //public float minIntensity;
    //public float maxIntensity;
    public float speed = 2f;

    private void Start()
    {
        Debug.Log(mat.GetColor("_EmissionColor"));
        // Store the original emission color
        baseEmissionColor = mat.GetColor("_EmissionColor");
    }

    private void Update()
    {
        float intensity = Mathf.Lerp(1f,15f,(Mathf.Sin(Time.time * speed) + 1f) * 1f);
        mat.SetColor("_EmissionColor", baseEmissionColor * intensity);
    }
}