using UnityEngine;

public class PerfectDashChecker : MonoBehaviour
{
    private float lifespan = .15f;
    private void Start() 
    {
        Destroy(gameObject, lifespan);
    }
}
