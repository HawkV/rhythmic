using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NoteController : MonoBehaviour
{
    public Vector3 start, finish;
    public float speed;

    private float t = 0f;

    public void Init(Vector3 start, Vector3 finish, float speed)
    {
        this.start = start;
        this.finish = finish;
        this.speed = speed;

        transform.LookAt(finish);
    }

    // Update is called once per frame
    void Update()
    {
        t += speed * Time.deltaTime;
        transform.position = Vector3.Lerp(start, finish, t);
    }
}
