using UnityEngine;

public class SelectSpot : MonoBehaviour
{
    new Transform transform;
    new Camera camera;

    void Awake()
    {
        transform = GetComponent<Transform>();
        // Let Tale do the heavy lifting
        camera = TaleUtil.Props.camera.obj;
    }

    private void Update()
    {
        transform.LookAt(camera.transform);
        transform.rotation = Quaternion.Euler(0f, transform.rotation.eulerAngles.y, 0f);
    }
}
