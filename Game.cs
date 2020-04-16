using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Net;

namespace Battleships {
    public partial class Game : Form {
        public Game() {
            InitializeComponent();
        }
        int player = 3, selectedI, selectedJ;
        GameHandler game;
        bool selected = false, startgame = false, addships = true, readyToPlay = false;
        string shotSend = "";
        List<int> ships = new List<int>();

        Server pull;
        Client push;
        private void Game_Load(object sender, EventArgs e) {

            pictureBox1.Width = this.ClientRectangle.Width;
            pictureBox1.Height = this.ClientRectangle.Height;
            timer1.Start();
            pictureBox2.Hide();
            pictureBox3.Hide();
            label1.Hide();
            label2.Hide();
            

            Form1 mainMenu = new Form1();
            
            pull = new Server(mainMenu.getIpThis());
            push = new Client(mainMenu.getIpOther());
        }

        private void pictureBox1_Click(object sender, EventArgs e) {
            if (this.PointToClient(new Point(Cursor.Position.X, Cursor.Position.Y)).Y > this.ClientRectangle.Height / 2) {
                player = 2;
            }
            else player = 1;
            gameLayoutLoad();
            game = new GameHandler(player);
        }
        bool red, blue;
        private void pictureBox1_Paint(object sender, PaintEventArgs e)         //player selection
        {
            Brush brushred = Brushes.DarkRed;
            Brush brushblue = Brushes.DarkBlue;
            if (red)
                brushred = Brushes.Red;
            if (blue)
                brushblue = Brushes.Blue;
            e.Graphics.FillRectangle(brushblue, 0, 0, this.ClientRectangle.Width, this.ClientRectangle.Height / 2);
            e.Graphics.DrawString("player 1", this.Font, Brushes.White, 10, 10);
            e.Graphics.FillRectangle(brushred, 0, this.ClientRectangle.Height / 2, this.ClientRectangle.Width, this.ClientRectangle.Height / 2);
            e.Graphics.DrawString("player 2", this.Font, Brushes.White, 10, this.ClientRectangle.Height / 2 + 10);
        }

        private void timer1_Tick(object sender, EventArgs e) {
            if (this.PointToClient(new Point(Cursor.Position.X, Cursor.Position.Y)).Y < this.ClientRectangle.Height / 2) {
                blue = true;
                red = false;
            }
            if (this.PointToClient(new Point(Cursor.Position.X, Cursor.Position.Y)).Y > this.ClientRectangle.Height / 2) {
                blue = false;
                red = true;
            }
            pictureBox1.Refresh();

            int cursorX = this.PointToClient(new Point(Cursor.Position.X, Cursor.Position.Y)).X - 5 - pictureBox2.Location.X - 60;
            int cursorY = this.PointToClient(new Point(Cursor.Position.X, Cursor.Position.Y)).Y - 5 - pictureBox2.Location.Y - 60;
            if (cursorX < 0 || cursorY < 0 || cursorX > 655 || cursorY > 655) {
                label2.Hide();
            }
            else {
                if (player == 1 || player == 2)
                    label2.Show();
            }

            int indexI = 0, indexJ = 0;
            getIndex(cursorX, cursorY, 5, 60, ref indexJ, ref indexI);
            label2.Text = getStringFromIndex(indexI, indexJ);
            updateLocations();

            if (ships.Count == 34) {
                game.myStart(ships);
                ships.Clear();
                gameStart();
            }
        }

        private void timer2_Tick(object sender, EventArgs e) {
            if(game.playing == 2) {
                game.enemyTurn(pull.receive());
                if (game.lost()) {
                    label1.Text = "Better luck next time, captain!";
                    updateLocations();
                    MessageBox.Show("You lost!");
                    this.Close();
                }
                game.playing = 1;
                label1.Text = "Your turn!";
                timer2.Stop();
            }
        }

