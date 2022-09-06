using DevourDev.Unity.ScriptableObjects;
using Externals.Utils;
using System.Collections;
using UnityEngine;

namespace Externals.Utils.Economics
{
    [CreateAssetMenu(menuName = "Economics/Shop/Shop Item Object")]
    public class ShopItemObject : GameDatabaseElement
    {
        [SerializeField] private MetaInfo _metaInfo;
        [SerializeField] private UnlockCondition[] _unlockConditions;
    }

    [System.Serializable]
    public class UnlockCondition
    {
        /// <summary>
        /// Прости, Глинк, я не выдаю
        /// кредиты, я выдаю пизды.
        /// </summary>
        public enum UnlockMode
        {
            /// <summary>
            /// если у тебя есть указанные
            /// статы в указанном количестве,
            /// предмет открывается (все статы
            /// остаются).
            /// </summary>
            Reach,
            /// <summary>
            /// лампы,веревки (для
            /// жопы), помпы (для члена),
            /// тебе всё это нужно? Оно твоё,
            /// мой друг! Если у тебя
            /// достаточно Рупий...
            /// </summary>
            Buy
        }
    }

}