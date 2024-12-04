using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyCrystal : MonoBehaviour
{
    public void SetActiveCrystal(bool isActive)
    {
        gameObject.SetActive(isActive);
    }
}
