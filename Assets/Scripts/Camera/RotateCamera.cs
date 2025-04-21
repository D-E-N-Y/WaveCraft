using System.Collections;
using UnityEngine;

public class RotateCamera : MonoBehaviour
{
    [SerializeField, Range(0.1f, 10f)] private float rotateSpeed;

    private Coroutine _rotate;

    private void Start() 
    {
        _rotate = StartCoroutine(nameof(Rotate));
    }    
    
    private IEnumerator Rotate()
    {
        while(true)
        {
            transform.Rotate(Vector3.up * rotateSpeed * Time.deltaTime);
            yield return null;
        }
    }
}
