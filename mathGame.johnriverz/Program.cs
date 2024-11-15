using System.Diagnostics;
using System.Linq.Expressions;
using mathGame.johnriverz;

MathGameLogic mathGame = new MathGameLogic();
Random random = new Random();

int firstNum;
int secondNum;
int userMenuSelection;
int score = 0;
bool gameOver = false;

DifficultyLevel difficultyLevel = DifficultyLevel.Easy;


// game loop
while(!gameOver)
{
    userMenuSelection = GetUserMenuSelection(mathGame);

    firstNum = random.Next(1, 101);
    secondNum = random.Next(1, 101);

    switch(userMenuSelection)
    {
        case 1:
            score += await PerformOperation(mathGame, firstNum, secondNum, score, '+', difficultyLevel);
            break;
        case 2:
            score += await PerformOperation(mathGame, firstNum, secondNum, score, '-', difficultyLevel);
            break;
        case 3:
            score += await PerformOperation(mathGame, firstNum, secondNum, score, '*', difficultyLevel);
            break;
        case 4:
            while(firstNum % secondNum != 0)
            {
                firstNum = random.Next(1, 101);
                secondNum = random.Next(1, 101);
            }
            score += await PerformOperation(mathGame, firstNum, secondNum, score, '/', difficultyLevel);
            break;
        case 5:
            int numberOfQuestions = 99;
            Console.WriteLine("Number of questions?");

            while(!int.TryParse(Console.ReadLine(), out numberOfQuestions))
            {
                Console.WriteLine("Invalid. Enter number of questions you would like to attempt as an integer");
            }

            while(numberOfQuestions > 0)
            {
                int randomOperation = random.Next(1, 5);

                if(randomOperation == 1)
                {
                    firstNum = random.Next(1, 101);
                    secondNum = random.Next(1, 101);  

                    score += await PerformOperation(mathGame, firstNum, secondNum, score, '+', difficultyLevel);
                }
                else if(randomOperation == 2)
                {
                    firstNum = random.Next(1, 101);
                    secondNum = random.Next(1, 101);  

                    score += await PerformOperation(mathGame, firstNum, secondNum, score, '-', difficultyLevel);
                }
                else if(randomOperation == 3)
                {
                    firstNum = random.Next(1, 101);
                    secondNum = random.Next(1, 101);  

                    score += await PerformOperation(mathGame, firstNum, secondNum, score, '*', difficultyLevel);
                }
                else 
                {
                    firstNum = random.Next(1, 101);
                    secondNum = random.Next(1, 101);  

                    while(firstNum % secondNum != 0)
                    {
                        firstNum = random.Next(1, 101);
                        secondNum = random.Next(1, 101);
                    }

                    score += await PerformOperation(mathGame, firstNum, secondNum, score, '/', difficultyLevel);
                }

                numberOfQuestions--;
            }
            break;
        case 6:
            Console.WriteLine("Game history:");
            foreach (var operation in mathGame.GameHistory)
            {
                Console.WriteLine($"{operation}");
            }
            break;
        case 7:
            difficultyLevel = ChangeDifficulty();
            DifficultyLevel difficultyEnum = (DifficultyLevel)difficultyLevel;
            Enum.IsDefined(typeof(DifficultyLevel), difficultyEnum);

            Console.WriteLine($"New difficulty level: {difficultyLevel}");
            break;
        case 8:
            gameOver = true;
            Console.WriteLine($"Final score: {score}");
            break;
    }
}

static DifficultyLevel ChangeDifficulty()
{
    int userSelection = 0;

    Console.WriteLine("Please enter a difficulty level");
    Console.WriteLine("1. Easy\n2. Medium\n3. Hard");

    while(!int.TryParse(Console.ReadLine(), out  userSelection) || (userSelection < 1 || userSelection > 3))
    {
        Console.WriteLine("Enter a valid option from 1-3");
    }

    // converted switch statement to expression
    return userSelection switch
    {
        1 => DifficultyLevel.Easy,
        2 => DifficultyLevel.Medium,
        3 => DifficultyLevel.Hard,
        _ => DifficultyLevel.Easy,
    };
}

static void DisplayMathGameQuestion(int firstNum, int secondNum, char operation)
{
    Console.WriteLine($"{firstNum} {operation} {secondNum} = ???");
}

static int GetUserMenuSelection(MathGameLogic mathGame)
{
    int selection = -1;
    mathGame.ShowMenu();

    while(selection < 1 || selection > 8)
    {
        while(!int.TryParse(Console.ReadLine(), out selection))
        {
            Console.WriteLine("Enter a valid option from 1-8");
        }

        // logically equivalent to the above while loop
        if(!(selection >= 1 && selection <= 8))
        {
            Console.WriteLine("Enter a valid option from 1-8");
        }
    }

    return selection;
}

static async Task<int?> GetUserResponse(DifficultyLevel difficulty)
{
    int response = 0;
    int timeout = (int)difficulty;

    Stopwatch stopwatch = new Stopwatch();
    stopwatch.Start();

    Task<string?> getUserInputTask = Task.Run(() => Console.ReadLine());

    try
    {
        string? result = await Task.WhenAny(getUserInputTask, Task.Delay(timeout * 1000)) == getUserInputTask ? getUserInputTask.Result : null;

        stopwatch.Stop();

        if(result != null && int.TryParse(result, out response))
        {
            Console.WriteLine($"Time taken to answer: {stopwatch.Elapsed.ToString(@"mm\:ss\.fff")}");
            return response;
        }
        else
        {
            throw new OperationCanceledException();
        }
    }
    catch(OperationCanceledException)
    {
        Console.WriteLine("Time up");
        return null;
    }
}

static int ValidateResult(int result, int? userResponse, int score)
{
    if(result == userResponse)
    {
        Console.WriteLine("Congratulations, you answered correctly (+5 pts)");
        score += 5;
    }
    else
    {
        Console.WriteLine("Try again (+0 pts)");
        Console.WriteLine($"Correct answer is: {result}");
    }

    return score;
}

// calls GetUserResponse()
static async Task<int> PerformOperation(MathGameLogic mathGame, int firstNum, int secondNum, int score, char operation, DifficultyLevel difficulty)
{
    int result;
    int? userResponse;

    DisplayMathGameQuestion(firstNum, secondNum, operation);
    result = mathGame.MathOperation(firstNum, secondNum, operation);

    userResponse = await GetUserResponse(difficulty);
    score += ValidateResult(result, userResponse, score);

    return score;
}

public enum DifficultyLevel
{
    Easy = 45,
    Medium = 30,
    Hard = 15
}