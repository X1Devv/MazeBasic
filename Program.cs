using System;

class Program
{
    static char dog = '@', symbol;
    static Random random = new Random();
    static bool Reached_finish = false;
    static int dogX, dogY, FinishX, FinishY, dX, dY, newX, newY, Width = 10, Height = 10;
    static char[,] Field = new char[Height, Width];

    static char[,] GenerateField()
    {
        for (int i = 0; i < Height; i++)
        {
            for (int j = 0; j < Width; j++)
            {
                symbol = random.Next(0, 100) < 40 ? '#' : '.';
                Field[i, j] = symbol;
            }
        }

        FinishX = random.Next(0, Width);
        FinishY = random.Next(0, Height);
        Field[FinishY, FinishX] = 'F';

        return Field;
    }

    static void PlaceDog()
    {
        dogX = random.Next(0, Width);
        dogY = random.Next(0, Height);
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
        dX = 0;
        dY = 0;

        string input = Console.ReadLine();

        if (input == "W" || input == "w") 
            dY = -1;
        else if (input == "A" || input == "a") 
            dX = -1;
        else if (input == "S" || input == "s") 
            dY = 1;
        else if (input == "D" || input == "d") 
            dX = 1;
    }

    static bool IsEndGame()
    {
        return Reached_finish;
    }

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

    static bool CheckFinish()
    {
        if (dogX == FinishX && dogY == FinishY)
            return Reached_finish = true;
        return false;
    }

    static void Logic()
    {
        newX = dogX + dX;
        newY = dogY + dY;
        TryGoTo(newX, newY);
        dX = 0;
        dY = 0;
        CheckFinish();
    }

    static void Main(string[] args)
    {
        GenerateField();
        PlaceDog();

        while (!IsEndGame())
        {
            Draw();
            //Console.WriteLine($"cords: ({FinishX}, {FinishY})");
            GetInput();
            Logic();
        }
        Console.WriteLine("Finish!");
    }
}
