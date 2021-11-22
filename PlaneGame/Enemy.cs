using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Media;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace PlaneGame
{
    public class Enemy:PictureBox
    {
        private static System.Drawing.Image[] EnemyArray = {
            global::PlaneGame.Properties.Resources.emery01,
            global::PlaneGame.Properties.Resources.emery11,
            global::PlaneGame.Properties.Resources.emery02,
            global::PlaneGame.Properties.Resources.emery12,
            global::PlaneGame.Properties.Resources.emery03,
            global::PlaneGame.Properties.Resources.emery13
        };
        private Point[][] BothPointArray = { Type0BronPoint, Type1BornPoint, Type2BornPoint };
        private static Point[] Type0BronPoint =
        {
            new Point(150,50),new Point(160,50),new Point(170,50), new Point(200,50),new Point(240,50),new Point(260,50)

        };
        private static Point[] Type1BornPoint =
        {
              new Point(50,50),new Point(50,125),new Point(50,250), new Point(300,50),new Point(300,125),new Point(300,250)
        };
        private static Point[] Type2BornPoint =
        {
              new Point(50,50),new Point(50,125),new Point(50,250), new Point(300,50),new Point(300,125),new Point(300,250)

        };
        private int type;
        private int rand1;
        private bool isDispose=false;
        private int speed=100;      //speed越小速度越快
        private Point bornPoint;
        private bool isBorn = false;
        private int MAXHP = 100;
        public int hp=100;
        int MAX_X;
        int MAX_Y;
        System.Timers.Timer timer;
        private PictureBox hpPictureBox;
        public Enemy(int num) : base()
        {
            rand1 = new Random(Guid.NewGuid().GetHashCode()).Next(0, 6);
            this.type = num;
            this.Image = EnemyArray[num * 2 + rand1 % 2];  //0 1; 2 3 ;4 5
            this.ForeColor = Color.Transparent;
            this.BackColor = Color.Transparent;
            this.TabStop = false;
            switch (type)
            {
                case 0:MAXHP = hp = 500;break;
                case 1:MAXHP = hp = 200;break;
                case 2:MAXHP = hp = 20;break;
            }
            MAX_X = GameForm.MAX_X - Width;
            MAX_Y = GameForm.MAX_Y - Height;
            bornPoint = BothPointArray[num][rand1];
            switch (num)
            {
                case 0:
                    this.Location = new Point(bornPoint.X, bornPoint.Y - Height);break;
                case 1:
                case 2:
                    if (rand1 < 3)
                        this.Location = new Point(bornPoint.X - Width, bornPoint.Y - Height);
                    else
                        this.Location = new Point(bornPoint.X + Width, bornPoint.Y - Height);
                    break;
            }
            System.Timers.Timer timer = new System.Timers.Timer();
            timer.Interval = 20;
            timer.Elapsed += delegate
            {
                int x = Location.X, y = Location.Y;
                int px0 = GameForm.planeLocation.X, px1 = GameForm.planeLocation.X + GameForm.planeSize.Width;
                int py0 = GameForm.planeLocation.Y, py1 = GameForm.planeLocation.Y + GameForm.planeSize.Height;
                //Console.WriteLine(Math.Abs(y - py1));
                if ((Math.Abs(x - px0) < 30 && Math.Abs(y - py0) < 30) || (Math.Abs(x - px1) < 30 && Math.Abs(y - py1) < 30))
                {
                    GameForm.PlaneBeHint(num * 10 + 10);
                    if (Parent != null)
                        Parent.BeginInvoke((MethodInvoker)delegate ()
                        {
                            Refresh();
                            hpPictureBox.Refresh();
                        });
                    Dispose();
                    SoundPlayer simpleSound = new SoundPlayer(Properties.Resources.crash);
                    simpleSound.Play();
                    Console.WriteLine("hit");
                    timer.Stop();
                } 
            };
            timer.Start();
            /*Thread hintThread = new Thread((ThreadStart)delegate
            {
                  while (true)
                  {
                      Thread.Sleep(20);
                      int x = Location.X, y = Location.Y;
                      int px0 = GameForm.planeLocation.X, px1 = GameForm.planeLocation.X + GameForm.planeSize.Width;
                      int py0 = GameForm.planeLocation.Y, py1 = GameForm.planeLocation.Y + GameForm.planeSize.Height;
                      //Console.WriteLine(Math.Abs(y - py1));
                      if ((Math.Abs(x-px0)<30&&Math.Abs(y-py0)<30)|| (Math.Abs(x - px1) < 30 && Math.Abs(y - py1) < 30))
                      {
                          GameForm.PlaneBeHint(num * 10 + 10);
                          if(Parent!=null)
                            Parent.Refresh();
                          Dispose();
                          Console.WriteLine("hit");
                          break;
                      }
                  }
              });
            hintThread.IsBackground = true;
            hintThread.Start();*/

            Program.AddEmery(this);
        }
     
        public void InitHpPictureBox()
        {
            hpPictureBox = new PictureBox();
            hpPictureBox.SizeMode = PictureBoxSizeMode.StretchImage;
            hpPictureBox.Location = new System.Drawing.Point(Location.X, Location.Y - 5);
            hpPictureBox.Image = global::PlaneGame.Properties.Resources.blood;
            hpPictureBox.Size = new System.Drawing.Size(Width, 5);
            hpPictureBox.TabIndex = 0;
            hpPictureBox.TabStop = false;
            if(Parent!=null&& !isDispose)
            Parent.BeginInvoke((MethodInvoker)delegate ()
            {
                Parent.Controls.Add(hpPictureBox);
            });
        }
        public void BeHint(int fire)
        {
            hp -= fire;
            SoundPlayer simpleSound = new SoundPlayer(Properties.Resources.bit);
            simpleSound.Play();
            if (hp < 0)
            {
                GameForm.DisposeEnemy(this);
                GameForm.AddScore(type * 10 + 10);
            }
            if(!isDispose&&Parent!=null)
            Parent.BeginInvoke((MethodInvoker)delegate ()
            {
                hpPictureBox.Width = (int)(Width * (1.0 * hp / MAXHP));
            });
        }
        public void Born()
        {
            Thread thread=new Thread(new ThreadStart(delegate { Move(Location, bornPoint,20); }));
            thread.IsBackground = true;
            thread.Start();
        }
        private new void Move(Point start, Point end, int speed = 0,bool toDispose=false)
        {
            int dx, dy;
            if (speed == 0)
                speed = this.speed;

            dx = -(start.X - end.X) / speed;
            dy = -(start.Y - end.Y) / speed;
            if (type == 0)
            {
                dx = 0;
                dy = 1;
            }
            int x = Location.X;
            int y = Location.Y;
            /*
            while ((Math.Abs(x - end.X) > 2 && Math.Abs(y - end.Y) > 2)
                || (Math.Abs(x - end.X) == 0 && Math.Abs(y - end.Y) > 2)    //实现横向飞行和竖向飞行
                ||(Math.Abs(x - end.X) > 2 && Math.Abs(y - end.Y) == 0))
            {
                //Console.WriteLine("move,," + x + " " + y);
                Thread.Sleep(10);
                if (isBorn)
                {
                   if (x < 0 || y < 0 || x > 500 || y > 600 || isDispose)
                      break;
                }
                if (!isDispose && Parent != null)
                {
                    Parent.BeginInvoke((MethodInvoker)delegate ()
                    {
                        Location = new Point(x + dx, y + dy);
                        x = Location.X;
                        y = Location.Y;
                        hpPictureBox.Location = new Point(x, y - 5);
                        hpPictureBox.Refresh();
                        this.Refresh();
                    });
                }
            }*/
            System.Timers.Timer timer = new System.Timers.Timer();

            timer.Interval = 5;

            timer.Elapsed += delegate
            {
                if (isBorn)
                {
                    if (x < 0 || y < 0 || x > 500 || y > 600 || isDispose)
                        timer.Stop();
                }
                if (!isDispose && Parent != null)
                {
                    Parent.BeginInvoke((MethodInvoker)delegate ()
                    {
                        Location = new Point(x + dx, y + dy);
                        x = Location.X;
                        y = Location.Y;
                        hpPictureBox.Location = new Point(x, y - 5);
                        //Parent.Refresh();
                        hpPictureBox.Refresh();
                    });
                }
               if (!((Math.Abs(x - end.X) > 2 && Math.Abs(y - end.Y) > 2)
               || (Math.Abs(x - end.X) == 0 && Math.Abs(y - end.Y) > 2) 
               || (Math.Abs(x - end.X) > 2 && Math.Abs(y - end.Y) == 0)))
                    timer.Stop();
            };

            timer.Start();
            if (!isBorn)
                isBorn = true;
            if (toDispose)
                Dispose();
            //Console.WriteLine("movedown");
        }
        public void StratEnmeyAI()
        {
            Thread AIThread = new Thread(new ThreadStart(delegate
            {
                int x = Location.X;
                int y = Location.Y;
                int px=0, py=0, disx=0, disy=0;
                Thread.Sleep(500);
                switch (type)
                {
                    case 0:
                        Thread.Sleep(4000);
                        px = 0;
                        py = 0;
                        disx = x;
                        disy = 600; 
                        break;
                    case 1:
                    case 2:
                        Thread.Sleep(400);
                        if (rand1 < 3)
                        {
                            px = Math.Abs(x - MAX_X / 2)+rand1*10;
                            py = Math.Abs(y - MAX_Y / 2)+rand1*10;
                            disx = 400;
                            disy = py;
                            //
                        }
                        else if (rand1 < 6)
                        {
                            px = Math.Abs(x - MAX_X / 2) - rand1 * 10;
                            py = Math.Abs(y - MAX_Y / 2) - rand1 * 10;
                            disx = 400;
                            disy = py;
                        }
                        break;
                }
                Thread thread;
                if (type != 0)
                {
                    Thread.Sleep(3000);
                    if (!isDispose)
                    {
                        thread = new Thread(new ThreadStart(delegate { Move(Location, new Point(px, py)); }));
                        thread.IsBackground = true;
                        thread.Start();
                    }
                }
                switch (type)
                {
                    case 0: Thread.Sleep(5000);break;
                    case 1:Thread.Sleep(3000);break;
                    case 2: Thread.Sleep(2000);break;
                }
                if (!isDispose)
                {
                    thread = new Thread(new ThreadStart(delegate { Move(Location, new Point(disx, disy), speed, true); }));
                    thread.IsBackground = true;
                    thread.Start();
                }
            }));
            AIThread.IsBackground = true;
            AIThread.Start();
        }
        public new void Dispose()
        {
            isDispose = true;
            Program.RmEmery(this);
            if(Parent!=null)
                Parent.BeginInvoke((MethodInvoker)delegate ()
                {
                    Dispose(true);
                    hpPictureBox.Dispose();
                    base.Dispose();
                });
            GC.SuppressFinalize(this);
        }
        private void TimersTimer_Elapsed(object sender, System.Timers.ElapsedEventArgs e)
        {
            for (int i = 0; i < 10000; i++)
            {
                this.BeginInvoke(new Action(() =>
                {
                   // this.label_output.Text = "当前时间：" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                }), null);
            }
        }

        ~Enemy()
        {
            isDispose = true;
            hpPictureBox.Dispose();
            Program.RmEmery(this);
        }
    }
}
