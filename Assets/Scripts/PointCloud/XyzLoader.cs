using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace PointCloud {
    class XyzLoader : PointCloudLoader {
        [SerializeField] private TextAsset xyzFile;

        private readonly List<(Vector3, Vector3)> _pointCloud = new();
        private int _current = 0;
        private bool _isReady = false;

        private void OnEnable() {
            _pointCloud.Clear();
            var rows = xyzFile.text.Split("\n");
            foreach (var row in rows) {
                var cols = row.Split(",");
                if(cols.Length != 6)
                    continue;

                var coords = cols.Take(3).Select(float.Parse).ToArray();
                var coord = new Vector3(coords[0], coords[1], coords[2]);

                var colors = cols.Skip(3).Select(int.Parse).ToArray();
                var color = new Vector3(colors[0], colors[1], colors[2]);
                _pointCloud.Add((coord, color));
            }

            _isReady = true;
        }

        public override bool IsReady() {
            return _isReady;
        }

        public override (Vector3, Vector3) GetPoint(int i) {
            return _pointCloud[i];
        }

        public override IEnumerable<(Vector3, Vector3)> IteratePoint() {
            foreach (var p in _pointCloud) {
                yield return p;
            }
        }

        public override int Count() {
            return _pointCloud.Count;
        }
        

    }
}