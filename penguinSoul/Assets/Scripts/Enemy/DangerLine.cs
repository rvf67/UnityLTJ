using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DangerLine : RecycleObject
{
    TrailRenderer tr;
    public float trailSpeed = 3.5f;
    public Vector3 endPosition;
    public float disaberTime = 3.0f;
    Vector3 dir;
    private void Start()
    {
        tr = GetComponent<TrailRenderer>();
        tr.startColor = new Color(1, 0, 0, 0.7f);
        tr.endColor = new Color(1, 0, 0, 0.7f);
        endPosition = GameManager.Instance.Player.transform.position;
        dir= endPosition-transform.position;
        DisableTimer(disaberTime);
    }

    protected override void OnReset()
    {
        transform.Translate(Vector3.zero);
        
        endPosition = GameManager.Instance.Player.transform.position;
        dir = endPosition - transform.position;
        DisableTimer(disaberTime);
    }

    private void Update()
    {
        transform.Translate(Time.deltaTime*trailSpeed*dir);
    }
}
