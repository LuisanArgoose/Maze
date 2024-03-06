
using System;
using System.Collections.Generic;
using System.Linq;
using Maze.Lib.Helpers;
using Maze.Lib.Models;

namespace Maze.Lib
{
    /// <summary>
    /// Генератор лабиринтов размерами X*Y
    /// Где 1 - это стена
    /// 2 - стрелка
    /// 3 - конечная точка
    /// 0 - пустое пространство
    /// </summary>
    public class Maze
    {
        /// <summary>
        /// Массив матрицы лабиринта
        /// </summary>
        private int[,] _map;

        /// <summary>
        /// Список клеток с метаданными
        /// </summary>
        private List<Cell> _mapCell;


        /// <summary>
        /// Координаты для конечной точки лабиринта
        /// </summary>
        private int _endPointX, _endPointY = 1;

        /// <summary>
        /// Координаты для начальной точки лабиринта
        /// </summary>
        private int _arrowX, _arrowY;

        /// <summary>
        /// Класс случайных чисел
        /// </summary>
        private SecretRandom _rnd = new SecretRandom();

        /// <summary>
        /// Ключ генерации карты
        /// </summary>
        private string _seed { get; set; }

        /// <summary>
        /// Массив с размерами матрицы лабиринта
        /// </summary>
        private int[] _mapSizeXY = { 6, 6 };

        /// <summary>
        /// Длинна максимального пути
        /// </summary>
        int _maxWay = 0;

        public int GetMaxWay()
        {
            return _maxWay;
        }

        /// <summary>
        /// Базовый конструктор с генерацией самой первой карты
        /// </summary>
        public Maze()
        {
            GetMap();
        }

        /// <summary>
        /// Вывод матрицы лабиринта на консоль для отладки
        /// </summary>
        public void Print(int[,] map = null)
        {
            if (map == null)
            {
                if (_map == null)
                    return;
                map = _map;
            }
            Console.WriteLine("сид-слово:" + _seed);
            Console.WriteLine("Размеры:" + _mapSizeXY[0] + "x" + _mapSizeXY[1]);
            for (int i = 0; i < map.GetLength(0); i++)
            {
                for (int j = 0; j < map.GetLength(1); j++)
                {
                    Console.Write(map[i, j]);
                }
                Console.WriteLine();
            }

        }

        /// <summary>
        /// Установка сид-слова
        /// </summary>
        /// <param name="seed">Сид-слово</param>
        private string setSeed(string seed = "random")
        {
            
            return _rnd.SetSeed(seed);
        }

        /// <returns>Сид-Слово</returns>
        public string GetSeed()
        {
            return _seed;
        }

        /// <summary>
        /// Односторонний алгоритм шифрования,
        /// основанный на генерации лабиринтов
        /// </summary>
        /// <param name="word">Слово шифровки</param>
        /// <returns>Зашифрованное слово</returns>
        public string GetEncryptMap(string word)
        {
            int[,] map = generateMap(16, 16, word);
            List<string> ready = new List<string>();
            string tempReady = "";
            int k = 0;

            for (int i = 0; i < map.GetLength(0); i++)
            {
                for (int j = 0; j < map.GetLength(1); j++)
                {
                    tempReady +=string.Join("",map[i, j]);
                    k++;
                    if(k == 8) { 
                        k = 0;
                        ready.Add(tempReady);
                        tempReady = "";
                    }
                }
            }
            string result = new string(ready.Select(i => (char)(Convert.ToInt32(i, 2))).ToArray());
          
            return result;

        }

