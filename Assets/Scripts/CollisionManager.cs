using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CollisionManager : MonoBehaviour
{
    public static CollisionManager Instance { get; private set; }
    private int playerColliderIndex = -1;
    [SerializeField] private List<CustomCollider> colliders = new List<CustomCollider>();

    [SerializeField] private AudioSource audioSource;
    [SerializeField] private AudioClip collisionSound;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else if (Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        colliders.Clear();
    }

    private void Start()
    {
        foreach (var collider in colliders)
        {
            Debug.Log("Collider added: " + collider);
        }
    }

    private void Update()
    {
        for (int i = 0; i < colliders.Count; i++)
        {
            if (colliders[i].gameObject.CompareTag("Player"))
            {
                playerColliderIndex = i;
            }

            if (playerColliderIndex > -1 && colliders[i].enabled)
            {
                CheckCollisions(i);
            }
        }
    }

    private void CheckCollisions(int i)
    {
        if (i != playerColliderIndex && colliders[i].enabled)
        {
            Debug.Log("Checking collision between player and: " + colliders[i].gameObject.name);

            if (colliders[i].gameObject.CompareTag("Pickup"))
            {
                if (CheckCollisionCircles((CircleCollision)colliders[playerColliderIndex], (CircleCollision)colliders[i]))
                {
                    var pickupProperty = colliders[i].gameObject.GetComponent<property>();
                    GameManager.Instance.ModifyPickupAmount(pickupProperty);
                    DisableCollider(colliders[i]);

                    if (pickupProperty.GetProperty() == GameManager.propertyList.Golden)
                    {
                        GameManager.Instance.AddScore(100);
                    }
                    else if (pickupProperty.GetProperty() == GameManager.propertyList.Chest)
                    {
                        GameManager.Instance.AddScore(50);
                    }

                    PlayCollisionSound();
                }
            }
            else if (colliders[i].gameObject.CompareTag("Wall")
                  || colliders[i].gameObject.CompareTag("door"))
            {
                Goals(i);
            }
        }
    }

    private void Goals(int i)
    {
        if (CheckCollisionCircleRect((CircleCollision)colliders[playerColliderIndex], (RecCollision)colliders[i]))
        {
            var doorProperty = colliders[i].gameObject.GetComponent<property>();
            if (colliders[i].gameObject.CompareTag("door") && GameManager.Instance.ifOpenDoors(doorProperty))
            {
                DisableCollider(colliders[i]);
                PlayCollisionSound();
            }
            else
            {
                Vector2 correction = ColDistRectCir((CircleCollision)colliders[playerColliderIndex], (RecCollision)colliders[i]);
                if (correction != Vector2.zero)
                {
                    Vector2 pos = colliders[playerColliderIndex].transform.position;
                    colliders[playerColliderIndex].transform.position = pos + correction;
                    Debug.Log("Applying correction: " + correction);

                    PlayCollisionSound();
                }
            }
        }
    }

    private void PlayCollisionSound()
    {
        if (audioSource != null && collisionSound != null)
        {
            audioSource.PlayOneShot(collisionSound);
        }
    }

    public int AddCollider(CustomCollider collider)
    {
        colliders.Add(collider);
        Debug.Log("Collider added: " + collider);
        return colliders.Count - 1;
    }

    public void DisableCollider(CustomCollider collider)
    {
        collider.gameObject.SetActive(false);
        collider.enabled = false;
    }

    public bool CheckCollisionCircles(CircleCollision collider1, CircleCollision collider2)
    {
        Vector2 distanceCenter = collider2.GetCenter() - collider1.GetCenter();
        float distanceSquared = distanceCenter.sqrMagnitude;
        float radiusSumSquared = Mathf.Pow(collider1.GetRadius() + collider2.GetRadius(), 2);
        Debug.Log("Circle collision check: " + (distanceSquared <= radiusSumSquared));
        return distanceSquared <= radiusSumSquared;
    }

    public bool CheckCollisionCircleRect(CircleCollision circle, RecCollision rect)
    {
        Vector2 circleCenter = circle.GetCenter();
        Vector2 rectCenter = rect.GetCenter();
        Vector2 rectSize = rect.GetSize();
        Quaternion rectRotation = rect.GetRotation();

        Vector2 localCircleCenter = Quaternion.Inverse(rectRotation) * (circleCenter - rectCenter);

        Vector2 centerDifference = new Vector2(Mathf.Abs(localCircleCenter.x), Mathf.Abs(localCircleCenter.y));

        if (centerDifference.x > rectSize.x / 2 + circle.GetRadius() || centerDifference.y > rectSize.y / 2 + circle.GetRadius())
        {
            Debug.Log("No collision: Circle outside Rect");
            return false;
        }
        else if (centerDifference.x <= rectSize.x / 2 || centerDifference.y <= rectSize.y / 2)
        {
            Debug.Log("Collision: Circle intersects Rect");
            return true;
        }
        else
        {
            float cornerDistanceSq = Mathf.Pow(centerDifference.x - rectSize.x / 2, 2) + Mathf.Pow(centerDifference.y - rectSize.y / 2, 2);
            Debug.Log("Corner collision check: " + (cornerDistanceSq <= Mathf.Pow(circle.GetRadius(), 2)));
            return cornerDistanceSq <= Mathf.Pow(circle.GetRadius(), 2);
        }
    }

    public Vector2 ColDistRectCir(CircleCollision circle, RecCollision rect)
    {
        Vector2 colDist = Vector2.zero;

        Vector2 circleCenter = circle.GetCenter();
        Vector2 rectCenter = rect.GetCenter();
        Vector2 rectSize = rect.GetSize();
        Quaternion rectRotation = rect.GetRotation();

        Vector2 localCircleCenter = Quaternion.Inverse(rectRotation) * (circleCenter - rectCenter);

        float xOverlap = rectSize.x / 2 + circle.GetRadius() - Mathf.Abs(localCircleCenter.x);
        float yOverlap = rectSize.y / 2 + circle.GetRadius() - Mathf.Abs(localCircleCenter.y);

        if (xOverlap > 0 && yOverlap > 0)
        {
            if (xOverlap < yOverlap)
            {
                colDist.x = (localCircleCenter.x < 0) ? -xOverlap : xOverlap;
            }
            else
            {
                colDist.y = (localCircleCenter.y < 0) ? -yOverlap : yOverlap;
            }

            colDist = rectRotation * colDist;
        }

        Debug.Log("Collision distance: " + colDist);
        return colDist;
    }
}
