using UnityEngine;

public class Chunk : MonoBehaviour
{
    public Chunk Active(float position)
    {
        transform.localPosition = new Vector3(transform.position.x, position, transform.position.z);
        gameObject.SetActive(true);
        return this;
    }
}