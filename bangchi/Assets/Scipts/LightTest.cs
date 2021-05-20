using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.Rendering.Universal;

public class LightTest : MonoBehaviour
{
    Light2D light;

    [SerializeField]
    private float speed = 0.5f;
    public float nextTime;
    private void Awake()
    {
        light = GetComponent<Light2D>();
    }
    void Start()
    {
        StartCoroutine(FadeIn());
    }

    void Update()
    {
        
    }

    public IEnumerator ChangeBackground()
    {
        StartCoroutine(FadeOut());
        yield return new WaitForSeconds(0.5f);
        light.color = Random.ColorHSV();
        StartCoroutine(FadeIn());
    }

    IEnumerator FadeIn()
    {
        nextTime = Time.time + 0.5f;
        while(nextTime > Time.time)
        {
            light.pointLightOuterRadius += speed * Time.deltaTime;
            yield return new WaitForSeconds(0f);
        }
    }

    IEnumerator FadeOut()
    {
        nextTime = Time.time + 0.5f;
        while (nextTime > Time.time)
        {
            light.pointLightOuterRadius -= speed * Time.deltaTime;
            yield return new WaitForSeconds(0f);
        }
    }
}
