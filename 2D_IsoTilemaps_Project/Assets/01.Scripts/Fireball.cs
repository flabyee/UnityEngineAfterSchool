using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class Fireball : MonoBehaviour
{
    List<Vector2> point = new List<Vector2>();

    Vector3 dir;

    private float t = 0;

    public float spd = 5;
    public float radiusA = 0.55f;
    public float radiusB = 0.45f;

    public GameObject master;
    Vector3 objectPos;

    void Start()
    {
        master = GameObject.Find("Player_Isometric_Witch");
        

        objectPos = master.transform.position + 3 * dir;

        point.Add(master.transform.position);
        point.Add(SetRandomBezierPoint2(master.transform.position));
        point.Add(SetRandomBezierPoint3(objectPos));
        point.Add(objectPos);
    }

    void Update()
    {
        if (t > 1) Destroy(gameObject);
        t += Time.deltaTime * spd;
        transform.position = MoveBezier();
    }

    public void SetDir(Vector3 d)
    {
        dir = d;
    }

    private Vector2 MoveBezier()
    {
        return new Vector2(
            FourPointBezier(point[0].x, point[1].x, point[2].x, point[3].x),
            FourPointBezier(point[0].y, point[1].y, point[2].y, point[3].y)
            );
    }

    private float FourPointBezier(float a, float b, float c, float d)
    {
        return Mathf.Pow(1 - t, 3) * a
            + Mathf.Pow(1 - t, 2) * 3 * t * b
            + Mathf.Pow(t, 2) * 3 * (1 - t) * c
            + Mathf.Pow(t, 3) * d;
    }

    public Vector2 SetRandomBezierPoint2(Vector2 origin)
    {
        return new Vector2(
            radiusA * Mathf.Cos(Random.Range(0, 360) * Mathf.Deg2Rad) + origin.x,
            radiusA * Mathf.Sin(Random.Range(0, 360) * Mathf.Deg2Rad) + origin.y
            );
    }
    public Vector2 SetRandomBezierPoint3(Vector2 origin)
    {
        return new Vector2(
            radiusB * Mathf.Cos(Random.Range(0, 360) * Mathf.Deg2Rad) + origin.x,
            radiusB * Mathf.Sin(Random.Range(0, 360) * Mathf.Deg2Rad) + origin.y
            );
    }
}
