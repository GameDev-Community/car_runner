using UnityEngine;

namespace Utils.Items.Customizables
{
    /// <summary>
    /// объект, хранящий изменяемые штуковины
    /// и определяющий логику их изменений.
    /// </summary>
    public interface ICustomizablesOwner
    {
        public void ChangeItem(ItemType type, CustomizableItem newItem);
    }
}
