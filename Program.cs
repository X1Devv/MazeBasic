class Program
{
    static Random random = new Random();

    static char dog = '@', symbol;
    static bool ReachedFinish = false;
    static int dogX, dogY;
    static int FinishX, FinishY, JetPackX, JetPackY;
    static int dX, dY, newX, newY;
    static int Width = 10, Height = 10;
    static int RemainingTime = 300;
    static bool JetPackUsed;
    static bool JetPackIsAvaliable = false;

    static char[,] Field = new char[Height, Width];
    static Thread timerThread;

    static char[,] GenerateField()
    {
        for (int i = 0; i < Height; i++)
        {
            for (int j = 0; j < Width; j++)
            {
                var symbol = random.Next(0, 100);
                if (symbol < 40)
                    symbol = '#';
                else
                    symbol = '.';
                Field[i, j] = (char)symbol;
            }
        }

        Field[FinishY, FinishX] = 'F';
        Field[JetPackY, JetPackX] = 'J';

        return Field;
    }

    static void GenerationOfCoordinatesForElements()
    {
        dogX = random.Next(0, Width);
        dogY = random.Next(0, Height);

        JetPackX = random.Next(0, Width);
        JetPackY = random.Next(0, Height);

        FinishX = random.Next(0, Width);
        FinishY = random.Next(0, Height);
    }

    static void Draw()
    {
        Console.Clear();
        for (int i = 0; i < Height; i++)
        {
            for (int j = 0; j < Width; j++)
            {
                if (i == dogY && j == dogX)
                    Console.Write(dog + " ");
                else
                    Console.Write(Field[i, j] + " ");
            }
            Console.WriteLine();
        }
    }

    static void GetInput()
    {

        char input = Console.ReadKey(true).KeyChar;

        if (input == 'W' || input == 'w')
            dY = -1;
        else if (input == 'A' || input == 'a')
            dX = -1;
        else if (input == 'S' || input == 's')
            dY = 1;
        else if (input == 'D' || input == 'd')
            dX = 1;
    }

    #region Move logic
    static bool IsWalkable(int X, int Y)
    {
        return Field[Y, X] != '#';
    }

    static bool CanGoTo(int newX, int newY)
    {
        if (newX < 0 || newY < 0 || newX >= Width || newY >= Height)
            return false;
        if (!IsWalkable(newX, newY))
            return false;

        return true;
    }

    static void GoTo(int newX, int newY)
    {
        (dogX, dogY) = (newX, newY);
    }

    static void TryGoTo(int newX, int newY)
    {
        if (CanGoTo(newX, newY))
            GoTo(newX, newY);
    }
    #endregion

    static bool IsEndGame()
    {
        return ReachedFinish || RemainingTime <= 0;
    }

    static void Timer()
    {
        while (RemainingTime > 0 && !ReachedFinish)
        {
            Thread.Sleep(1000);
            RemainingTime--;
        }
    }

    static bool CheckFinish()
    {
        if (dogX == FinishX && dogY == FinishY)
            return ReachedFinish = true;
        return false;
    }

    static void JetPackLogic()
    {
        if (dogX == JetPackX && dogY == JetPackY && !JetPackIsAvaliable)
        {
            JetPackIsAvaliable = true;
            Field[JetPackY, JetPackX] = '.';
        }

        if (JetPackIsAvaliable && !JetPackUsed)
        {
            if (Field[dogY + dY, dogX + dX] == '#')
            {
                JetPackUsed = true;
                dogX += dX;
                dogY += dY;
            }
        }
    }
    static void Logic()
    {
        newX = dogX + dX;
        newY = dogY + dY;
        JetPackLogic();
        TryGoTo(newX, newY);
        dX = 0;
        dY = 0;
        CheckFinish();
    }

    static void Main(string[] args)
    {
        GenerationOfCoordinatesForElements();
        GenerateField();

        timerThread = new Thread(Timer);
        timerThread.Start();

        while (!IsEndGame())
        {
            Draw();
            //Console.WriteLine($"Finish coordinates:{FinishX}, {FinishY}");
            Console.WriteLine($"Remaining time:{RemainingTime} seconds");
            //Console.WriteLine($"JetPack:{JetPackIsAvaliable}\nJetPack used:{JetPackUsed}\nCoordinates JetPack:{JetPackX} {JetPackY}\nCoordinates Dog:{dogX}, {dogY}");

            GetInput();
            Logic();
        }
        Console.WriteLine("Game end!");

    }
}