        private void pictureBox2_Paint(object sender, PaintEventArgs e)         //enemy shooting area
        {
            for (int i = 60; i <= 710; i += 65) {
                e.Graphics.DrawString(Convert.ToInt32((i - 60) / 65 + 1).ToString(), this.Font, Brushes.LightGray, i + 29, 20);
                char letter;
                switch ((i - 60) / 65) {
                    case 0:
                        letter = 'A';
                        break;
                    case 1:
                        letter = 'B';
                        break;
                    case 2:
                        letter = 'C';
                        break;
                    case 3:
                        letter = 'D';
                        break;
                    case 4:
                        letter = 'E';
                        break;
                    case 5:
                        letter = 'F';
                        break;
                    case 6:
                        letter = 'G';
                        break;
                    case 7:
                        letter = 'H';
                        break;
                    case 8:
                        letter = 'I';
                        break;
                    case 9:
                        letter = 'J';
                        break;
                    default:
                        letter = 'x';
                        break;
                }
                e.Graphics.DrawString(letter.ToString(), this.Font, Brushes.LightGray, 20, i + 29);
                e.Graphics.FillRectangle(Brushes.LightGray, 60, i, 655, 5);
                e.Graphics.FillRectangle(Brushes.LightGray, i, 60, 5, 655);
            }

            if (addships)
                for (int i = 0; i < ships.Count; i += 2) {
                    e.Graphics.FillRectangle(Brushes.Blue, 65 + 65 * ships[i + 1], 65 + 65 * ships[i], 60, 60);
                }
        }

        private void pictureBox3_Paint(object sender, PaintEventArgs e)         //enemy ships
        {
            for (int i = 60; i <= 453; i += 39) {
                e.Graphics.DrawString(Convert.ToInt32((i - 60) / 39 + 1).ToString(), this.Font, Brushes.LightGray, i + 15, 20);
                char letter;
                switch ((i - 60) / 39) {
                    case 0:
                        letter = 'A';
                        break;
                    case 1:
                        letter = 'B';
                        break;
                    case 2:
                        letter = 'C';
                        break;
                    case 3:
                        letter = 'D';
                        break;
                    case 4:
                        letter = 'E';
                        break;
                    case 5:
                        letter = 'F';
                        break;
                    case 6:
                        letter = 'G';
                        break;
                    case 7:
                        letter = 'H';
                        break;
                    case 8:
                        letter = 'I';
                        break;
                    case 9:
                        letter = 'J';
                        break;
                    default:
                        letter = 'x';
                        break;
                }
                e.Graphics.DrawString(letter.ToString(), this.Font, Brushes.LightGray, 20, i + 15);
                e.Graphics.FillRectangle(Brushes.LightGray, 60, i, 453, 3);
                e.Graphics.FillRectangle(Brushes.LightGray, i, 60, 3, 453);
            }

            if (addships)
                for (int i = 0; i < ships.Count; i += 2) {
                    e.Graphics.FillRectangle(Brushes.Blue, 63 + 39 * ships[i + 1], 63 + 39 * ships[i], 36, 36);
                }
            else {
                for (int i = 0; i < 10; i++) {
                    for (int j = 0; j < 10; j++) {
                        if (game.myShips[i, j] == 1)
                            e.Graphics.FillRectangle(Brushes.Blue, 63 + 39 * i, 63 + 39 * j, 36, 36);
                        else if(game.myShips[i,j] == 2) {
                            e.Graphics.FillRectangle(Brushes.Red, 63 + 39 * i, 63 + 39 * j, 36, 36);
                        }
                    }
                }
            }
        }

        private void label3_Click(object sender, EventArgs e) {
            this.Close();
        }

