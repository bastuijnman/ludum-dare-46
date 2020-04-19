using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EquipmentSelectionGlow : MonoBehaviour
{
    Material mat;

    float floor = 0.0f;
    float ceil = 0.25f;
    // Start is called before the first frame update
    void Start()
    {
        mat = GetComponent<Renderer>().material;
        mat.EnableKeyword("_EMISSION");
    }

    // Update is called once per frame
    void Update()
    {
        float emission = floor + Mathf.PingPong (Time.time * 0.2f, ceil - floor);
        Color baseColor = Color.yellow;
        Color finalColor = baseColor * Mathf.LinearToGammaSpace (emission);
        mat.SetColor ("_EmissionColor", finalColor);
    }
}
