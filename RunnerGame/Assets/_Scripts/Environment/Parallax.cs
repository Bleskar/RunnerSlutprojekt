using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Parallax : MonoBehaviour
{
    Transform Parent => Camera.main.transform; //what the background should follow
    [SerializeField] Vector2 weight; //how much the background should stick to the parent object

    // Update is called once per frame
    void Update()
    {
        //make the background follow the camera
        transform.position = weight * (Vector2)Parent.position;
    }
}