        /// <summary>
        /// Установка размеров карты
        /// </summary>
        /// <param name="xSize">Размеры по X</param>
        /// <param name="ySize">Размеры по Y</param>
        private void setMapSize(int xSize, int ySize)
        {
            // Проварка на размеры не меньше 5
            if (xSize < 6) { xSize = 6; }
            if (ySize < 6) { ySize = 6; }

            // Установка размеров
            _mapSizeXY[0] = xSize;
            _mapSizeXY[1] = ySize;
        }
        private int[,] generateMap(int mapSizeX,
            int mapSizeY, string seed)
        {
            // Установка сид-слова
            setSeed(seed);

            // создание массива матрицы лабиринта
            int[,] map = new int[mapSizeY, mapSizeX];

            // Заполнение массива символом 1
            for (int y = 0; y < map.GetLength(0); y++) //_map.GetLength(0) - Y 
            {
                for (int x = 0; x < map.GetLength(1); x++) //_map.GetLength(1) - X
                {
                    map[y, x] = 1;
                }
            }

            // Задание случайных координат для начальной точки
            _arrowX = _rnd.Next(1, map.GetLength(1) - 1); // Случайный X
            _arrowY = _rnd.Next(1, map.GetLength(0) - 1); // Случайный Y

            // Расположение начальной точки в случайном месте
            map[_arrowY, _arrowX] = 0;

            // Запуск алгоритма генерации карты
            wormV2(_arrowX, _arrowY, map);
            return map;
        }
        
        /// <summary>
        /// Генерация нового уровня по размерам и сид-слову
        /// </summary>
        /// <param name="mapSizeX">Длинна по X</param>
        /// <param name="mapSizeY">Длинна по Y</param>
        /// <param name="seed">Сид-слово</param>
        /// <returns> Массив матрицы лабиринта </returns>
        public int[,] GetMap(int mapSizeX = 6,
            int mapSizeY = 6, string seed = "random")
        {
            // Установка размеров карты
            setMapSize(mapSizeX, mapSizeY);

            // Установка сид-слова
            _seed = setSeed(seed);


            int[,] map = generateMap(_mapSizeXY[0], _mapSizeXY[1], seed);
            // Создание стрелки на координатах начальной точки
            map[_arrowY, _arrowX] = 2;

            // Создание конечной точке в вычеслинных координатах самого длинного
            // пути от начальной точки
            map[_endPointY, _endPointX] = 3;
            _map = map;
            return map;
        }

        /// <summary>
        /// Алгоритм генерации от начальной точки
        /// </summary>
        /// <param name="x">Начальная координата X</param>
        /// <param name="y">Начальная координата Y</param>
        private int[,] wormV2(int x, int y, int[,] map) 
        {
            // Длинна самого длинного пути
            int maxWayLocal = 0;

            // Список с возможными путями
            List<Way> NewWays = new List<Way>();

            // Создание первой точки
            NewWays.Add(new Way(x, y, maxWayLocal));

            //Перебор всех возможных путей пока они есть
            while (NewWays.Count > 0)
            {
                // Визуализация каждого шага если включён дебаг-мод
                // Print();
                

                // Выбор случайого элемента из списка с возможными путями
                int randomIndex = _rnd.Next(0, NewWays.Count);
                Way Way = NewWays[randomIndex];

                // Вложенная функция для выполнения операции над выбранной клетко
                void swap(int directionSide, int xStep, int yStep)
                {
                    // true Если клетки вокруг позволяют проделать разрез
                    bool debug = boxCheck(Way.x + xStep,
                        Way.y + yStep, directionSide, map); 
                    if (debug)
                    {
                        // Создание разреза
                        map[Way.y + yStep, Way.x + xStep] = 0;

                        // Добавление новой точки в список возможных путей
                        NewWays.Add(new Way(Way.x + xStep,
                            Way.y + yStep, Way.MaxWayLocal + 1));

                        // Задание координат самой дальней точки 
                        if (Way.MaxWayLocal > maxWayLocal)
                        {
                            _endPointX = Way.x + xStep;
                            _endPointY = Way.y + yStep;
                            maxWayLocal = Way.MaxWayLocal;
                        }

                    }
                }

                // Всех сторон на возможные пути
                for (int i = 1; i < 5; i++)
                {
                    switch (i)
                    {
                        case 1: //Вправо
                                //1 - сдвиг по x
                                //0 - сдвиг по y
                                //i - это сторона, с которой прийдет сигнал
                            swap(i, 1, 0);                
                            break;
                        case 2: //Вверх
                                //0 - сдвиг по x
                                //-1 - сдвиг по y
                                //i - это сторона, с которой прийдет сигнал
                            swap(i, 0, -1);                  
                            break;
                        case 3: //Влево
                                //-1 - сдвиг по x
                                //0 - сдвиг по y
                                //i - это сторона, с которой прийдет сигнал
                            swap(i, -1, 0);                     
                            break;
                        case 4: //Вниз
                                //i - это сторона, с которой прийдет сигнал
                                //0 - сдвиг по x
                                //1 - сдвиг по y
                            swap(i, 0, 1);                      
                            break;
                    }
                }

                // Очистка данного пути, так как он закончен
                NewWays.RemoveAt(randomIndex);
            }
            _maxWay = maxWayLocal;
            return map;
        }

