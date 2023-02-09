using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Lidar : MonoBehaviour
{
    public float low_angle;
    public float high_angle;
    public int rays;
    public float max_distance;

    private float interval_angle;
    private float start_angle;

    public List<RaycastHit> hits;

    // Start is called before the first frame update
    void Start()
    {
        interval_angle = Mathf.Abs(high_angle - low_angle) / (rays - 1);
        start_angle = -Mathf.Abs(high_angle - low_angle) / 2;
        hits = new List<RaycastHit>();
    }

    // Update is called once per frame
    void Update()
    {
        hits.Clear();
        for(int i = 0; i < rays; i++)
        {
            RaycastHit hit;
            Vector3 direction = Quaternion.Euler(0, start_angle + i * interval_angle, 0) * transform.forward;
            if(Physics.Raycast(transform.position, direction, out hit, max_distance))
            {
                Debug.DrawLine(transform.position, hit.point, Color.yellow);
                hits.Add(hit);
            }
            else
            {
                hits.Add(hit);
            }
            
        }
    }
}
