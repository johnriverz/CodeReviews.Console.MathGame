namespace mathGame.johnriverz
{
    public class MathGameLogic
    {
        //every operation we do will be saved here
        public List<string> GameHistory { get; set; } = new List<string>(); 

        // present user with options
        public void ShowMenu()
        {
            Console.WriteLine("Select an operation");
            Console.WriteLine("1. Summation\n2. Subtraction\n3. Multiplication\n4. Division\n5. Random Mode\n6. Show History\n7. Change Difficulty\n8. Exit");
        }

        // can perform any operation (modular)
        public int MathOperation(int firstNum, int secondNum, char operation)
        {
            switch(operation)
            {
                case '+':
                // log history
                    GameHistory.Add($"{firstNum} + {secondNum} = {firstNum + secondNum}");

                    return firstNum + secondNum;
                case '-':
                    GameHistory.Add($"{firstNum} - {secondNum} = {firstNum - secondNum}");

                    return firstNum - secondNum;
                case '*':
                    GameHistory.Add($"{firstNum} * {secondNum} = {firstNum * secondNum}");

                    return firstNum * secondNum;
                case '/':
                // check if out of range
                    while(firstNum < 0 || firstNum > 100)
                    {
                        try
                        {
                            Console.WriteLine("Enter a number between 0-100");
                            firstNum = Convert.ToInt32(Console.ReadLine());
                        }
                        catch (System.Exception)
                        {
                            // do nothing, while loop will try again
                        }
                    }
                    GameHistory.Add($"{firstNum} / {secondNum} = {firstNum / secondNum}");

                    return firstNum / secondNum;
            }
            return 0;
        }
    }
}