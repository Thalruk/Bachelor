using System;
using System.Collections;
using TMPro;
using UnityEngine;

public enum LightState
{
    Green,
    [HideInInspector]
    Yellow,
    Red
}

public class JunctionLights : MonoBehaviour
{
    [SerializeField] LightState startingLightState;
    [SerializeField] float greenLightDuration;
    [SerializeField] float yellowLightDuration;
    [SerializeField] float redLightDuration;

    [SerializeField] Light junctionLight;
    public BoxCollider boxCollider;
    public TextMeshProUGUI text;

    public int carNumber;
    public float carTime;

    private void Awake()
    {
        boxCollider = GetComponent<BoxCollider>();
        switch (startingLightState)
        {
            case LightState.Green:
                StartCoroutine(nameof(GreenLight));
                break;
            case LightState.Yellow:
                StartCoroutine(nameof(YellowLight), LightState.Green);
                break;
            case LightState.Red:
                StartCoroutine(nameof(RedLight));
                break;
            default:
                break;
        }
    }
    IEnumerator GreenLight()
    {
        junctionLight.color = Color.green;
        boxCollider.isTrigger = true;
        yield return new WaitForSecondsRealtime(greenLightDuration);
        StartCoroutine(nameof(YellowLight), LightState.Green);
    }

    IEnumerator YellowLight(LightState state)
    {
        junctionLight.color = Color.yellow;
        yield return new WaitForSecondsRealtime(yellowLightDuration);
        if (state == LightState.Green)
        {
            StartCoroutine(nameof(RedLight));
        }

        if (state == LightState.Red)
        {
            boxCollider.isTrigger = true;
            StartCoroutine(nameof(GreenLight));
        }
    }

    IEnumerator RedLight()
    {
        junctionLight.color = Color.red;
        boxCollider.isTrigger = false;
        yield return new WaitForSecondsRealtime(redLightDuration);
        StartCoroutine(nameof(YellowLight), LightState.Red);
    }

    public double GetAverageTime()
    {
        //obiac do 2 miejsca po przecinku
        if (carNumber != 0)
        {
            return Math.Round(carTime / carNumber, 2);
        }
        else
        {
            return 0;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.TryGetComponent(out Car car))
        {
            carNumber++;
            carTime += car.GetActualTrafficTime();
            car.Invoke(nameof(car.ResetActualTrafficTime), 0.1f);
        }
        text.text = GetAverageTime().ToString();
    }
}
