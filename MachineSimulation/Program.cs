namespace MachineSimulation
{
    public struct RandomPair
    {
        public List<double> z;
     
        public RandomPair()
        {
            z = new();
        }


        public int Count
        {
            get { return z.Count; }
        }

        public void Push(double x)
        {
            if (Count > 2)
                throw new Exception();
            else
                z.Add(x);
        }

        public double Pop()
        {
            if (z.Count == 0)
                throw new Exception();
            else
            {
                double temp = z[z.Count - 1];
                z.RemoveAt(z.Count - 1);
                return temp;
            }
        }

    }

    public class Program
    {
        static Random random = new Random();

        static RandomPair rp = new RandomPair();


        static double UniformDistrib(double a, double b)
        {
            return a + (b - a) * random.NextDouble();
        }

        static double ExpDistrib(double a, double b)
        {
            return Math.Log(UniformDistrib(Math.Exp(a), Math.Exp(b)));
        }

        static double NormalDistribImpl()
        {
            if (rp.Count == 0)
            {
                double phi, r;

                do { phi = UniformDistrib(0, 1); } while (phi == 0);
                do { r = UniformDistrib(0, 1); } while (r == 0);

                (double z1, double z2) = (Math.Cos(2 * Math.PI * phi) * Math.Sqrt(-2 * Math.Log(r)), Math.Sin(2 * Math.PI * phi) * Math.Sqrt(-2 * Math.Log(r)));
                rp.Push(z1);
                rp.Push(z2);
            }

            return rp.Pop();
        }

        static double NormalDistrib(double expectedval, double deviation)
        {
            return expectedval + deviation * NormalDistribImpl();
        }

        static void Main()
        {
            const int DetailsCount = 500;

            double PassedTime = 0;

            double NextBreak = PassedTime + NormalDistrib(20, 2);
            double BreakCount = 0;
            double BreakTime = 0;

            double UsefulTime = 0;

            double DelayTime = 0;

            for (int i = 0; i < DetailsCount; i++)
            {
                double MachineBreak = 0;
                if (PassedTime >= NextBreak)
                {
                    MachineBreak = UniformDistrib(0.1, 0.5);

                    PassedTime += MachineBreak;
                    BreakTime += MachineBreak;

                    NextBreak = PassedTime + NormalDistrib(20, 2);
                    BreakCount++;
                }

                double MachineSetup = UniformDistrib(0.2, 0.5);
                double DetailTime = MachineSetup + NormalDistrib(0.5, 0.1);

                double DetailDelay;
                if (DetailTime + MachineBreak > 1)
                    DetailDelay = 0;
                else
                    DetailDelay = ExpDistrib(0, 1.2 - (DetailTime + MachineBreak));

                UsefulTime += DetailTime;
                DelayTime += DetailDelay;
                PassedTime += (DetailTime + DetailDelay);
            }

            Console.WriteLine("Общее время работы: {0:F2} часов", PassedTime);

            Console.WriteLine();

            Console.WriteLine("Количество деталей: {0}", DetailsCount);
            Console.WriteLine("Время, потраченное на наладку станка и обработку деталей: {0:F2} часов", UsefulTime);

            Console.WriteLine();

            Console.WriteLine("Время простоя (время ожидания деталей): {0:F2} часов", DelayTime);

            Console.WriteLine();

            Console.WriteLine("Количество поломок: {0}", BreakCount);
            Console.WriteLine("Время, потраченное на починку поломок: {0:F2}", BreakTime);

            Console.WriteLine();

        }
    }
}