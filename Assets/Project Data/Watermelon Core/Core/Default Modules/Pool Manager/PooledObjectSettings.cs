using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Watermelon
{
    public class PooledObjectSettings
    {
        //activate
        private bool activate;
        private bool useActiveOnHierarchy;
        public bool Activate => activate;
        public bool UseActiveOnHierarchy => useActiveOnHierarchy;

        //position
        private Vector3 position;
        private bool applyPosition;
        public Vector3 Position => position;
        public bool ApplyPosition => applyPosition;

        //localPosition
        private Vector3 localPosition;
        private bool applyLocalPosition;
        public Vector3 LocalPosition => localPosition;
        public bool ApplyLocalPosition => applyLocalPosition;

        //eulerRotation
        private Vector3 eulerRotation;
        private bool applyEulerRotation;
        public Vector3 EulerRotation => eulerRotation;
        public bool ApplyEulerRotation => applyEulerRotation;

        //localEulerRotation
        private Vector3 localEulerRotation;
        private bool applyLocalEulerRotation;
        public Vector3 LocalEulerRotation => localEulerRotation;
        public bool ApplyLocalEulerRotation => applyLocalEulerRotation;

        //rotation
        private Quaternion rotation;
        private bool applyRotation;
        public Quaternion Rotation => rotation;
        public bool ApplyRotation => applyRotation;

        //localRotation
        private Quaternion localRotation;
        private bool applyLocalRotation;
        public Quaternion LocalRotation => localRotation;
        public bool ApplyLocalRotation => applyLocalRotation;

        //localScale
        private Vector3 localScale;
        private bool applyLocalScale;
        public Vector3 LocalScale => localScale;
        public bool ApplyLocalScale => applyLocalScale;

        //parrent
        private Transform parrent;
        private bool applyParrent;
        public Transform Parrent => parrent;
        public bool ApplyParrent => applyParrent;



        public PooledObjectSettings(bool activate = true, bool useActiveOnHierarchy = false)
        {
            this.activate = activate;
            this.useActiveOnHierarchy = useActiveOnHierarchy;

            applyPosition = false;
            applyEulerRotation = false;
            applyLocalEulerRotation = false;
            applyRotation = false;
            applyLocalRotation = false;
            applyLocalScale = false;
            applyParrent = false;
        }

        public PooledObjectSettings SetActivate(bool activate)
        {
            this.activate = activate;
            return this;
        }

        public PooledObjectSettings SetPosition(Vector3 position)
        {
            this.position = position;
            applyPosition = true;
            return this;
        }

        public PooledObjectSettings SetLocalPosition(Vector3 localPosition)
        {
            this.localPosition = localPosition;
            applyLocalPosition = true;
            return this;
        }

        public PooledObjectSettings SetEulerRotation(Vector3 eulerRotation)
        {
            this.eulerRotation = eulerRotation;
            applyEulerRotation = true;
            return this;
        }

        public PooledObjectSettings SetLocalEulerRotation(Vector3 eulerRotation)
        {
            this.localEulerRotation = eulerRotation;
            applyLocalEulerRotation = true;
            return this;
        }

        public PooledObjectSettings SetRotation(Quaternion rotation)
        {
            this.rotation = rotation;
            applyRotation = true;
            return this;
        }

        public PooledObjectSettings SetLocalRotation(Quaternion rotation)
        {
            this.localRotation = rotation;
            applyLocalRotation = true;
            return this;
        }

        public PooledObjectSettings SetLocalScale(Vector3 localScale)
        {
            this.localScale = localScale;
            applyLocalScale = true;
            return this;
        }

        public PooledObjectSettings SetLocalScale(float localScale)
        {
            this.localScale = localScale * Vector3.one;
            applyLocalScale = true;
            return this;
        }

        public PooledObjectSettings SetParrent(Transform parrent)
        {
            this.parrent = parrent;
            applyParrent = true;
            return this;
        }
    }
}

// -----------------
// Pool Manager v 1.6.5
// -----------------