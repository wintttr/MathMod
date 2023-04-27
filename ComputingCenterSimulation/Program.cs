using System.Runtime.CompilerServices;

namespace ComputingCenterSimulation
{
    class Program
    {

        enum TaskType { NONE, ERROR, CORRECT };
        enum SpecialistDuty { SIGN_SORT, FIRST_ERROR, SECOND_ERROR, NONE };

        static Random _random = new();

        static bool CasePercent(int percent)
        {
            return _random.Next(0, 100) < percent;
        }
        
        static void Main(string[] args)
        {
            int TaskCount = 100;
            int TaskDelay = 2;
            int ErrorPercent = 70;
            int ErrorSolveTime = 3;
            int SignSortTime = 12;

            Queue<TaskType> InputQueue = new();
            Queue<TaskType> PC1TaskQueue = new();
            Queue<TaskType> PC2TaskQueue = new();

            int Time = 0;

            int TasksPC1 = 0;
            int TasksPC1WE = 0;

            int TasksPC2 = 0;
            int TasksPC2WE = 0;

            int PC1WorkTime = 0;
            int PC2WorkTime = 0;

            int PC1DownTime = 0;
            int PC2DownTime = 0;

            (int, TaskType) PC1CurrentTask = (0, TaskType.NONE);
            (int, TaskType) PC2CurrentTask = (0, TaskType.NONE);

            int specialistTime = 0;

            int specialistWork = 0;
            int specialistIdle = 0;
            int specialistError = 0;

            int errorTime = 0;
            int errorCount = 0;
            SpecialistDuty specialistDuty = SpecialistDuty.NONE;
            SpecialistDuty prevDuty = SpecialistDuty.NONE;

            for(; TasksPC1 + TasksPC2 < TaskCount; Time++)
            {
                if (Time % TaskDelay == 0)
                    InputQueue.Enqueue(CasePercent(ErrorPercent) ? TaskType.ERROR : TaskType.CORRECT);

                if (InputQueue.Count != 0 && specialistDuty == SpecialistDuty.NONE)
                {
                    specialistDuty = SpecialistDuty.SIGN_SORT;
                    specialistTime = SignSortTime;
                }

                if (PC1CurrentTask.Item1 > 0)
                {
                    PC1CurrentTask.Item1--;
                    PC1WorkTime++;
                }
                else
                    PC1DownTime++;

                if(PC2CurrentTask.Item1 > 0)
                {
                    PC2CurrentTask.Item1--;
                    PC2WorkTime++;
                }
                else
                    PC2DownTime++;

                if (PC1CurrentTask.Item1 == 0)
                {

                    switch (PC1CurrentTask.Item2)
                    {
                        case TaskType.CORRECT:
                            TasksPC1++;
                            PC1CurrentTask.Item1 = 0;
                            PC1CurrentTask.Item2 = TaskType.NONE;
                            break;
                        case TaskType.ERROR:
                            if (specialistDuty != SpecialistDuty.FIRST_ERROR && specialistDuty != SpecialistDuty.SECOND_ERROR)
                            {
                                TasksPC1WE++;
                                errorTime = ErrorSolveTime;
                                prevDuty = specialistDuty;
                                specialistDuty = SpecialistDuty.FIRST_ERROR;
                            }
                            break;
                    }
                }

                if (PC2CurrentTask.Item1 == 0)
                {

                    switch (PC2CurrentTask.Item2)
                    {
                        case TaskType.CORRECT:
                            TasksPC2++;
                            PC2CurrentTask.Item1 = 0;
                            PC2CurrentTask.Item2 = TaskType.NONE;
                            break;
                        case TaskType.ERROR:
                            if (specialistDuty != SpecialistDuty.FIRST_ERROR && specialistDuty != SpecialistDuty.SECOND_ERROR)
                            {
                                TasksPC2WE++;
                                errorTime = ErrorSolveTime;
                                prevDuty = specialistDuty;
                                specialistDuty = SpecialistDuty.SECOND_ERROR;
                            }
                            break;
                    }
                }

                if (PC1TaskQueue.Count != 0 && PC1CurrentTask.Item2 == TaskType.NONE)
                {
                    TaskType newTask = PC1TaskQueue.Dequeue();
                    PC1CurrentTask = (10, newTask);
                }

                if (PC2TaskQueue.Count != 0 && PC2CurrentTask.Item2 == TaskType.NONE)
                {
                    TaskType newTask = PC2TaskQueue.Dequeue();
                    PC2CurrentTask = (10, newTask);
                }

                switch(specialistDuty)
                {
                    case SpecialistDuty.SIGN_SORT:
                        if (specialistTime > 0)
                        {
                            specialistTime--;
                            specialistWork++;
                        }

                        if(specialistTime == 0)
                        {
                            if (CasePercent(50))
                                PC2TaskQueue.Enqueue(InputQueue.Dequeue());
                            else
                                PC1TaskQueue.Enqueue(InputQueue.Dequeue());

                            specialistDuty = SpecialistDuty.NONE;
                        }
                        break;

                    case SpecialistDuty.FIRST_ERROR:
                        if (errorTime > 0)
                        {
                            errorTime--;
                            specialistError++;
                        }

                        if (errorTime == 0)
                        {
                            errorCount++;
                            PC1TaskQueue.Enqueue(TaskType.CORRECT);
                            PC1CurrentTask = (0, TaskType.NONE);

                            specialistDuty = prevDuty;
                        }
                        break;

                    case SpecialistDuty.SECOND_ERROR:
                        if (errorTime > 0)
                        {
                            errorTime--;
                            specialistError++;
                        }

                        if (errorTime == 0)
                        {
                            errorCount++;
                            PC2TaskQueue.Enqueue(TaskType.CORRECT);
                            PC2CurrentTask = (0, TaskType.NONE);

                            specialistDuty = prevDuty;
                        }
                        break;

                    case SpecialistDuty.NONE:
                        specialistIdle++;
                        break;
                }
            }
            

            Console.WriteLine($"Количество заданий: {TaskCount}");
            Console.WriteLine();
            Console.WriteLine("---");
            Console.WriteLine();
            Console.WriteLine($"Время работы: {Time} мин");
            Console.WriteLine();
            Console.WriteLine("---");
            Console.WriteLine();
            Console.WriteLine($"{InputQueue.Count} задач осталось в очереди");
            Console.WriteLine();
            Console.WriteLine("---");
            Console.WriteLine();
            Console.WriteLine($"Полезное время 1 ЭВМ: {PC1WorkTime} мин, количество заданий: {TasksPC1}, из них с ошибкой {TasksPC1WE}");
            Console.WriteLine($"Полезное время 2 ЭВМ: {PC2WorkTime} мин, количество заданий: {TasksPC2}, из них с ошибкой {TasksPC2WE}");
            Console.WriteLine();
            Console.WriteLine("---");
            Console.WriteLine();
            Console.WriteLine($"Время простоя 1 ЭВМ: {PC1DownTime} мин");  
            Console.WriteLine($"Время простоя 2 ЭВМ: {PC2DownTime} мин");
            Console.WriteLine();
            Console.WriteLine("---");
            Console.WriteLine();
            Console.WriteLine($"Время регистрации и сортировки: {specialistWork}");
            Console.WriteLine($"Время простоя оператора: {specialistIdle}");
            Console.WriteLine($"Время исправления ошибок: {specialistError}");
        }
    }
}