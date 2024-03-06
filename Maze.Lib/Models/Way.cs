using System;
using System.Collections.Generic;
using System.Text;

namespace Maze.Lib.Models
{
    /// <summary>
    /// Класс с параметрами нового пути
    /// </summary>
    public class Way
    {
        /// <summary>
        /// Координата X
        /// </summary>
        public int x { get; set; }

        /// <summary>
        /// Координата Y
        /// </summary>
        public int y { get; set; }

        /// <summary>
        /// Дальность текущей клетки от начальной
        /// </summary>
        public int MaxWayLocal { get; set; }

        /// <summary>
        /// Базовый конструктор
        /// </summary>
        /// <param name="x">Координата X</param>
        /// <param name="y">Координата Y</param>
        /// <param name="maxWaylocal">Дальность текущей клетки от начальной</param>
        public Way(int x, int y, int maxWaylocal)
        {
            this.x = x;
            this.y = y;
            MaxWayLocal = maxWaylocal;
        }
    }
}
