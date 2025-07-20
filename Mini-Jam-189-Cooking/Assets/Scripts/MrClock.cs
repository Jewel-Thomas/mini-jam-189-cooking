using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class MrClock : MonoBehaviour
{
    public GameObject[] ingredientPrefabs;
    public Transform throwPoint;
    public Transform chefTarget; // Assign the chef's Transform in the Inspector
    public float throwForce = 7f;

    public void ReactToFailure() {
        // Screen shake, grumble, etc.
    }
}
