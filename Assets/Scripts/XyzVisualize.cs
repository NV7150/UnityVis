using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using PointCloud;
using UnityEngine;

public class XyzVisualize : MonoBehaviour {
    [SerializeField] private PointCloudLoader pointCloud;
    [SerializeField] private Shader shader;

    [SerializeField] private float rad;
    [SerializeField] private int ballSize;

    private bool _end = false;

    public bool End {
        get => _end;
        set => _end = value;
    }

    private Material _material;
    private bool _isReady = false;

    IEnumerator DebugShow() {
        yield return new WaitUntil(pointCloud.IsReady);

        while (!_end) {
            pointCloud.ResetNew();
            _isReady = false;
            
            var coords = new List<Vector3>();
            var colors = new List<Vector3>();

            foreach (var p in pointCloud.IteratePoint()) {
                coords.Add(p.Item1);
                colors.Add(p.Item2);
            }
        
            var size = Marshal.SizeOf(new Vector3());
            var posbuffer = new ComputeBuffer(coords.Count, size);
            var colbuffer = new ComputeBuffer(colors.Count, size);
            posbuffer.SetData(coords);
            colbuffer.SetData(colors);
        
            var material = new Material(shader);
            material.SetBuffer("colBuffer", colbuffer);
            material.SetBuffer("posBuffer", posbuffer);
            material.SetFloat("_Radius", rad);
            material.SetFloat("_Size", ballSize);
            _material = material;

            _isReady = true;
            
            yield return new WaitUntil(pointCloud.HasNew);
        }

    }
    
    // Start is called before the first frame update
    void Start() {
        StartCoroutine(DebugShow());
    }

    private void OnRenderObject() {
        if(!_isReady)
            return;
        
        _material.SetPass(0);
        Graphics.DrawProceduralNow(MeshTopology.Points, pointCloud.Count());
    }

    // Update is called once per frame
    void Update() {
    }
}
