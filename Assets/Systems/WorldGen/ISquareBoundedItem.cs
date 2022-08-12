using UnityEngine;

namespace Systems.WorldGen
{
    public interface ISquareBoundedItem
    {
        public SquareBounds SquareBounds { get; }


        /// <param name="worldPosition">x, z</param>
        /// <param name="minCorner">x, z</param>
        /// <param name="maxCorner">x, z</param>
        public void GetWorldSquareBoundsOnPosition(Vector2 worldPosition, out Vector2 minCorner, out Vector2 maxCorner)
        {
            var b = SquareBounds;
            minCorner = b.MinCorner + worldPosition;
            maxCorner = b.MaxCorner + worldPosition;
        }


        public Component GetItemInstance();
        public Component GetItemInstance(Transform parent);
    }
}