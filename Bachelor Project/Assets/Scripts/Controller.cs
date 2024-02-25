using UnityEngine;

public class Controller : MonoBehaviour
{
    Camera cam;
    public LayerMask mask;
    public Car car;
    void Awake()
    {
        cam = Camera.main;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButton(0))
        {
            Ray ray = cam.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out RaycastHit hit, 100, mask))
            {
                car.MoveTo(hit.point);
            }
        }
    }
}