        /// <summary>
        /// Генерация нового уровня по размерам и сид-слову
        /// </summary>
        /// <param name="x">Координата X</param>
        /// <param name="y">Координата Y</param>
        /// <param name="directionSide">Направление прореза</param>
        private bool boxCheck(int x, int y, int directionSide, int[,] map)
        {
            switch (directionSide)
            {
                case 1: //Вправо
                    try
                    {
                        if (
                            map[y - 1, x] == 1 && //Сверху в центре
                            map[y - 1, x + 1] == 1 && //Сверху справа
                            map[y, x + 1] == 1 && //Справа в центре
                            map[y + 1, x] == 1 && //Снизу в центре
                            map[y + 1, x + 1] == 1 //Справа снизу
                          )
                        {
                            return true;
                        }
                    }
                    catch
                    {
                        return false;
                    }
                    break;
                case 2: //Вверх
                    try
                    {
                        if (
                            map[y - 1, x] == 1 && //Сверху в центре
                            map[y - 1, x + 1] == 1 && //Сверху справа
                            map[y - 1, x - 1] == 1 && //Сверху слева
                            map[y, x + 1] == 1 && //Справа в центре
                            map[y, x - 1] == 1  //Слева в центре
                          )
                        {
                            return true;
                        }
                    }
                    catch
                    {
                        return false;
                    }
                    break;
                case 3: //Влево
                    try
                    {
                        if (
                            map[y - 1, x] == 1 && //Сверху в центре                       
                            map[y - 1, x - 1] == 1 && //Сверху слева                        
                            map[y, x - 1] == 1 && //Слева в центре
                            map[y + 1, x - 1] == 1 && //Слева снизу
                            map[y + 1, x] == 1  //Слева в центре
                          )
                        {
                            return true;
                        }
                    }
                    catch
                    {
                        return false;
                    }
                    break;
                case 4: //Вниз
                    try
                    {
                        if (
                            map[y, x - 1] == 1 && //Слева в центре
                            map[y + 1, x - 1] == 1 && //Слева снизу
                            map[y + 1, x] == 1 && //Внизу в центре
                            map[y + 1, x + 1] == 1 && //Справа снизу
                            map[y, x + 1] == 1 //Справа в центре
                          )
                        {
                            return true;
                        }
                    }
                    catch
                    {
                        return false;
                    }
                    break;

            }
            return false;
        }

        /// <summary>
        /// Формирование списка клеток с метаданными
        /// по размерам и сид-слову уровня 
        /// </summary>
        /// <param name="mapSizeX">Длинна по X</param>
        /// <param name="mapSizeY">Длинна по Y</param>
        /// <param name="seed">Сид-слово</param>
        public List<Cell> GetLevel(int mapSizeX = 5, int mapSizeY = 5,
            string seed = "random")
        {
            _mapCell = new List<Cell>();
            _map = GetMap(mapSizeX, mapSizeY, seed);
            for (int y = 0; y < _map.GetLength(0); y++) //_map.GetLength(0) - Y 
            {
                for (int x = 0; x < _map.GetLength(1); x++) //_map.GetLength(1) - X
                {
                    _mapCell.Add(SetType(x, y));
                }
            }
            return _mapCell;
        }

        /// <summary>
        /// Структура для хранения сдвигов для окружающих клеток
        /// </summary>
        private struct Swap
        {
            public int x;
            public int y;
        };

