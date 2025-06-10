using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class MaterialController 
{
    //Inspector Assigned
    [SerializeField] protected Material Material = null;

    [SerializeField] protected Texture _diffuseTexture = null;
    [SerializeField] protected Color _diffuseColor = Color.white;
    [SerializeField] protected Texture _normalMap = null;
    [SerializeField] protected float _normalStrength = 1.0f;

    [SerializeField] protected Texture _emissiveTexture = null;
    [SerializeField] protected Color _emissionColor = Color.black;
    [SerializeField] protected float _emissionScale = 1.0f;

    protected MaterialController _backup = null;
    protected bool _started = false;

    public Material material { get { return Material; } }

    public void OnStart()
    {
        if (Material == null || _started) return;

        _started = true;
        _backup = new MaterialController();

        _backup._diffuseColor = Material.GetColor("_Color");
        _backup._diffuseTexture = Material.GetTexture("_MainTex");
        _backup._emissionColor = Material.GetColor("_EmissionColor");
        _backup._emissionScale = 1;
        _backup._emissiveTexture = Material.GetTexture("_EmissionMap");
        _backup._normalMap = Material.GetTexture("_BumpMap");
        _backup._normalStrength = Material.GetFloat("_BumpScale");

        if (GameSceneManager.instance) GameSceneManager.instance.RegisterMaterialController(Material.GetInstanceID(), this);
    }
    public void Activate(bool activate)
    {
        if (!_started || Material == null) return;

        if (activate)
        {
            Material.SetColor("_Color",_diffuseColor);
            Material.SetTexture("_MainTex",_diffuseTexture);
            Material.SetColor("_EmissionColor",_emissionColor*_emissionScale);
            Material.SetTexture("_EmissionMap",_emissiveTexture);
            Material.SetTexture("_BumpMap",_normalMap);
            Material.SetFloat("_BumpScale",_normalStrength);
        }
        else
        {
            Material.SetColor("_Color", _backup._diffuseColor);
            Material.SetTexture("_MainTex", _backup._diffuseTexture);
            Material.SetColor("_EmissionColor", _backup._emissionColor *_backup._emissionScale);
            Material.SetTexture("_EmissionMap", _backup._emissiveTexture);
            Material.SetTexture("_BumpMap", _backup._normalMap);
            Material.SetFloat("_BumpScale", _backup._normalStrength);
        }
    }
    public void OnReset()
    {
        if (_backup == null || Material == null) return;
        Material.SetColor("_Color", _backup._diffuseColor);
        Material.SetTexture("_MainTex", _backup._diffuseTexture);
        Material.SetColor("_EmissionColor", _backup._emissionColor * _backup._emissionScale);
        Material.SetTexture("_EmissionMap", _backup._emissiveTexture);
        Material.SetTexture("_BumpMap", _backup._normalMap);
        Material.SetFloat("_BumpScale", _backup._normalStrength);
    }
    public int GetInstanceID()
    {
        if (Material == null) return -1;
        return Material.GetInstanceID();
    }
}
