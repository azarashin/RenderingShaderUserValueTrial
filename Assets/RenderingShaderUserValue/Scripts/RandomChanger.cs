using System.Linq;
using TMPro;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using static RandomChanger;

public class RandomChanger : MonoBehaviour
{
    public enum ChangeType
    {
        Unknown, 
        MM, 
        PB, 
        RSUV
    }

    [SerializeField]
    Duplicator _mmDuplicator;

    [SerializeField]
    Duplicator _rsuvDuplicator;

    [SerializeField]
    TextMeshProUGUI _fps;

    private MeshRenderer[] _allMeshRenderers;
    private ChangeType _changeType = ChangeType.Unknown; 

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
    }

    public void OnMM()
    {
        _mmDuplicator.Clear(); 
        _rsuvDuplicator.Clear();
        var objs = _mmDuplicator.Generate();
        _allMeshRenderers = objs.Select(s => s.GetComponent<MeshRenderer>()).ToArray();
        _changeType = ChangeType.MM;
    }

    public void OnPB()
    {
        _mmDuplicator.Clear();
        _rsuvDuplicator.Clear();
        var objs = _mmDuplicator.Generate();
        _allMeshRenderers = objs.Select(s => s.GetComponent<MeshRenderer>()).ToArray();
        _changeType = ChangeType.PB;
    }

    public void OnRSUV()
    {
        _mmDuplicator.Clear();
        _rsuvDuplicator.Clear();
        var objs = _rsuvDuplicator.Generate();
        _allMeshRenderers = objs.Select(s => s.GetComponent<MeshRenderer>()).ToArray();
        _changeType = ChangeType.RSUV;
    }

    // Update is called once per frame
    void Update()
    {
        if(_allMeshRenderers == null)
        {
            return; 
        }
        switch(_changeType)
        {
            case ChangeType.Unknown:
                break;
            case ChangeType.MM:
                ChangeAsMM(); 
                break;
            case ChangeType.RSUV:
                ChangeAsRSUV();
                break; 
        }
#if UNITY_EDITOR
        _fps.text = $@"FPS: {1.0f / Time.deltaTime}
Draw Calls: { UnityStats.drawCalls}
Batches: {UnityStats.batches}
SetPass Calls: {UnityStats.setPassCalls}";
#else
        _fps.text = $@"FPS: {1.0f / Time.deltaTime}"; 
#endif

    }

    private void ChangeAsMM()
    {
        MaterialPropertyBlock block = new MaterialPropertyBlock();
        foreach (MeshRenderer meshRenderer in _allMeshRenderers)
        {
            // compute a random color
            Color c = Color.HSVToRGB(UnityEngine.Random.Range(0f, 1f), UnityEngine.Random.Range(0.6f, 1.0f), UnityEngine.Random.Range(0.6f, 1.0f));
            meshRenderer.material.SetColor("_Color", c);
        }
    }

    private void ChangeAsPB()
    {
        MaterialPropertyBlock block = new MaterialPropertyBlock();
        foreach (MeshRenderer meshRenderer in _allMeshRenderers)
        {
            // compute a random color
            Color c = Color.HSVToRGB(UnityEngine.Random.Range(0f, 1f), UnityEngine.Random.Range(0.6f, 1.0f), UnityEngine.Random.Range(0.6f, 1.0f));
            // åªç›ÇÃ PropertyBlock ÇéÊìæ
            meshRenderer.GetPropertyBlock(block);

            // êFÇê›íË
            block.SetColor("_Color", c);

            // çƒê›íË
            meshRenderer.SetPropertyBlock(block);
        }
    }

    private void ChangeAsRSUV()
    {
        foreach (MeshRenderer meshRenderer in _allMeshRenderers)
        {
            // compute a random color
            Color32 c32 = Color.HSVToRGB(UnityEngine.Random.Range(0f, 1f), UnityEngine.Random.Range(0.6f, 1.0f), UnityEngine.Random.Range(0.6f, 1.0f));

            // set it as a LDR color in the RSUV value of the renderer
            uint cc = ((uint)c32.b << 16) | ((uint)c32.g << 8) | ((uint)c32.r << 0);
            meshRenderer.SetShaderUserValue(cc);
        }
    }
}
