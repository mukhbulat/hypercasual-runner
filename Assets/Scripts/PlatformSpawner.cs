using System;
using System.Collections.Generic;
using UnityEngine;

public class PlatformSpawner : MonoBehaviour
{
    [SerializeField] private List<PlatformBehaviour> platforms;

    private List<PlatformBehaviour> _platformPool;
    private Camera _camera;
    private int maxPlatformsCount = 30;
    
    
    private void Awake()
    {
        _camera = Camera.main;
    }

    private void Update()
    {
        
    }
}