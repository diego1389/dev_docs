using System;
namespace DesignPatterns2.Proxy
{
    public class Math : IMath
    {
        public double Add(double x, double y) => x + y;

        public double Div(double x, double y) => x / y;

        public double Mul(double x, double y) => x * y;

        public double Sub(double x, double y) => x - y;
    }
}
