using System.IO;

namespace Externals.Utils.SaveManager
{
    /// <summary>
    /// Для некоторых классов может быть
    /// предпочтительна внутренняя загрузка
    /// (void Load(...)), такие классы должны
    /// осуществлять сохранение и загрузку
    /// через SaveManager события
    /// </summary>
    public interface ISavable<T>
    {
        public void Save(BinaryWriter bw);
        public T Load(BinaryReader br);
    }
}
