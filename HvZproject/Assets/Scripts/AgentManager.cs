using UnityEngine;
using Random = UnityEngine.Random;

public class AgentManager : MonoBehaviour
{
    public GameObject target;
    public Vehicle vehicle;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.S))
        {
            vehicle.isSeeking = true;
        }

        if (Input.GetKeyDown(KeyCode.F))
        {
            // Set seeker mode to false (flee mode)
            vehicle.isSeeking = false;
        }
        
        Camera cam = Camera.main;
        Vector3 max = cam.ScreenToWorldPoint(new Vector3(cam.pixelWidth, cam.pixelHeight, cam.nearClipPlane));
        Vector3 min = cam.ScreenToWorldPoint(new Vector3(0, 0, cam.nearClipPlane));

        float distance = Vector3.Distance(target.transform.position, vehicle.transform.position);
        if (distance < 0.5f)
        {
            target.transform.position = new Vector3(Random.Range(min.x, max.x), 1, Random.Range(min.y, max.y));
        }
    }
    
    void OnGUI()
    {
        GUI.color = Color.cyan;
        GUI.skin.box.fontSize = 20;

        if (vehicle.isSeeking)
        {
            GUI.Box(new Rect(10, 10, 100, 40), "Seeking");
        }
        else
        {
            GUI.Box(new Rect(10, 10, 100, 40), "Fleeing");
        }
    }
}
