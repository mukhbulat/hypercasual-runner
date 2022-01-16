using System;
using System.Collections.Generic;
using UnityEngine;

public class PlatformSpawner : MonoBehaviour
{
    [SerializeField] private List<PlatformData> platforms;

    private Camera _camera;
    private void Awake()
    {
        _camera = Camera.main;
    }

    private void Update()
    {
    }
}