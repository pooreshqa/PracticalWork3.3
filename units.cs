namespace course_work {
    public interface IUnit
    {
        public byte getID();
    }

    public abstract class Mortal {
        public byte health { get; set; }
    }


    public class Player : Mortal, IUnit {
        public Player () {this.health = 20;}
        public byte getID() => (byte)4;
        public override string ToString() => "@";
    }

    public class Warrior : Mortal, IUnit {
        public Warrior () {this.health = 2;}

        public byte getID() => (byte)2;

        public override string ToString() => "I";
    }

    public class Warrior2 : Mortal, IUnit {
        public Warrior2 () {this.health = 4;}

        public byte getID() => (byte)5;

        public override string ToString() => "V";
    }

    public class Trap : Mortal, IUnit {
        public Trap () {this.health = 2;}
        public byte getID() => (byte)3;

        public override string ToString() => ":";
    }
    public class HealPotion : IUnit {
        public HealPotion() {}
        public byte getID() => (byte)6;
        public override string ToString() => "+";
    }
    public class DoorSwitch : IUnit {
        public delegate void activate();
        public event activate activationEvent;
        public DoorSwitch() {}
        public void toggle() {
            if (activationEvent != null) activationEvent();
        }
        public byte getID() => (byte)7;
        public override string ToString() => "%";
    }
    public class Door : IUnit {
        public string gfx = "H";
        public bool isOpen = false;
        public Door() { if (gameManager.Instance.doorSwitch != null) gameManager.Instance.doorSwitch.activationEvent += onSwitchToggle;}
        public void onSwitchToggle() {
            gameManager.Instance.doorSwitch.activationEvent -= onSwitchToggle;
            this.isOpen = true;
            this.gfx = "_";
        }
        public byte getID() => (byte)8;
        public override string ToString() => this.gfx;
    }
    public class Floor : IUnit {
        public Floor () {}
        public byte getID() => (byte)0;

        public override string ToString() => ".";
    }

    public class Wall : IUnit {
        public Wall () {}
        public byte getID() => (byte)1;

        public override string ToString() => "#";
    }

}