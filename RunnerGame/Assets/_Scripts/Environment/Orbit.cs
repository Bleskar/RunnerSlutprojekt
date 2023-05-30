using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Orbit : MonoBehaviour
{
    [SerializeField] float speed = 1f;
    [SerializeField] float range = 1f;
    Vector2 startPosition;
    float progress;

    float Radians => progress * Mathf.PI * 2f;

    // Start is called before the first frame update
    void Start()
    {
        startPosition = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        progress += Time.deltaTime * speed;
        if (progress > 1f)
            progress--;
        else if (progress < 0f)
            progress++;

        transform.position = startPosition + new Vector2(Mathf.Cos(Radians), Mathf.Sin(Radians)) * range;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, range);
    }
}
