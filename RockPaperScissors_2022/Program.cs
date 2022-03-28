// See https://aka.ms/new-console-template for more information

//Game Configuration
var completeConfiguration = false;
var GameStart = true;
var neededToWin = 3;

var playerTwoHuman = false;
var playerTwoWeaknessBot = false;
var playerTwoRandomBot = false;

//Game State
var playerOneName = "Player One";
var playerTwoName = "Player Two";

var playerOneScore = 0;
var playerTwoScore = 0;

var playerOneChoice = string.Empty;
var playerTwoChoice = string.Empty;

var hasWinner = false;
var WinnerName = string.Empty;

//Bot State
var previousWeakness = string.Empty;

/* Game logic */
var playerChoices = new Dictionary<string, string>();
//choice name / choice weaknesses
playerChoices.Add("Rock", "Paper");
playerChoices.Add("Paper", "Scissors Flamethrower");
playerChoices.Add("Scissors", "Rock");
playerChoices.Add("FlameThrower", "Rock Scissors");

int maxChoices = playerChoices.Count - 1;

//configure game
while (!completeConfiguration)
{
    PlayerOpponentConfiguration();
}

//game loop
while (GameStart)
{

    playerOneChoice = ValidatePlayerInput(playerOneName);
    Console.Clear();

    if (playerTwoHuman)
    {
        playerTwoChoice = ValidatePlayerInput(playerTwoName);
    }

    if (playerTwoRandomBot)
    {
        playerTwoChoice = RandomBotTypeInput();
    }

    if (playerTwoWeaknessBot)
    {
        playerTwoChoice = PreviousWeaknessBotTypeInput();
    }

    DisplayPlayerSelections();

    ResolveRound();

    DisplayPlayerScores();

    HasAPlayerWonTheGame();

    DisplayWinnerOfGame();

}

void PlayerOpponentConfiguration()
{
    Console.WriteLine("Which will you be playing against?");
    Console.WriteLine();
    Console.WriteLine("[H]uman");
    Console.WriteLine("[R]andom Bot");
    Console.WriteLine("[W]eakness Bot");
    Console.WriteLine();
    Console.WriteLine("Press H, R or W.");

    var input = Console.ReadLine();

    if (!string.IsNullOrWhiteSpace(input))
    {
        switch (input?.ToUpper())
        {
            case "H":
                playerTwoHuman = true;
                completeConfiguration = true;
                break;
            case "R":
                playerTwoRandomBot = true;
                completeConfiguration = true;
                break;
            case "W":
                playerTwoWeaknessBot = true;
                completeConfiguration = true;
                break;
            default:
                break;
        }
    }

    if (completeConfiguration)
    {
        Console.WriteLine("Configuration Complete!");
        Console.WriteLine("Press any key to continue to the game.");
    }
    else
    {
        Console.WriteLine("The Option you chose wasn't available.");
    }
    Console.ReadLine();
    Console.Clear();
}

string ValidatePlayerInput(string playerName)
{

    bool IsANumber = false;
    bool IsValidObject = false;
    string? playerInput = string.Empty;

    while (!IsANumber || !IsValidObject)
    {
        Console.WriteLine($"{playerName.ToUpper()}, please select your item.");
        DisplayItemOptions();

        playerInput = Console.ReadLine();

        if (!InputIsNumber(playerInput))
        {
            DisplayNotANumberErrorMessage();
            continue;
        }

        IsANumber = true;

        if (playerChoices?.ElementAtOrDefault(Convert.ToInt32(playerInput)).Key is null)
        {
            DisplayNotWithinOptionsErrorMessage();
            continue;
        }

        IsValidObject = true;
    }

    return playerChoices.ElementAt(Convert.ToInt32(playerInput)).Key;
}

void DisplayItemOptions()
{
    Console.WriteLine($"Select one of these objects. Type the NUMBER (0-{maxChoices}) associated with your selection:");
    for (int i = 0; i < playerChoices.Count; i++)
    {
        Console.WriteLine($"{i}. {playerChoices.ElementAt(i).Key}");
    }

    Console.WriteLine();
}

bool InputIsNumber(string? playerInput)
{
    int number;
    if (int.TryParse(playerInput, out number))
    {
        return true;
    }

    return false;
}

void DisplayNotANumberErrorMessage()
{
    Console.WriteLine($"Please Enter a number (0-{playerChoices.Count})");
    Console.WriteLine();
    Console.WriteLine("Press any key to try again.");
    Console.ReadLine();
    Console.Clear();
}

void DisplayNotWithinOptionsErrorMessage()
{
    Console.WriteLine($"Please Enter a number between 0 and {maxChoices}.");
    Console.WriteLine();
    Console.WriteLine("Press any key to try again.");
    Console.ReadLine();
    Console.Clear();
}

string RandomBotTypeInput()
{
    var randomizer = new Random();
    var num = randomizer.Next(0, maxChoices);
    return playerChoices.ElementAt(num).Key;
}

string PreviousWeaknessBotTypeInput()
{
    //First round, randomizes the bot choice
    if (string.IsNullOrWhiteSpace(previousWeakness))
    {
        var randomizer = new Random();
        var num = randomizer.Next(0, maxChoices);
        var currentChoice = playerChoices.ElementAt(num).Key;
        previousWeakness = playerChoices[currentChoice].Split(' ').First();
        return currentChoice;
    }
    else
    {
        var currentChoice = previousWeakness;
        previousWeakness = playerChoices[currentChoice].Split(' ').First();
        return currentChoice;
    }
}

void DisplayPlayerSelections()
{
    Console.Clear();
    Console.WriteLine($"Player One Has: {playerOneChoice}.");
    Console.WriteLine($"Player Two Has: {playerTwoChoice}.");
}

void ResolveRound()
{
    var p1weaknesses = playerChoices[playerOneChoice].Split(' ');
    var p2weaknesses = playerChoices[playerTwoChoice].Split(' ');

    if (p1weaknesses.Contains(playerTwoChoice))
    {
        Console.WriteLine("Player Two Wins");
        playerTwoScore++;
    }
    else if (p2weaknesses.Contains(playerOneChoice))
    {
        Console.WriteLine("Player One Wins");
        playerOneScore++;
    }
    else
    {
        Console.WriteLine("Draw");
    }
}

void DisplayPlayerScores()
{
    //Show scores
    Console.WriteLine($"SCORE: {playerOneName} : {playerOneScore}");
    Console.WriteLine($"SCORE: {playerTwoName} : {playerTwoScore}");
    Console.WriteLine();
    Console.WriteLine("Press any key to complete this round.");
    Console.ReadLine();
    Console.Clear();
}

void HasAPlayerWonTheGame()
{
    if (playerOneScore == neededToWin)
    {
        hasWinner = true;
        WinnerName = playerOneName;
        GameStart = false;
    }

    if (playerTwoScore == neededToWin)
    {
        hasWinner = true;
        WinnerName = playerTwoName;
        GameStart = false;
    }


}

void DisplayWinnerOfGame()
{
    if (hasWinner)
    {
        Console.WriteLine("!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!");
        Console.WriteLine();
        Console.WriteLine($"{WinnerName} has WON THE GAME!!");
        Console.WriteLine();
        Console.WriteLine("!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!");
        Console.ReadLine();
    }
}
