using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveMechanism : Mechanism
{
    [SerializeField] Vector2 endPoint; //move here when triggered
    [SerializeField] float moveTime = 1f; //how long it takes to move

    public override void Trigger()
    {
        StartCoroutine(Move());
    }

    IEnumerator Move()
    {
        Vector2 start = transform.position;

        float timer = 0f;
        while (timer < moveTime)
        {
            transform.position = Vector2.Lerp(start, endPoint, timer / moveTime);
            timer += Time.deltaTime;
            yield return null;
        }

        transform.position = endPoint;
    }
}
