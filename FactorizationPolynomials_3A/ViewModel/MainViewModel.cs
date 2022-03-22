using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Combinatorics.Collections;

using FactorizationPolynomials_3A.Model;

namespace FactorizationPolynomials_3A.ViewModel
{
    public class MainViewModel : BaseVM
    {
        private ObservableCollection<GridItem> _defaultTable;
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
        private bool _isTableEmpty;
        public bool IsTableEmpty
        {
            get
            {
                return _isTableEmpty;
            }
            set
            {
                _isTableEmpty = value;
                OnPropertyChanged();
            }
        }
        
        private ObservableCollection<GridItem> _table;
        public ObservableCollection<GridItem> Table 
        {
            get {

                return _table; 
            }
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
        private string _n;
        public string N
        {
            get { return _n; }
            set {
                _n = value;
                OnPropertyChanged();
                Sort();
            }
        }
        /*private string _GCD;
        public string GCD
        {
            get { return _GCD; }
            set
            {
                _GCD = value;
                OnPropertyChanged();
                Sort();
            }
        } */
        private string _GCDbyLCMandN;
        public string GCDbyLCMandN
        {
            get { return _GCDbyLCMandN; }
            set
            {
                _GCDbyLCMandN = value;
                OnPropertyChanged();
                Sort();
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
                        var integers = Enumerable.Range(2, Convert.ToInt32(Deggre)-1);
                      
                        IsWorking = true;
                        var c = new Combinations<int>(integers, 3, GenerateOption.WithRepetition);

                        int id = 0;
                        foreach(var item in c)
                        {
                            table.Add(new GridItem(++id, item[0], item[1], item[2]));
                        }
                        Table = table;
                        _defaultTable = table;
                        
                        IsTableEmpty = !(Table == null);
                        IsWorking = false;
                    });
                }, command => IsWorking == false));
            }
        }
        private RelayCommand _reset;
        public RelayCommand Reset
        {
            get
            {            
                return _reset ?? (_reset = new RelayCommand(command => 
                {
                    Table = null;
                    _defaultTable = null;
                    IsTableEmpty = !(Table == null);

                    Deggre = "";
                    N = "";
                    //GCD = "";
                }, command => true));
            }
        }
        private void Sort()
        {
            if (_defaultTable == null)
                return;
            if (string.IsNullOrWhiteSpace(N) /*&& string.IsNullOrWhiteSpace(GCD)*/ && string.IsNullOrWhiteSpace(GCDbyLCMandN))
            {
                Table = _defaultTable;
                for (int i = 0; i < Table.Count; i++)
                {
                    Table[i].Id = i + 1;
                }
                return;
            }

            var table = new List<GridItem>();
            if (IsInt(N))
            {
                table.AddRange(_defaultTable.Where(x => x.N == int.Parse(N)));
                /*if (IsInt(GCD))
                {
                    table = table.Where(x => x.GCD == int.Parse(GCD)).ToList();
                }*/
                if (IsInt(GCDbyLCMandN))
                {
                    table = table.Where(x => x.GCDbyLCMandN == int.Parse(GCDbyLCMandN)).ToList();
                }
            }
            /*else if (IsInt(GCD))
            {
                table.AddRange(_defaultTable.Where(x => x.GCD == int.Parse(GCD)));
                if (IsInt(N))
                {
                    table = table.Where(x => x.N == int.Parse(N)).ToList();      
                }
                if (IsInt(GCDbyLCMandN))
                {
                    table = table.Where(x => x.GCDbyLCMandN == int.Parse(GCDbyLCMandN)).ToList();
                }
            }*/
            else if(IsInt(GCDbyLCMandN))
            {
                table = _defaultTable.Where(x => x.GCDbyLCMandN == int.Parse(GCDbyLCMandN)).ToList();
                if (IsInt(N))
                {
                    table = table.Where(x => x.N == int.Parse(N)).ToList();
                }
                /*if (IsInt(GCD))
                {
                    table = table.Where(x => x.GCD == int.Parse(GCD)).ToList();
                }*/
            }
            Table = new ObservableCollection<GridItem>(table);     
            for(int i = 0; i < Table.Count; i++)
            {
                Table[i].Id = i + 1;
            }
        }
        private bool IsInt(string value)
        {
            return int.TryParse(value, out int res);
        }
    }
    public class GridItem
    {
        public long Id { get; set; }
        public long X { get; }
        public long Y { get; }
        public long Z { get; }
        public long N { get; }
        public long GCD { get; }
        public long LCM { get; }
        public long GCDbyLCMandN { get; }
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
            GCDbyLCMandN = Factorization.GCD(LCM, N);
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
