using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class property : MonoBehaviour
{
    [SerializeField] private GameManager.propertyList attributeValue;
    public GameManager.propertyList GetProperty()
    {
        return attributeValue;
    }
}
