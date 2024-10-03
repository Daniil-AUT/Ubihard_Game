using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Mathematics;

[ExecuteInEditMode]
public class STINGRAY_Sun_Color : MonoBehaviour
{
    Light _light;
    [SerializeField] private float sunIntensity = 3;
    [SerializeField] private Color dayColour;
    [SerializeField] private Color eveningColour;

    [SerializeField] private float startAngle = 60f;
    private float timeOfDayOffset;

    void Start()
    {
        //set a beginning angle of the sun
        timeOfDayOffset = (startAngle + 90f) / 360f;
        transform.localEulerAngles = new Vector3(startAngle, 0, 0);
    }

    void Update()
    {
        if (_light == null)
        {
            _light = GetComponent<Light>();
        }

        //rotate the sun in the game
        if (Application.isPlaying)
        {
            float timeOfDay = Mathf.PingPong(Time.time * 0.01f + timeOfDayOffset, 1);
            transform.localEulerAngles = new Vector3(timeOfDay * 360 - 90, 0, 0);
        }

        //calculate the relationship of sun direction and horizon, change the light based on that
        float dotProduct = Vector3.Dot(-transform.forward, Vector3.up);
        float clampedDot = Mathf.Clamp((dotProduct + 0.9f), 0, 1);
        float topDot = (1 - Mathf.Clamp01(dotProduct)) * Mathf.Clamp01(Mathf.Sign(dotProduct));
        float bottomDot = (1 - Mathf.Clamp01(-dotProduct)) * Mathf.Clamp01(Mathf.Sign(-dotProduct));
        topDot = Mathf.Pow(math.smoothstep(0f, 0.9f, topDot), 5);
        bottomDot = Mathf.Pow(bottomDot, 5);

        //change the strength of the light
        _light.intensity = Mathf.Lerp(0.1f, sunIntensity, Mathf.Pow(clampedDot, 5));
        _light.color = Color.Lerp(dayColour, eveningColour, topDot + bottomDot);

        //avoid the sun to go over
        if (transform.localEulerAngles.x == -90)
        {
            transform.localEulerAngles = new Vector3(-89.9f, transform.eulerAngles.y, transform.eulerAngles.z);
        }


    }
}
