using UnityEngine;

public class PerfectDashChecker : MonoBehaviour
{
    [SerializeField] private float lifespan;
    private void Start() 
    {
        Destroy(gameObject, lifespan);
    }
}
