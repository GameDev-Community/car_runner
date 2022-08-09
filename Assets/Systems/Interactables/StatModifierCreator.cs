using Game.Core;
using Game.Stats;

namespace Game.Interactables
{
    [System.Serializable]
    public struct StatModifierCreator
    {
        public StatObject StatObject;
        public StatModifier Modifier;


        public StatModifierCreator(StatObject statObject, StatModifier modifier)
        {
            StatObject = statObject;
            Modifier = modifier;
        }
    }


}