        /// <summary>
        /// Список структур сдвига для окружающих клеток
        /// </summary>
        private List<Swap> swaps = new List<Swap>()
        {
            new Swap { x = -1, y = -1 },//Сверху слева 0
            new Swap { x = +1, y = -1 },//Сверху справа 1
            new Swap { x = +1, y = +1 },//Снизу справа 2
            new Swap { x = -1, y = +1 },//Снизу слева 3 

            new Swap { x = 0, y = -1 },//Сверху в центре 4    
            new Swap { x = +1, y = 0 },//Справа в центре 5          
            new Swap { x = 0, y = +1 },//Снизу в центре 6        
            new Swap { x = -1, y = 0 },//Слева в центре 7
        };

        /// <summary>
        /// Ислледование клетки для формирования метаданных
        /// </summary>
        /// <returns>Объект клетки с метаданными</returns>
        private Cell SetType(int x, int y)
        {
            // Очищение метаданных перед началом
            string metaInfo = "";
            string type = "Неизвестно";

            // Определение группы блока - стена(1) или пол(0)
            int group = _map[y, x] != 1 ? 0 : 1;

            // Определение типа блока - начальная точка(2), конечная точка(3) и любые другие
            int addons = _map[y, x];

            //Перебор всех блоков в оружении
            foreach (Swap swap in swaps)
            {
                try
                {
                    // Задание данных о клетке при текущем сдвиге
                    int result;
                    result = _map[y + swap.y, x + swap.x] != 1 ? 0 : 1;
                    metaInfo += result == group ? "1" : "0";// 1 - такой же блок
                }
                catch
                {
                    // не такой же блок, если индекс вышел за границы массива
                    metaInfo += "0";
                }
            }

            // Исследование метаданных об окружении
            switch (string.Join("", metaInfo.TakeLast(4)))
            {
                // Три блока со сторон
                case "1110": //Одна сторона влево
                    type = "SL";
                    break;
                case "0111": //Одна сторона вверх
                    type = "SU";
                    break;
                case "1011": //Одна сторона вправо
                    type = "SR";
                    break;
                case "1101": //Одна сторона вниз
                    type = "SD";
                    break;
                // два блока со сторон
                case "1100": //Две стороны влево вниз
                    type = "2SLD";
                    break;
                case "0110": //Две стороны влево вверх
                    type = "2SLU";
                    break;
                case "0011": //Две стороны вправо вверх
                    type = "2SRU";
                    break;
                case "1001": //Две стороны вправо вниз
                    type = "2SRD";
                    break;
                // Четыре блока со сторон
                case "1111": //Без сторон
                    type = "0S";
                    break;
            }

            // один угол
            if (metaInfo[4].ToString() + metaInfo[1].ToString() + metaInfo[5].ToString() == "101")
            {
                //угол вправо вверх
                type += "CRU";
            }
            if (metaInfo[5].ToString() + metaInfo[2].ToString() + metaInfo[6].ToString() == "101")
            {
                //угол вправо вниз
                type += "CRD";
            }
            if (metaInfo[6].ToString() + metaInfo[3].ToString() + metaInfo[7].ToString() == "101")
            {
                //угол влево вниз
                type += "CLD";
            }
            if (metaInfo[7].ToString() + metaInfo[0].ToString() + metaInfo[4].ToString() == "101")
            {
                //угол влево вверх
                type += "CLU";
            }

            switch (string.Join("", metaInfo.TakeLast(4)))
            {
                // Без блоков со сторон
                case "0000": //Четыре стороны
                    type = "4S";
                    break;
                // Две параллельных стороны
                case "0101": //Две стороны горизонт
                    type = "2SH";
                    break;
                case "1010": //Две стороны вертикаль
                    type = "2SV";
                    break;

                // Один блок со стороны
                case "1000": //Три стороны вниз
                    type = "3SD";
                    break;
                case "0100": //Три стороны влево
                    type = "3SL";
                    break;
                case "0010": //Три стороны вверх
                    type = "3SU";
                    break;
                case "0001": //Три стороны вправо
                    type = "3SR";
                    break;
            }

            // Создание объекта клетки с метаанными
            return new Cell(x, y, type, addons, group, metaInfo);
        }
    }
}