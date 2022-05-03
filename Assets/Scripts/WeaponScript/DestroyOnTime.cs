using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyOnTime : MonoBehaviour
{
    public float time = 1;

    private void Awake()
    {
        Destroy(gameObject, time);
    }
}
