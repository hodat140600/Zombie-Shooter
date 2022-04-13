using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightFlicker : MonoBehaviour
{
    Light theLight;

    [SerializeField]
    float minTime;
    [SerializeField]
    float maxTime;

    // Start is called before the first frame update
    void Start()
    {
        theLight = GetComponent<Light>();
        StartCoroutine(MakeLightFlicker());
    }

    IEnumerator MakeLightFlicker()
    {
        while (true)
        {
            yield return new WaitForSeconds(Random.Range(minTime, maxTime));
            theLight.enabled = !theLight.enabled;
        }
    }
}
