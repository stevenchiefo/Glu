using UnityEngine;

public class BasicMovement : MonoBehaviour
{

    [Header("BasicMovement")]
	public  float       speed;



    private void Update()
    {
        transform.Translate(Vector2.up * speed * Time.deltaTime);
    }
}
