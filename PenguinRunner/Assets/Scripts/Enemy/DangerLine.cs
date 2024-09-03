using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DangerLine : RecycleObject
{
    public TrailRenderer tr;
    public float trailSpeed = 3.5f;
    public Vector3 endPosition;
    public float disaberTime = 3.0f;
    Vector3 dir;
    private void Awake()
    {
        tr = GetComponent<TrailRenderer>();
    }
    private void Start()
    {
        tr.startColor = new Color(1, 0, 0, 0.7f);
        tr.endColor = new Color(1, 0, 0, 0.7f);
        endPosition = GameManager.Instance.Player.transform.position;
        SetDestination(endPosition);
        DisableTimer(disaberTime);
    }
    protected override void OnReset()
    {
        DisableTimer(disaberTime);
    }

    protected override void OnDisable()
    {
        base.OnDisable();
        tr.Clear();
    }

    private void Update()
    {
        transform.Translate(Time.deltaTime*trailSpeed*dir);
    }

    public void SetDestination(Vector3 endPosition)
    {
        dir= (endPosition-transform.position).normalized;
    }
}
