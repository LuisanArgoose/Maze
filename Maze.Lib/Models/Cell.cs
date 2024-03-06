using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Text;

namespace Maze.Lib.Models
{
    /// <summary>
    /// Класс для хранения клетки с метаданными
    /// </summary>
    public class Cell : IEquatable<Cell>
    {
        /// <summary>
        /// Координата X
        /// </summary>
        public int Xpos { get; private set; }

        /// <summary>
        /// Координата Y
        /// </summary>
        public int Ypos { get; private set; }

        /// <summary>
        /// Тип клетки - в зависимости от окружения
        /// например "3SR" - Три стороны вправо
        /// </summary>
        public string Type { get; private set; }

        /// <summary>
        /// Дополнения клетки (2, 3 и тд);
        /// </summary>
        public int Addons { get; set; }

        /// <summary>
        /// с - стена(1) или пол(0)
        /// </summary>
        public int Group { get; private set; }

        /// <summary>
        /// Строка из 8 цифр с информацией об окружающих клетках
        /// </summary>
        public string MetaInfo { get; private set; }

        /// <summary>
        /// Стандартный конструктор
        /// </summary>
        /// <param name="xpos">Координата X</param>
        /// <param name="ypos">Координата Y</param>
        /// <param name="type">Тип клетки</param>
        /// <param name="addons">Дополнения клетки</param>
        /// <param name="group">Группа клетки</param>
        /// <param name="metaInfo">Метаданные об окружении</param>
        public Cell(int xpos, int ypos, string type, int addons, int group, string metaInfo)
        {
            Xpos = xpos;
            Ypos = ypos;
            Type = type;
            Addons = addons;
            Group = group;
            MetaInfo = metaInfo;
        }
        
        public bool Equals([AllowNull] Cell other)
        {
            if (
                Xpos == other.Xpos &&
                Ypos == other.Ypos &&
                Type == other.Type &&
                Addons == other.Addons &&
                Group == other.Group &&
                MetaInfo == other.MetaInfo
            ) { return true; }
            else
            {
                return false;
            }

           
        }
        
    }
}
