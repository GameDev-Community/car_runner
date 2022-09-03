using UnityEngine;

namespace Externals.Utils.Outlining
{
    public static class OutliningHelpers
    {
        public const Outline.Mode DefaultMode = Outline.Mode.OutlineAll;
        public static readonly Color DefaultColor = Color.white;
        public const float DefaultThickness = 8f;


        public static Outline.OutlineAnimatingState GetOutlineAnimatingState(GameObject target)
        {
            if (!target.TryGetComponent<Outline>(out var outline))
                return Outline.OutlineAnimatingState.Inactive;

            return outline.GetAnimatingState();
        }

        public static Outline.OutlineAnimatingState GetOutlineAnimatingState(Outline outline)
        {
            if (outline == null)
                return Outline.OutlineAnimatingState.Inactive;

            return outline.GetAnimatingState();
        }


        public static void ApplyOutline(GameObject target, Outline.Mode? mode, Color? color, float? thickness, float time = 0f)
        {
            if (!target.TryGetComponent<Outline>(out var outline))
                outline = target.AddComponent<Outline>();

            ApplyOutline(outline, mode, color, thickness, time);
        }

        public static void ApplyOutline(Outline outline, Outline.Mode? mode, Color? color, float? thickness, float time = 0f)
        {
            if (!outline.enabled)
                outline.enabled = true;

            if (mode.HasValue)
                outline.OutlineMode = mode.Value;

            if (color.HasValue)
                outline.OutlineColor = color.Value;

            if (time > 0)
            {
                outline.OutlineWidth = 0;
                outline.AnimateWidth(time, thickness ?? DefaultThickness);
            }
            else
            {
                outline.OutlineWidth = thickness ?? DefaultThickness;
            }
        }


        public static void RemoveOutline(GameObject target, float time = 0f)
        {
            if (!target.TryGetComponent<Outline>(out var outline))
                return;

            RemoveOutline(outline);
        }

        public static void RemoveOutline(Outline outline, float time = 0f)
        {
            if (outline == null)
                return;

            if (!outline.enabled)
                return;

            if (time > 0)
            {
                outline.AnimateWidth(time, 0);
            }
            else
            {
                outline.enabled = false;
            }
        }

    }
}