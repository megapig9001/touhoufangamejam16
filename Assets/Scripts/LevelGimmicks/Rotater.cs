using UnityEngine;

public class Rotater : MonoBehaviour
{
    public bool canRotate;
    public float initialX;
    public float initialY;
    public float initialZ;
    public PlayerHealth playerHealth;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        initialX = transform.position.x;
        initialY = transform.position.y;
        initialZ = transform.position.z;
    }

    // Update is called once per frame
    void Update()
    {
        if (canRotate)
            transform.Rotate(0, 0, 2 * Time.deltaTime); //rotates 50 degrees per second around z axis
        if (playerHealth.GetCurrentHealth() <= 0)
        {
            canRotate = false;
            transform.rotation = Quaternion.Euler(0, 0, 0);
        }
    }
}
