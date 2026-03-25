using UnityEngine;

public class CameraController : MonoBehaviour
{
    GameObject followTarget;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        followTarget = GameObject.Find("PlayerCharacter");
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 targetWithSameZ = followTarget.transform.position;
        targetWithSameZ.z = transform.position.z;

        transform.position = Vector3.Lerp(transform.position, targetWithSameZ, Time.deltaTime);
    }
}