using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RecCollision : CustomCollider
{
    [SerializeField] private Transform tf;
    [SerializeField] private Vector2 offset;
    [SerializeField] private bool isDynamic;
    private Vector2 size;
    private Vector2 center;
    private Quaternion rotation;

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

    private void Update()
    {
        if (isDynamic)
        {
            UpdateColliderProperties();
        }
    }

    private void UpdateColliderProperties()
    {
        center = (Vector2)tf.position + offset;
        size = new Vector2(tf.localScale.x, tf.localScale.y) + offset;
        rotation = tf.rotation;
    }

    public override void UpdateCollider()
    {

    }

    public Vector2 GetCenter()
    {
        return center;
    }

    public Vector2 GetSize()
    {
        return size;
    }

    public Quaternion GetRotation()
    {
        return rotation;
    }
}
