using System;

namespace course_work {
    public abstract class Action {
        public abstract string execute();
        public static string fail(string a) {
            return "ПРОВАЛ: " + a;
        }
        public static string damage(string a) {
            return "УРОН: " + a;
        }
        public static string success(string a) {
            return "УСПЕХ: " + a;
        }
    }

    public class MOVE : Action {
        public IUnit unit;
        public int dir;
        public MOVE(IUnit unit, int dir) {
            this.unit = unit;
            this.dir = dir;
        }
        public override string execute() {
            Grid g = gameManager.Instance.grid;
            IUnit cell = g.getCellDir(unit, dir);
            cellType cellT = Grid.IUnit2cellType(cell);
            

            gameManager.Instance.timer -= 1;
            gameManager.Instance.CheckTimer();
            if (gameManager.Instance.player.health == 0) {
                gameManager.Instance.GameOver = true;
                gameManager.Instance.grid.removeUnit((IUnit)gameManager.Instance.player);
                return fail("Game Over");
            }

            switch (cellT)
            {
                case cellType.Floor:
                    g.moveUnit(unit, g.getUnitIndex(unit) + g.dir2offset(dir));
                    return "";
                case cellType.Wall:
                    return fail("Не могу туда пойти!");
                case cellType.Warrior or cellType.Warrior2:
                    Action action1 = new ATTACK((Mortal)unit, (Mortal)cell, false);
                    string result1 = action1.execute();
                    if(result1.Contains("УРОН")) {
                        g.moveUnit(unit, g.getUnitIndex(unit) + g.dir2offset(dir));
                    }
                    return result1;
                case cellType.Trap:
                    Action action2 = new ATTACK((Mortal)unit, (Mortal)cell, true);
                    string result2 = action2.execute();
                    if(result2.Contains("УРОН") || result2.Contains("УСПЕХ")) {
                        g.moveUnit(unit, g.getUnitIndex(unit) + g.dir2offset(dir));
                    }
                    return result2;
                case cellType.Heal:
                    Action action3 = new HEAL();
                    g.moveUnit(unit, g.getUnitIndex(unit) + g.dir2offset(dir));
                    return action3.execute();
                case cellType.DoorSwitch:
                    ((DoorSwitch)cell).toggle();
                    g.moveUnit(unit, g.getUnitIndex(unit) + g.dir2offset(dir));
                    return success("Двери открываются, идите в них");
                case cellType.Door:
                    if(((Door)cell).isOpen) {
                        gameManager.Instance.w++;
                        gameManager.Instance.level++;
                        byte ph = gameManager.Instance.player.health;
                        gameManager.Instance.grid = new Grid(gameManager.Instance.w, gameManager.Instance.w);
                        gameManager.Instance.timer = 10;
                        gameManager.Instance.player.health = ph;
                        return success("СЛЕДУЮЩИЙ УРОВЕНЬ");
                    } else {
                        return fail("Дверь закрыта");
                    }
                default:
                    return fail("ОшибОчка");
            }
        }
    }
    
    public class ATTACK : Action {
        public Mortal attacker;
        public Mortal reciever;
        public int dir;
        public bool trap;

        public ATTACK(Mortal a, Mortal r, bool t) {
            this.attacker = a;
            this.reciever = r;
            this.trap = t;
        }
        public override string execute() {
            if (!this.trap) {
                attacker.health -= reciever.health;
                attacker.health = (attacker.health >= 200) ? (byte)0 : attacker.health;
                
                if (attacker.health == 0) {
                    gameManager.Instance.GameOver = true;
                    gameManager.Instance.grid.removeUnit((IUnit)attacker);
                    return fail("Game Over");
                } else {
                    gameManager.Instance.grid.removeUnit((IUnit)reciever);
                    return damage($"Игрок потерял {reciever.health} здоровья, но победил врага");
                }
            } else {
                var rand = new Random();
                double c = rand.NextDouble();
                if (c >= .5) {
                    gameManager.Instance.timer -= (int)reciever.health;
                    gameManager.Instance.CheckTimer();
                    if (attacker.health == 0) {
                        gameManager.Instance.GameOver = true;
                        gameManager.Instance.grid.removeUnit((IUnit)attacker);
                        return fail("Game Over");
                    }
                    gameManager.Instance.grid.removeUnit((IUnit)reciever);
                    return damage($"Игрок застрял и потерял {reciever.health} минуты времени");
                } else {
                    gameManager.Instance.grid.removeUnit((IUnit)reciever);
                    return success("Игрок успешно обезвредил ловушку");
                }
            }
        }
    }

    public class HEAL : Action {
        public HEAL() {}
        public override string execute()
        {
            gameManager.Instance.player.health = (byte)20;
            return success("Игрок восстановил всё свое здоровье");
        }
    }
}
 