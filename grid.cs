using System.Collections.Generic;
using System;

namespace course_work {
    public enum cellType {
        Floor = 0,
        Wall = 1,
        Player = 4,
        Warrior = 2,
        Warrior2 = 5,
        Trap = 3,
        Heal = 6,
        DoorSwitch = 7,
        Door = 8
    }

    public class Grid {
        private List<IUnit> board {get; set;}
        public int width {get; set;}
        public int height {get; set;}
        public Grid(Grid copy) {
            this.width = copy.width;
            this.height = copy.height;
            this.board = new List<IUnit>(copy.board);
        }

        public Grid(int width, int height) {
            this.board = new List<IUnit>(width*height);
            this.width = width;
            this.height = height;
            int len = width * height;
            var rand = new Random();

            byte[] map = new byte[len];
            for (int i = 1; i < this.height-1; i++) {
                for (int j = 1; j < this.width-1; j++){
                    double c = rand.NextDouble();
                    if (c >= .7) {
                        map[Grid.oneDIndex(j,i,width)] = 2;
                    } else if (c >= .5) {
                        map[Grid.oneDIndex(j,i,width)] = 3;
                    } else if (c <= .05) {
                        map[Grid.oneDIndex(j,i,width)] = 6;
                    }
                }
            }
            map[Grid.oneDIndex(1,1, width)] = 4;
            map[Grid.oneDIndex(width-2,height-2, width)] = 7;

            for (int i = 0; i < width; i++) {
                map[i] = 1;
            }
            for (int i = len-width; i < len; i++) {
                map[i] = 1;
            }
            for (int i = 1; i < height-1; i++) {
                if (rand.NextDouble() >= .6) {
                    map[Grid.oneDIndex(0, i, width)] = 8;
                    map[Grid.oneDIndex(width-1, i, width)] = 8;
                } else {
                    map[Grid.oneDIndex(0, i, width)] = 1;
                    map[Grid.oneDIndex(width-1, i, width)] = 1;
                }
            }

            DoorSwitch ds = new DoorSwitch();
            gameManager.Instance.doorSwitch = ds;

            foreach (var item in map)
            {   
                IUnit newUnit;
                switch (item) 
                {
                    case 0 or 1 or 6 or 8:
                        newUnit = gameManager.Instance.BaseUnitFactory.GetUnit((cellType)item);
                        break;
                    case 2 or 4 or 3:
                        newUnit = gameManager.Instance.MortalUnitFactory.GetUnit((cellType)item);
                        if (newUnit as Player != null) {
                            gameManager.Instance.player = (Player)newUnit;
                        }
                        break;
                    default:
                        newUnit = null;
                        break;
                }
                this.board.Add(newUnit);
            }
            this.board[len - width - 2] = ds;
        }
        public void draw() {
            for (int i = 0; i < this.height; i++) {
                for (int j = 0; j < this.width; j++){
                    System.Console.Write($"{this.board[Grid.oneDIndex(j, i, this.width)]} ");
                }
                System.Console.WriteLine();
            }
        }
        public IUnit getCellDir(IUnit unit, int dir) {
            int index = this.getUnitIndex(unit);
            IUnit r;
            switch (dir)
            {
                case 1:
                    r = this.board[index-this.width];
                    break;
                case 2:
                    r = this.board[index+1];
                    break;
                case 3:
                    r = this.board[index+this.width];
                    break;
                case 4:
                    r = this.board[index-1];
                    break;
                default:
                    r = new Wall();
                    break;
            };
            return r;
        }
        public void moveUnit(IUnit unit, int i) {
            int index = this.getUnitIndex(unit);
            this.board[index] = new Floor();
            this.board[i] = unit;
        }
        public void addUnit(IUnit unit, int i) => this.board[i] = unit;
        public void removeUnit(IUnit unit) {
            int i = this.board.IndexOf(unit);
            if (i != -1) {
                this.board[i] = new Floor();
            }
        }
        public int dir2offset(int dir) {
            int offset = 0;
            switch (dir)
            { 
                case 1:
                    offset -= this.width;
                    break;
                case 2:
                    offset += 1;
                    break;
                case 3:
                    offset += this.width;
                    break;
                case 4:
                    offset -= 1;
                    break;
                default:
                    break;
            };
            return offset;
        }
        public static int oneDIndex(int x, int y, int w) => y * w + x;
        public static cellType IUnit2cellType(IUnit unit) => (cellType)unit.getID();
        public int getUnitIndex(IUnit unit) => this.board.IndexOf(unit);
        public IEnumerable<IUnit> Units
        {
            get {
                for (int i = 0; i < this.board.Count; i++) yield return this.board[i];
            }
        }
        private IEnumerator<IUnit> GetEnumerator()
        {
            for (int i = 0; i < this.board.Count; i++) yield return this.board[i];
        }
    }

}