using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum Dimension { Laser, Electrical, Steampunk }

public class ShowIfLevelFinished : MonoBehaviour
{
    public Dimension level;

    public void Start()
    {
        gameObject.SetActive(SaveManager.IsLevelFinished(level));
    }
}