        private void pictureBox2_Click(object sender, EventArgs e) {
            if (addships) {
                int cursorX = this.PointToClient(new Point(Cursor.Position.X, Cursor.Position.Y)).X - 5 - pictureBox2.Location.X - 60;
                int cursorY = this.PointToClient(new Point(Cursor.Position.X, Cursor.Position.Y)).Y - 5 - pictureBox2.Location.Y - 60;
                int i = 0, j = 0;
                getIndex(cursorX, cursorY, 5, 60, ref j, ref i);
                ships.Add(i);
                ships.Add(j);
                pictureBox3.Refresh();
                pictureBox2.Refresh();
            }

            if(ships.Count == 34) {
                addships = false;
            }

            if (game.playing == 1 && readyToPlay) {
                
                readyToPlay = false;
                int cursorX = this.PointToClient(new Point(Cursor.Position.X, Cursor.Position.Y)).X - 5 - pictureBox2.Location.X - 60;
                int cursorY = this.PointToClient(new Point(Cursor.Position.X, Cursor.Position.Y)).Y - 5 - pictureBox2.Location.Y - 60;
                int i = 0, j = 0;
                getIndex(cursorX, cursorY, 5, 60, ref j, ref i);
                game.playing = game.myTurn(i,j);
                shotSend += i + " " + j + " ";
                if (game.playing == 1 && !game.won()) {
                    label1.Text = "Nice shot! One more chance!";
                    readyToPlay = true;
                }
                else if(game.playing == 1 && game.won()) {
                    label1.Text = "AHOY SPONGEBOB ME BOY!";
                    updateLocations();
                    MessageBox.Show("You won!");
                    this.Close();
                }
                else {
                    label1.Text = "You missed! Enemy turn.";
                    push.send(shotSend);
                    shotSend = "";
                }
                updateLocations();
            }
        }

        private void gameLayoutLoad() {
            pictureBox2.Location = new Point(50, 50);
            pictureBox2.Width = 715;
            pictureBox2.Height = 715;

            pictureBox3.Location = new Point(1087, pictureBox2.Location.Y);
            pictureBox3.Width = 453;
            pictureBox3.Height = 453;

            label1.Text = "Welcome!";
            updateLocations();

            pictureBox1.Hide();
            pictureBox2.Show();
            pictureBox3.Show();
            label1.Show();

        }

        private void getIndex(int cursorX, int cursorY, int wallWidth, int cellWidth, ref int x, ref int y) {
            cursorX = cursorX - cursorX % (cellWidth + wallWidth);
            x = cursorX / (cellWidth + wallWidth);

            cursorY = cursorY - cursorY % (cellWidth + wallWidth);
            y = cursorY / (cellWidth + wallWidth);
        }

        private string getStringFromIndex(int I, int J) {
            switch (I) {
                case 0:
                    return "A - " + (J + 1);
                    break;
                case 1:
                    return "B - " + (J + 1);
                    break;
                case 2:
                    return "C - " + (J + 1);
                    break;
                case 3:
                    return "D - " + (J + 1);
                    break;
                case 4:
                    return "E - " + (J + 1);
                    break;
                case 5:
                    return "F - " + (J + 1);
                    break;
                case 6:
                    return "G - " + (J + 1);
                    break;
                case 7:
                    return "H - " + (J + 1);
                    break;
                case 8:
                    return "I - " + (J + 1);
                    break;
                case 9:
                    return "J - " + (J + 1);
                    break;
                default:
                    return "ERROR";
                    break;
            }
        }
        private void gameStart() {
            if (player == 1) {
                push.send(game.getMyShips());
                game.enemyStart(pull.receive());
                label1.Text = "Connected with the enemy captain! Your turn!";
                pictureBox2.Refresh();
                readyToPlay = true;
            }
            else {
                game.enemyStart(pull.receive());
                push.send(game.getMyShips());
                label1.Text = "Connected with the enemy captain! Wait for your turn.";
                pictureBox2.Refresh();
                timer2.Start();
            }
        }
        private void updateLocations() {
            label2.Location = new Point(20 + (pictureBox3.Location.X - (pictureBox2.Location.X + pictureBox2.Width)) / 2 + (pictureBox2.Location.X + pictureBox2.Width) - label2.Width / 2, pictureBox3.Location.Y + pictureBox3.Height / 2);
            label1.Location = new Point(pictureBox2.Location.X + pictureBox2.Width / 2 - label1.Width / 2, ((pictureBox2.Height + pictureBox2.Location.Y) + (this.ClientRectangle.Height - (pictureBox2.Height + pictureBox2.Location.Y)) / 2) - label1.Height / 2);

        }
    }
}
