using UnityEngine;

// Self-Destruct component
public class SelfDestruct : MonoBehaviour
{
    public float lifetime = 3f;

    private float timeAlive;

    void Start()
    {
        timeAlive = 0f;
    }

    // Update is called once per frame
    void Update()
    {
        timeAlive += Time.unscaledDeltaTime;
        if (timeAlive > lifetime)
        {
            Destroy(gameObject);
        }
    }
}