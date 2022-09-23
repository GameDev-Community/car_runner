using UnityEngine;

namespace Externals.Utils.Items.Customizables
{
    /// <summary>
    /// объект, хранящий изменяемые штуковины
    /// и определяющий логику их изменений.
    /// </summary>
    public interface ICustomizablesOwner<TItem> where TItem : IItem
    {
        public void ChangeItem(TItem newItem);
    }
}
