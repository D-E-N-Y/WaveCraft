using UnityEngine;

public class RandomDirectionLight : MonoBehaviour
{
    void Start()
    {
        transform.rotation = Quaternion.Euler(
            Random.Range(0, 360), 
            0, 
            0
        );
    }
}
