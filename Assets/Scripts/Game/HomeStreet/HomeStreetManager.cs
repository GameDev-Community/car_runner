using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Game.HomeStreet
{
    /// <summary>
    /// Класс - главный унитаз для сцены HomeStreet (улица, откуда можно
    /// получить доступ к Гаражу, Магазину, Заезду.
    /// Если фича с снайпингом зомбёв будет реализована, то заебись. Джаст сеинг.
    /// строка для дальнейших уточнений
    /// </summary>
    public class HomeStreetManager
    {
        /// <summary>
        /// флаг, определяющий, должны ли
        /// интерактивные объекты (гараж, магаз, ворота)
        /// игнорировать клики по себе.
        /// </summary>
        private static bool _ignoreClicks;


        public static bool IgnoreClicks => _ignoreClicks;

    }


}
