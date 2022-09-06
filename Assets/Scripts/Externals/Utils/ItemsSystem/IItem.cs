using Externals.Utils.SaveManager;
using System.IO;

namespace Utils.Items
{
    public interface IItem
    {
        public ItemType ItemType { get; }
    }


    public class ItemData : ISavable
    {
        private IItem _item;
        private System.DateTime _purchaseDate;


        public void Load(BinaryReader br)
        {
            throw new System.NotImplementedException();
        }

        public void Save(BinaryWriter bw)
        {
            throw new System.NotImplementedException();
        }
    }

    public struct ItemAmount<TItem>
        where TItem : IItem
    {
        public TItem Item;
        public int Amount;
    }

    public interface IInventory<TItem> : ISavable
        where TItem : IItem
    {
        /// <returns>true, если указанный предмет
        /// присутствует в инвентаре</returns>
        public bool ContainsItem(TItem item);

        /// <returns>true, если указанный предмет
        /// присутствует в инвентаре в количестве
        /// не меньшем ItemAmount.Amount</returns>
        public bool ContainsItem(ItemAmount<TItem> itemAmount);


        /// <returns>true, если каждый указанный предмет
        /// присутствует в инвентаре</returns>
        public bool ContainsItems(TItem[] item);

        /// <returns>true, если каждый указанный предмет
        /// присутствует в инвентаре в количестве
        /// не меньшем ItemAmount.Amount</returns>
        public bool ContainsItems(ItemAmount<TItem>[] itemAmounts);


        /// <summary>
        /// удаляет указанный предмет из инвентаря,
        /// если он там есть
        /// </summary>
        /// <returns>true, если указанный предмет
        /// был найден и удалён</returns>
        public bool TryGetItem(TItem item);

        /// <summary>
        /// удаляет все указанные предметы из инвентаря,
        /// если они там есть (если любой
        /// из предметов отсутствует - возвращается
        /// false и ничего не происходит
        /// <returns>true, если каждый указанный предмет
        /// был найден и удалён</returns>
        /// </summary>
        public bool TryGetItems(TItem[] items);

        /// <summary>
        /// удаляет количество указанного предмета
        /// из инвентаря, если он присутствует
        /// в инвентаре в не меньшем количестве,
        /// чем ItemAmount.Amount
        /// </summary>
        /// <returns>true, если кол-во предмета
        /// было найдено и удалено</returns>
        public bool TryGetItem(ItemAmount<TItem> itemAmount);

        /// <summary>
        /// удаляет количество каждого указанного предмета
        /// из инвентаря, если он присутствует
        /// в инвентаре в не меньшем количестве,
        /// чем ItemAmount.Amount
        /// </summary>
        /// <returns>true, если кол-во каждого указанного
        /// было найдено и удалено</returns>
        public bool TryGetItems(ItemAmount<TItem>[] itemAmounts);
    }
}
