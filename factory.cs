namespace course_work
{
    public abstract class UnitFactory {
        public IUnit GetUnit(cellType UnitT) => createUnit(UnitT);
        public abstract IUnit createUnit(cellType UnitT);
    }

    public class BaseFactory : UnitFactory {
        public override IUnit createUnit(cellType UnitT) {
            switch (UnitT)
                {
                    case cellType.Floor: 
                        return new Floor();
                    case cellType.Wall:
                        return new Wall();
                    case cellType.Heal:
                        return new HealPotion();
                    case cellType.DoorSwitch:
                        return new DoorSwitch();
                    case cellType.Door:
                        return new Door();
                    default:
                        System.Console.WriteLine("Can't create type " + UnitT);
                        return null;
                }
        }
    }
    public class MortalFactory : UnitFactory {
        public override IUnit createUnit(cellType UnitT) {
            switch (UnitT)
                {
                    case cellType.Warrior: 
                        if (gameManager.Instance.level < 5) {
                            return new Warrior();
                        } else {
                            return new Warrior2();
                        }
                    case cellType.Player:
                        return new Player();
                    case cellType.Trap:
                        return new Trap();
                    default:
                        System.Console.WriteLine("Can't create type " + UnitT);
                        return null;
                }
        }
    }
}