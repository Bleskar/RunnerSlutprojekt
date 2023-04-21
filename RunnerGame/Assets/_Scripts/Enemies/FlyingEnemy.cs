using UnityEngine;
using System.Collections;
public class FlyingEnemy : EnemyBase
{
    [SerializeField] float speed = 2f; //the speed at which the enemy flies
    [SerializeField] Transform[] points = new Transform[0]; //the points that the enemy flies between (starts at point 0)
    Vector3[] pointsPosition; //the positions of the patrol points,
                              //doing this so that the points can be children to the enemy to reduce clutter in the heirarchy
    [SerializeField] bool pingpong = true; //if true then it will go back and forth (start->end end->start)
                                           //, else it will loop it's movement (start->end->start->end)

    private void Start()
    {
        transform.position = points[0].transform.position;
        pointsPosition = new Vector3[points.Length];
        for (int i = 0; i < pointsPosition.Length; i++)
        {
            pointsPosition[i] = points[i].position;
        }
        Initialize();
        StartCoroutine(Patrol());
    }

    private void Update()
    {
        sr.color = Color.Lerp(sr.color, Color.white, Time.deltaTime); //Lerp the color back to it's origin to create damage effects
    }

    IEnumerator Patrol()
    {
        int current = 0; //the current point the enemy started on
        int target = 1; //the current target point the enemy is going to
        bool forward = true; //is the enemy moving forwarf through the patrol points
        while (true)
        {
            float progress = 0f;
            float distance = Vector2.Distance(pointsPosition[current], pointsPosition[target]);

            while (progress < 1f)
            {
                //Lerp between the current position and the target position with a sin wave to make movement smooth
                transform.position = 
                    Vector3.Lerp(pointsPosition[current], pointsPosition[target], (1f + Mathf.Cos(progress * Mathf.PI)) / 2f);
                
                yield return null;
                progress += (speed / distance) * Time.deltaTime;
            }

            transform.position = pointsPosition[target];

            current = target;

            target += forward ? 1 : -1;
            if ((target >= pointsPosition.Length || target < 0) && pingpong)
            {
                forward = !forward;
                target = Mathf.Clamp(target, 0, pointsPosition.Length - 1);
            }
            else if (target >= pointsPosition.Length)
            {
                target -= pointsPosition.Length;
            }
        }
    }

    public override void Kill()
    {
        Destroy(gameObject);
    }
}
