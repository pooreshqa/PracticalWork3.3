using System;

namespace course_work
{
    class Program
    {
        static void Main(string[] args)
        {   
            gameManager.Instance.BaseUnitFactory = new BaseFactory();
            gameManager.Instance.MortalUnitFactory = new MortalFactory();
            gameManager.Instance.timer = 10;
            gameManager.Instance.w = 6;
            if (gameManager.Instance.level == 0) {
                int a = gameManager.Instance.w;
                gameManager.Instance.grid = new Grid(a, a);
            }
            string message = "";
            while (true)
            {
                Console.Clear();
                gameManager.Instance.grid.draw();
                if (message != "") { Console.WriteLine(message); message = ""; }
                if (gameManager.Instance.GameOver) {
                    Console.WriteLine($"Здоровье: {gameManager.Instance.player.health} | Время: {gameManager.Instance.timer} | Уровень: {gameManager.Instance.level}\n0 - Выйти");
                } else {
                    Console.WriteLine($"Здоровье: {gameManager.Instance.player.health} | Время: {gameManager.Instance.timer} | Уровень: {gameManager.Instance.level}\nПеремещение:\nw - Шаг вверх\nd - Шаг вправо\ns - Шаг вниз\na - Шаг влево\n0 - Выйти");
                }
                string input = Console.ReadLine();
                int key = 0;
                if (input == "0") break;
                switch (input)
                {
                    case "w" or "a" or "s" or "d" when !gameManager.Instance.GameOver:
                        if (input == "w") key = 1; 
                        else if (input == "d") key = 2;
                        else if (input == "s") key = 3;
                        else if (input == "a") key = 4;
                        Action action = new MOVE(gameManager.Instance.player, key);
                        message = action.execute();
                        break;
                    default:
                        message = Action.fail("Моя твоя не понимать, еретик!");
                        break;
                }
            }
        }
    }
}
