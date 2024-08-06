using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CircleCollision : CustomCollider
{
    [SerializeField] private Transform tf;
    [SerializeField] private float offset;
    [SerializeField] private bool isDynamic;
    private float radius;
    private Vector2 center;

    private void Start()
    {
        if (tf == null)
        {
            Debug.LogError("Transform is not assigned.");
            return;
        }

        UpdateColliderProperties();
        CollisionManager.Instance.AddCollider(this);
    }

    public void DrawShape()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(center + new Vector2(offset, 0), radius);
    }

    private void OnDrawGizmosSelected()
    {
        DrawShape();
    }
    private void Update()
    {
        if (isDynamic)
        {
            UpdateColliderProperties();
        }
    }

    private void UpdateColliderProperties()
    {
        center = tf.position;
        radius = (tf.localScale.x + tf.localScale.y) / 4 + offset;
    }

    public override void UpdateCollider()
    {

    }

    public float GetRadius()
    {
        return radius;
    }

    public Vector2 GetCenter()
    {
        return center;
    }
}
