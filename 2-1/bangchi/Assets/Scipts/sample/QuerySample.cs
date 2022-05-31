using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class QuerySample : MonoBehaviour
{
    public void LinqSample()
    {
        int[] scores = new int[] { 97, 92, 81, 60 };

        IEnumerable<int> scoreQuery =
            from score in scores
            where score > 80
            select score;
        foreach(int i in scoreQuery)
        {
            Debug.Log(i + " ");
        }

        foreach(int i in scores.Where(x => x > 80))
        {
            Debug.LogWarning(i + " ");
        }
    }

    private void Start()
    {
        LinqSample();
    }
}
