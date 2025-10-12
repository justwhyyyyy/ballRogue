using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HomingProjectile : MonoBehaviour
{
    public Transform target;
    public float rotateSpeed = 200f;//转向速度
    public float moveSpeed;

    private Rigidbody2D rb;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }
    private void FixedUpdate()
    {
        if (target == null) return;

        moveSpeed = rb.velocity.magnitude;

        Vector2 currentDir = rb.velocity.normalized;
        Vector2 targetDir = ((Vector2)target.position - rb.position).normalized;//目标方向
        //
        float rotateAmount = Vector3.Cross(currentDir, targetDir).z;//我们关心的是叉乘的正负，正在左边，负在右边
        rb.angularVelocity = -rotateAmount * rotateSpeed;
        rb.velocity = transform.right * moveSpeed;//transform.right就是物体当前的正向，不同于vector3.right,这句的作用是保持速度
    }
}
