using System;
using System.Collections.Generic;
using System.Text;

namespace Maze.Lib.Helpers
{
    public class SecretRandom
    {
        private Random _random;

        private IEnumerator<int> _ESeed;

        public SecretRandom()
        {
            _random = new Random();
        }

        private void move()
        {
            if (!_ESeed.MoveNext())
            {
                _ESeed.Reset();
            }

            _random = new Random(_ESeed.Current);
        }

        public int Next(int one, int two)
        {
            if (_ESeed == null)
            {
                SetSeed("random");
            }

            move();
            return _random.Next(one, two);
        }

        public string SetSeed(string seed)
        {
            if (seed == "random")
            {
                _random = new Random();
                List<char> list = new List<char>();
                for (int i = 0; i < 16; i++)
                {
                    list.Add((char)_random.Next(0, 127));
                }

                seed = string.Join("", list);
            }

            _ESeed = seed.Select((char x) => Convert.ToInt32((byte)x)).ToList().GetEnumerator();
            return seed;
        }
    }
}
