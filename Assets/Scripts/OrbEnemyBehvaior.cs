using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OrbEnemyBehvaior : MonoBehaviour
{
    public GameObject FollowTarget;
    public float MovementSpeed;
    public float ScaleFactor;

    void FixedUpdate() {
        var targetPosition = FollowTarget.transform.position;
        var ourPosition = transform.position;

        var direction = (targetPosition - ourPosition).normalized;
        var velocity = direction * MovementSpeed * Time.fixedDeltaTime;

        transform.position += velocity;

        transform.localScale += Vector3.one * ScaleFactor * Time.fixedDeltaTime;
    }
}
