using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Battleships
{
    class GameHandler
    {
        public bool[,] enemyShips { get; }
        public int[,] shotsFired { get; }
        public int[,] myShips {get;}
        public int playing { get; set; }
        public int player { get; set; }
        public GameHandler(int player)
        {
            this.player = player;
            this.playing = player;
            enemyShips = new bool[10, 10];
            shotsFired = new int[10, 10];
            myShips = new int[10, 10];
        }

        //***setters***
        public void setEnemyShips(bool[,] enemyShips)
        {
            for (int i = 0; i < 10; i++)
            {
                for (int j = 0; j < 10; j++)
                {
                    this.enemyShips[i, j] = enemyShips[i, j];
                }
            }
        }
        public void setShotsFired(int[,] shotsFired)
        {
            for (int i = 0; i < 10; i++)
            {
                for (int j = 0; j < 10; j++)
                {
                    this.shotsFired[i, j] = shotsFired[i, j];
                }
            }
        }
        public void setMyShips(int[,] myShips)
        {
            for (int i = 0; i < 10; i++)
            {
                for (int j = 0; j < 10; j++)
                {
                    this.myShips[i, j] = myShips[i, j];
                }
            }
        }
        public void setplaying(int playing)
        {
            this.playing = playing;
        }

        //***functions***
        public int myTurn(int i, int j)
        {
            if (shotsFired[i, j] != 0)
            {
                return playing;
            }
            playing++;
            if (enemyShips[i, j])
            {
                playing--;
                shotsFired[i, j] = 2;
            }
            else
            {
                shotsFired[i, j] = 1;
            }
            return playing;
        }
        public void enemyTurn(string shot)
        {
            string[] shotsArrayString = shot.Split(' ');
            int numberOfShots = shotsArrayString.Length; //twice the actual number
            int[] shotsArrayInt = new int[shotsArrayString.Length];
            for (int i = 0; i < numberOfShots; i++)
            {
                shotsArrayInt[i] = Convert.ToInt32(shotsArrayString[i]);
            }
            for (int i = 0; i > numberOfShots; i += 2)
            {
                if (myShips[shotsArrayInt[i], shotsArrayInt[i + 1]] == 1)
                {
                    myShips[shotsArrayInt[i], shotsArrayInt[i + 1]] = 2;
                }
            }
        }
        public void myStart(List<int> myShips)
        {
            for (int i = 0; i < 34; i += 2)
            {
                this.myShips[myShips[i+1], myShips[i]] = 1;
            }
        }
        public void enemyStart(string enemyShips)
        {
            string[] enemyShipCoordsString = enemyShips.Split(' ');
            int[] enemyShipCoordsInt = new int[34];
            for (int i = 0; i < 34; i++)
            {
                enemyShipCoordsInt[i] = Convert.ToInt32(enemyShipCoordsString[i]);
            }
            for (int i = 0; i < 34; i += 2)
            {
                this.enemyShips[enemyShipCoordsInt[i], enemyShipCoordsInt[i + 1]] = true;
            }
        }
        public bool won()
        {
            int sunkenShips = 0;
            for (int i = 0; i < 10; i++)
                for (int j = 0; j < 10; j++)
                    if (enemyShips[i, j] && shotsFired[i,j]==2)
                        sunkenShips++;

            if (sunkenShips == 17)
                return true;
            return false;
        }
        public bool lost()
        {
            int sunkenShips = 0;
            for(int i = 0; i < 10; i++)
                for(int j = 0; j < 10; j++)
                    if(myShips[i,j] == 2)
                        sunkenShips++;

            if(sunkenShips == 17)
                return true;
            return false;
        }
        ///<summary>
        ///Returns coordinates of the ships in a string, each one divided by " ".
        ///</summary>
        public string getMyShips() {
            string s = "";
            for (int i = 0; i < 10; i++) {
                for (int j = 0; j < 10; j++) {
                    if (myShips[i, j] == 1) {
                        s += i + " " + j + " ";
                    }
                }
            }
            return s;
        }
    }
}
