using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HomingProjectile : MonoBehaviour
{
    public Transform target;
    public float rotateSpeed = 200f;//ת���ٶ�
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
        Vector2 targetDir = ((Vector2)target.position - rb.position).normalized;//Ŀ�귽��
        //
        float rotateAmount = Vector3.Cross(currentDir, targetDir).z;//���ǹ��ĵ��ǲ�˵�������������ߣ������ұ�
        rb.angularVelocity = -rotateAmount * rotateSpeed;
        rb.velocity = transform.right * moveSpeed;//transform.right�������嵱ǰ�����򣬲�ͬ��vector3.right,���������Ǳ����ٶ�
    }
}
