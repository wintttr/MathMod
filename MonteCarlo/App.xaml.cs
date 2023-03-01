using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;

namespace MonteCarlo
{
    public class EmptyListException : Exception { }

    public class NegativeNumberException : Exception 
    {
        public int number { get; private set; }
        public NegativeNumberException(int x)
        {
            number = x;
        }
    }
    public partial class App : Application
    {

    }
}
