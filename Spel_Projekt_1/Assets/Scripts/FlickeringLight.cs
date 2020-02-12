using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlickeringLight : MonoBehaviour
{
    Light testLight;
    public float minWaitTime;
    public float maxWaitTime;

    // Start is called before the first frame update
    void Start()
    {
        testLight = GetComponent<Light>();
        StartCoroutine(GetFlashing());
    }

    private IEnumerator GetFlashing()
    {
        while (true)
        {
            yield return new WaitForSeconds(Random.Range(minWaitTime, maxWaitTime));
            testLight.enabled = !testLight.enabled;
        }
    }
}
