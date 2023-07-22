using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;

public class LightController : MonoBehaviour
{
    [SerializeField] Light lightSource;
    [SerializeField] MeshRenderer _emissiveMeshRenderer;

    float _initialIntensity;

    List<Material> _materials = new List<Material>();
    List<Color> _initialEmissiveColors = new List<Color>();

    private void Awake()
    {
        
    }

    private void Start()
    {
        if (FuseBox.Instance != null)
        {
            FuseBox.Instance.OnPowerDown += TurnOff;
            FuseBox.Instance.OnPowerUp += TurnOn;
        }

        _initialIntensity = lightSource.intensity;

        foreach (Material material in _emissiveMeshRenderer.materials)
        {
            _initialEmissiveColors.Add(material.GetColor("_EmissionColor"));
        }
    }

    private void OnDestroy()
    {
        if (FuseBox.Instance != null)
        {
            FuseBox.Instance.OnPowerDown -= TurnOff;
            FuseBox.Instance.OnPowerUp -= TurnOn;
        }
    }

    public void TurnOn()
    {
        lightSource.intensity = _initialIntensity;

        for (int i = 0; i < _emissiveMeshRenderer.materials.Length; i++)
        {
            _emissiveMeshRenderer.materials[i].SetColor("_EmissionColor", _initialEmissiveColors[i]);
        }
    }

    public void TurnOff()
    {
        lightSource.intensity = 0;

        foreach (Material material in _emissiveMeshRenderer.materials)
        {
            material.SetColor("_EmissionColor", Color.black);
        }
    }
}
