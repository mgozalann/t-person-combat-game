using System;
using System.Collections.Generic;
using Cinemachine;
using UnityEngine;

public class Targeter : MonoBehaviour
{
    private Camera _mainCamera;
    
    [SerializeField] private CinemachineTargetGroup _cineTargetGroup;
    [SerializeField] private List<Target> _targets = new List<Target>();
    public Target CurrentTarget { get; private set; }

    private void Start()
    {
        _mainCamera = Camera.main;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!other.TryGetComponent(out Target target)) return;
        
        _targets.Add(target);
        target.OnDestroyed += RemoveTarget;
    }

    private void RemoveTarget(Target target)
    {
        if (CurrentTarget == target)
        {
            _cineTargetGroup.RemoveMember(CurrentTarget.transform);
            CurrentTarget = null;
        }

        target.OnDestroyed -= RemoveTarget;
        _targets.Remove(target);
    }

    private void OnTriggerExit(Collider other)
    {
        if(!other.TryGetComponent(out Target target)) return;

        RemoveTarget(target);
    }

    public bool SelectTarget()
    {
        if (_targets.Count == 0) return false;

        Target closestTarget = null;
        float closestTargetDistance = Mathf.Infinity;
        
        foreach (Target target in _targets)
        {
            Vector2 viewPos = _mainCamera.WorldToViewportPoint(target.transform.position);

            if (viewPos.x is < 0 or > 1 || viewPos.y is < 0 or > 1)
            {
                continue;
            }

            Vector2 toCenter = viewPos - new Vector2(0.5f, 0.5f);
            
            if(toCenter.sqrMagnitude < closestTargetDistance)
            {
                closestTarget = target;
                closestTargetDistance = toCenter.sqrMagnitude;
            }
        }

        if (closestTarget == null) {return false;}
        CurrentTarget = closestTarget;
        _cineTargetGroup.AddMember(CurrentTarget.transform,1f,2f);
        
        return true;
    }

    public void CancelTarget()
    {
        if(CurrentTarget == null) return;

        _cineTargetGroup.RemoveMember(CurrentTarget.transform);
        CurrentTarget = null;
    }
}
