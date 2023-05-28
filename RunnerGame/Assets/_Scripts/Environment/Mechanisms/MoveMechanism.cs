using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveMechanism : Mechanism
{
    [SerializeField] Transform end; //move here when triggered
    [SerializeField] float moveTime = 1f; //how long it takes to move

    public override void Trigger()
    {
        StartCoroutine(Move());
    }

    //routine for moving the object
    IEnumerator Move()
    {
        Vector2 start = transform.position;
        Vector2 endPos = end.transform.position;

        float timer = 0f;
        while (timer < moveTime)
        {
            transform.position = Vector2.Lerp(start, endPos, timer / moveTime);
            timer += Time.deltaTime;
            yield return null;
        }

        transform.position = endPos;
    }

    private void OnDrawGizmos()
    {
        if (!end) return;

        Gizmos.color = Color.green;
        Gizmos.DrawLine(transform.position, end.position);
    }
}
