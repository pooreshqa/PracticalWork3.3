namespace course_work{
    public sealed class gameManager {
        private static gameManager _instance;
        private gameManager() {}
        public static gameManager Instance {
            get {
                if (_instance == null) {
                    _instance = new gameManager();
                    _instance.GameOver = false;
                }
                return _instance;
            }
        }
        public Grid grid {get; set;}
        public Player player {get; set;}
        public int level {get; set;}
        public int timer {get; set;}
        public bool GameOver {get; set;}
        public int w {get; set;}

        public DoorSwitch doorSwitch {get; set;}

        public bool CheckTimer() {
            if (_instance.timer <= 0) {
                _instance.timer = 10;
                _instance.player.health -= (byte)3;
                _instance.player.health = (_instance.player.health >= 200) ? (byte)0 : _instance.player.health;
                return true;
            } else return false;
        }

        public BaseFactory BaseUnitFactory {get; set;}
        public MortalFactory MortalUnitFactory {get; set;}
    }
}
