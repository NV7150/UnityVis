using System;
using System.Collections.Generic;
using UnityEngine;
using WebSocketSharp;

namespace PointCloud {
    public class WebSockPcdLoader : PointCloudLoader {

        [SerializeField] private string url;

        private WebSocket _ws;
        private bool _connectionStarted = false;
        private bool _hasNew = true;

        private List<(Vector3, Vector3)> _points = new ();

        private void Start() {
            _ws = new WebSocket(url);

            _ws.OnOpen += (sender, e) => {
                // _connectionStarted = true;
                Debug.Log("started");
            };

            _ws.OnMessage += OnReceive;
            _ws.OnError += (s, e) => { Debug.Log("errored"); };

            _ws.Connect();
        }

        void OnReceive(object sender, MessageEventArgs e) {
            var item = JsonUtility.FromJson<MsgItem>(e.Data);
            _points = item.ToVec();
            _hasNew = true;
            _connectionStarted = true;
        }

        public override bool IsReady() {
            return _connectionStarted;
        }

        public override (Vector3, Vector3) GetPoint(int i) {
            return _points[i];
        }

        public override IEnumerable<(Vector3, Vector3)> IteratePoint() {
            foreach (var p in _points) {
                yield return p;
            }
        }

        public override int Count() {
            return _points.Count;
        }
        
        public override bool HasNew() {
            return _hasNew;
        }

        public override void ResetNew() {
            _hasNew = false;
        }
    }

    [Serializable]
    class MsgItem {
        [SerializeField] private List<JVec> points;
        [SerializeField] private List<JColor> colors;

        public List<(Vector3, Vector3)> ToVec() {
            var l = new List<(Vector3, Vector3)>();
            for (var i = 0; i < points.Count; i++) {
                var p = points[i];
                var c = colors[i];
                
                l.Add((p.Coord, c.Color));
            }

            return l;
        }
    }

    [Serializable]
    class JVec {
        [SerializeField] private float x;
        [SerializeField] private float y;
        [SerializeField] private float z;

        public Vector3 Coord => new(x, y, z);
    }

    [Serializable]
    class JColor {
        [SerializeField] private float r;
        [SerializeField] private float g;
        [SerializeField] private float b;
        public Vector3 Color => 255 * new Vector3(r, g, b);
    }
    
}
