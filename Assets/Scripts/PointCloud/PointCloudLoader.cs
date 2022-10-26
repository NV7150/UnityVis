using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PointCloud {
    public abstract class PointCloudLoader : MonoBehaviour {
        public abstract bool IsReady();
        public abstract (Vector3, Vector3) GetPoint(int i);
        public abstract IEnumerable<(Vector3, Vector3)> IteratePoint();

        public abstract int Count();

        public virtual bool HasNew() {
            return false;
        }
        
        public virtual void ResetNew() { }
    }
}