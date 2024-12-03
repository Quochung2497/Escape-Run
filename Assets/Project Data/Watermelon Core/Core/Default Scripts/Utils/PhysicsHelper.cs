using UnityEngine;

namespace Watermelon
{
    public static class PhysicsHelper
    {
        public static readonly int LAYER_LAND = LayerMask.NameToLayer("Land");
        public static readonly int LAYER_DEFAULT = LayerMask.NameToLayer("Default");

        public const string TAG_PLAYER = "Player";
        public const string TAG_NURSE = "Nurse";
        public const string TAG_VISITOR = "Visitor";

        public static void Init()
        {

        }
    }
}