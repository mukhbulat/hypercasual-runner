using System.Collections;
using Game;
using UnityEngine;

public class GreenScreenBehaviour : MonoBehaviour, IRestartable
{
    [SerializeField] private Transform player;
    [SerializeField] private float fadeTime = 1f;

    private MeshRenderer _meshRenderer;
    
    

    private void Awake()
    {
        _meshRenderer = GetComponent<MeshRenderer>();
        StartCoroutine(Fading());
    }

    private IEnumerator Fading()
    {
        
        while (player.position.z < -5)
        {
            yield return null;
        }

        yield return new WaitForSeconds(fadeTime);

        _meshRenderer.enabled = false;
        yield return new WaitForSeconds(fadeTime / 2);

        _meshRenderer.enabled = true;
        yield return new WaitForSeconds(fadeTime / 4);

        _meshRenderer.enabled = false;
        
    }
    
    public void Restart()
    {
        _meshRenderer.enabled = true;
        StartCoroutine(Fading());
    }
}
