using UnityEngine;
using Random = UnityEngine.Random;

public class AgentManager : MonoBehaviour
{
    public GameObject target;
    public Human human;
    private void Update()
    {
        
        Camera cam = Camera.main;
        Vector3 max = cam.ScreenToWorldPoint(new Vector3(cam.pixelWidth, cam.pixelHeight, cam.nearClipPlane));
        Vector3 min = cam.ScreenToWorldPoint(new Vector3(0, 0, cam.nearClipPlane));

        float distance = Vector3.Distance(target.transform.position, human.transform.position);
        if (distance < 0.5f)
        {
            target.transform.position = new Vector3(Random.Range(-25f, 25f), 1, Random.Range(-25f, 25f));
        }
    }
    
    void OnGUI()
    {
        GUI.color = Color.cyan;
        GUI.skin.box.fontSize = 20;  
    }
}
