using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Caculator.Models
{
    public class CalculatorModel
    {
        public decimal FirstNumber { get; set; }
        public decimal SecondNumber { get; set; }
        public string Operation { get; set; }
        public decimal Result { get; set; }

        public decimal Calculate()
        {
            return Operation switch
            {
                "+" => FirstNumber + SecondNumber,
                "-" => FirstNumber - SecondNumber,
                "*" => FirstNumber * SecondNumber,
                "/" => SecondNumber != 0 ? FirstNumber / SecondNumber : throw new DivideByZeroException(),
                _ => throw new InvalidOperationException()
            };
        }
    }
}
