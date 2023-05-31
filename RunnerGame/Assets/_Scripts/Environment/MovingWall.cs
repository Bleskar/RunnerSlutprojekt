using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingWall : MonoBehaviour
{
    Vector3 startPosition;
    public Vector2 speed;
    bool initialized;

    // Update is called once per frame
    void Update()
    {
        if (PlayerMovement.Instance.Frozen)
            return;

        transform.position += (Vector3)speed * Time.deltaTime;
    }

    private void OnEnable()
    {
        if (!initialized)
        {
            initialized = true;
            startPosition = transform.position;
            return;
        }

        transform.position = startPosition;
    }
}
