using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Watermelon.Outline
{
    public class SimpleMeshPool : ObjectPool<Mesh>
    {
        public override Mesh CreateNewObject()
        {
            var mesh = new Mesh();
            mesh.MarkDynamic();

            return mesh;
        }
    }
}