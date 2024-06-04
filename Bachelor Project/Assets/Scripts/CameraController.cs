using Cinemachine;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    [SerializeField] float panSpeed = 30f;
    [SerializeField] float panBorderThickness = 20f;

    [SerializeField] float scrollSpeed = 30f;
    [SerializeField] float minY = 20;
    [SerializeField] float maxY = 60;
    private CinemachineVirtualCamera cam;

    private void Awake()
    {
        cam = GetComponent<CinemachineVirtualCamera>();
    }

    void Update()
    {
        Vector3 pos = transform.position;

        if (Input.GetKey(KeyCode.W) || Input.mousePosition.y >= Screen.height - panBorderThickness)
        {
            pos.z += panSpeed * 10 * Time.deltaTime;
        }
        if (Input.GetKey(KeyCode.S) || Input.mousePosition.y <= panBorderThickness)
        {
            pos.z -= panSpeed * 10 * Time.deltaTime;
        }

        if (Input.GetKey(KeyCode.D) || Input.mousePosition.x >= Screen.width - panBorderThickness)
        {
            pos.x += panSpeed * 10 * Time.deltaTime;
        }
        if (Input.GetKey(KeyCode.A) || Input.mousePosition.x <= panBorderThickness)
        {
            pos.x -= panSpeed * 10 * Time.deltaTime;
        }

        float scroll = Input.GetAxis("Mouse ScrollWheel");
        cam.m_Lens.OrthographicSize -= scroll * scrollSpeed * 100f * Time.deltaTime;
        cam.m_Lens.OrthographicSize = Mathf.Clamp(cam.m_Lens.OrthographicSize, minY, maxY);

        transform.position = pos;
    }

}
