using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyTime : MonoBehaviour
{
    public int destroyTimer = 1;

    private void Awake()
    {
        destroyTimer = Random.Range(5, 10);
        Destroy(gameObject, destroyTimer);
    }
}
