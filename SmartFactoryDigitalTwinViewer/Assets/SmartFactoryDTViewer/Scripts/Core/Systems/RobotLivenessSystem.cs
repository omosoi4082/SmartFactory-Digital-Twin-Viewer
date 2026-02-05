using UnityEngine;

public class RobotLivenessSystem : MonoBehaviour
{
    [SerializeField] private float timeoutSeconds = 3f;
    private RobotRegistry _registry;

    public void Initialized(RobotRegistry registry)
    {
        _registry=registry; 
    }
    // Update is called once per frame
    void Update()
    {
        float now=Time.time;
        foreach (var item in _registry.GetAll())
        {
            if(item.isAlive&&now-item.lastSeenTime>timeoutSeconds)
            {
                item.MarkDisconnected();
            }
        }
    }
}
