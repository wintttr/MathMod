namespace ComputingCenterSimulation
{
    class Program
    {
        enum UsedComputer { FIRST, SECOND, NONE };

        static Random _random = new();

        static bool CasePercent(int percent)
        {
            return _random.Next(0, 100) < percent;
        }
        
        static void Main(string[] args)
        {
            int TaskCount = 0;

            while (true)
            {
                Console.Write("Введите количество заданий: ");
                try
                {
                    TaskCount = Int32.Parse(Console.ReadLine());
                    break;
                }
                catch (Exception)
                {
                    Console.WriteLine("Неверный формат числа, попробуйте ещё раз.");
                    Console.WriteLine();
                }
            }
            Console.WriteLine();

            int TSSAllTime = 0;
            int CurrentTaskSignSort = 0;
            int TaskSignSortTime = 0;

            Queue<int> TaskList = new();

            Queue<int> TaskListPC1 = new();
            Queue<int> TaskListPC2 = new();

            int SpecialistTime = 0;
            int SpecialistInaction = 0;

            UsedComputer used = UsedComputer.NONE;

            int Time = 0;

            int TimePC1 = 0;
            int TimePC2 = 0;

            int TasksPC1 = 0;
            int TasksPC2 = 0;


            for(; TasksPC1 + TasksPC2 < TaskCount; Time++)
            {
                if (TaskSignSortTime > 0)
                {
                    --TaskSignSortTime;
                    TSSAllTime++;
                }

                if (TaskSignSortTime == 0)
                {
                    if (CurrentTaskSignSort != 0)
                    {
                        if (TaskListPC1.Count > TaskListPC2.Count)
                            TaskListPC2.Enqueue(CurrentTaskSignSort);
                        else
                            TaskListPC1.Enqueue(CurrentTaskSignSort);
                        CurrentTaskSignSort = 0;
                    }
                    
                }

                if (Time % 2 == 0)
                    TaskList.Enqueue(CasePercent(70) ? 23 : 10);

                if(TaskList.Count != 0 && TaskSignSortTime == 0)
                {
                    TaskSignSortTime = 12;
                    CurrentTaskSignSort = TaskList.Dequeue();
                }

                if(used == UsedComputer.FIRST)
                {
                    if (SpecialistTime > 0)
                    {
                        SpecialistTime--;
                        TimePC1++;
                    }
                    
                    if (SpecialistTime == 0)
                    {
                        TasksPC1++;

                        TaskListPC1.Dequeue();
                        used = UsedComputer.NONE;
                    }
                }

                if(used == UsedComputer.SECOND)
                {
                    if (SpecialistTime > 0)
                    {
                        SpecialistTime--;
                        TimePC2++;
                    }
                    
                    if (SpecialistTime == 0)
                    {
                        TasksPC2++;
                        TaskListPC2.Dequeue();
                        used = UsedComputer.NONE;
                    }
                }

                if(used == UsedComputer.NONE)
                {
                    if (TaskListPC1.Count > TaskListPC2.Count && TaskListPC1.Count != 0)
                    {
                        used = UsedComputer.FIRST;
                        SpecialistTime = TaskListPC1.First();
                    }
                    else if (TaskListPC1.Count <= TaskListPC2.Count && TaskListPC2.Count != 0)
                    {
                        used = UsedComputer.SECOND;
                        SpecialistTime = TaskListPC2.First();
                    }
                    else
                    {
                        SpecialistInaction++;
                    }
                }
            }

            Console.WriteLine("Количество заданий: {0}", TaskCount);
            Console.WriteLine();
            Console.WriteLine("---");
            Console.WriteLine();
            Console.WriteLine("Время выполнения: {0} мин", Time);
            Console.WriteLine();
            Console.WriteLine("---");
            Console.WriteLine();
            Console.WriteLine("Полезное время 1 ЭВМ: {0} мин, количество заданий: {1}", TimePC1, TasksPC1);
            Console.WriteLine("Полезное время 2 ЭВМ: {0} мин, количество заданий: {1}", TimePC2, TasksPC2);
            Console.WriteLine();
            Console.WriteLine("---");
            Console.WriteLine();
            Console.WriteLine("Время простоя 1 ЭВМ: {0} мин", Time - TimePC1);  
            Console.WriteLine("Время простоя 2 ЭВМ: {0} мин", Time - TimePC2);
            Console.WriteLine();
            Console.WriteLine("---");
            Console.WriteLine();
            Console.WriteLine("Время бездействия оператора: {0}", SpecialistInaction);
        }
    }
}