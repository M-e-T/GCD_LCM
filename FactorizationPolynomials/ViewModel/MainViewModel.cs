using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Numerics;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Combinatorics.Collections;
using FactorizationPolynomials.Model;

namespace FactorizationPolynomials.ViewModel
{
    public class MainViewModel : BaseVM
    {
        private bool _isWorking; 
        public bool IsWorking
        {
            get
            {
                return _isWorking;
            }
            set
            {
                _isWorking = value;
                OnPropertyChanged();
            }
        }
        private ObservableCollection<GridItem> _table;
        public ObservableCollection<GridItem> Table 
        {
            get { return _table; }
            set {
                _table = value;
                OnPropertyChanged();
            }
        }

        private string _deggre;
        public string Deggre
        {
            get
            {
                return _deggre;
            }
            set
            {
                _deggre = value;
                OnPropertyChanged();
            }
        }
      
        private RelayCommand _start;
        public RelayCommand Start
        {
            get
            {
                return _start ?? (_start = new RelayCommand(command =>                
                {
                    Task.Run(() => {

                        var table = new ObservableCollection<GridItem>();

                        long max = (long)Math.Pow(Convert.ToInt32(Deggre) - 2, 3);
                        long Persent = PersentProgress(1, max);
                        var integers = Enumerable.Range(2, Convert.ToInt32(Deggre)-1);
                        IsWorking = true;
                        var c = new Combinations<int>(integers, 3, GenerateOption.WithRepetition);

                        int id = 0;
                        foreach(var item in c)
                        {
                            table.Add(new GridItem(++id, item[0], item[1], item[2]));
                        }
                        Table = table;
                        IsWorking = false;
                        /*for (int x = 2; x <= Convert.ToInt32(Deggre); x++)
                        {
                            for (int y = 2; y <= Convert.ToInt32(Deggre); y++)
                            {
                                for (int z = 2; z <= Convert.ToInt32(Deggre); z++)
                                {
                                    table.Add(new GridItem(x, y, z));
                                    i++;
                                    if(i % Persent == 0)
                                    {
                                        Progress = (int)((double)i / max  * 100);
                                    }
                                }
                            }
                        }*/
                    });
                }, command => IsWorking == false));
            }
        }
        protected long PersentProgress(long min, long max)
        {
            long Persent = (long)((max - min) / 100);
            if (Persent == 0)
                Persent++;
            return Persent;
        }
    }
    public class T
    {
        public long X { get; }
        public long Y { get; }
        public long Z { get; }
    }
    public class GridItem
    {
        public long Id { get; }
        public long X { get; }
        public long Y { get; }
        public long Z { get; }
        public long N { get; }
        public long GCD { get; }
        public long LCM { get; }
        public string Dividers { get; }
        public string DivedersLCM { get; }
        public GridItem(int id, long x, long y, long z)
        {
            X = x;
            Y = y;
            Z = z;

            N = x + y + z;
            GCD = Factorization.GCD(Factorization.GCD(x, y), z);
            LCM = Factorization.LCM(Factorization.LCM(x, y), z);
            Dividers = ToString(Factorization.GetDivisors(N));
            DivedersLCM = ToString(Factorization.GetDivisors(LCM));
            Id = id;
        }
        private string ToString(ICollection<long> collection)
        {
            if (collection.Count == 0)
                return "ПЧ";
            StringBuilder sb = new StringBuilder(); 
            foreach(var item in collection)
            {
                sb.Append(item + ", ");
            }
            sb.Remove(sb.Length - 2, 1);
            return sb.ToString();
        }
    }
    public class Factorization
    {
        /// <summary>
        /// GCD - the greatest common divisor
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <returns></returns>
        public static long GCD(long a, long b)
        {
            while (b != 0)
            {
                var temp = b;
                b = a % b;
                a = temp;
            }
            return a;
        }
        /// <summary>
        /// LCM - least common multiple
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <returns></returns>
        public static long LCM(long a, long b)
        {
            return Math.Abs(a * b) / GCD(a, b);
        }
        public static List<long> GetDivisors(long n)
        {
            if (n <= 0)
            {
                return null;
            }
            List<long> divisors = new List<long>();
            for (int i = 2; i <= Math.Sqrt(n); i++)
            {
                if (n % i == 0)
                {
                    divisors.Add(i);
                    if (i != n / i)
                    {
                        divisors.Add(n / i);
                    }
                }
            }
            divisors.Sort();
            return divisors;
        }
    }
}
