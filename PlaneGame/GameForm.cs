using Microsoft.DirectX;
using Microsoft.DirectX.DirectSound;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Media;
using System.Runtime.InteropServices;
using System.Threading;
using System.Windows.Forms;

namespace PlaneGame
{
    public partial class GameForm : Form
    {
        static Panel panel;
        public static int score = 0;
        public static int MAX_X = 0;
        public static int MAX_Y = 0;
        private bool enemyCreated = false;
        bool isStrat = false;
        bool isHide1 = false;
        int select = 0;
        int[] planex0, planey0,planex1,planey1;
        int[][] ptype;
        List<PictureBox> pictureBoxes;
        Point tmpLocation;
        int Cx, Cy;
        int bulletSpeed = 5;
        int planeSpeed = 10;
        int planefire;
        int planegre;
        int planelen;
        static int planehp;
        static int planeMAXhp;
        int maxBulletNum = 5;
        int buttleNum = 0;
        int type0Num = 0;
        int type1Num = 0;
        int type2Num = 0;
        int type0MAX = 1;
        int type1MAX = 2;
        int type2MAX = 5;
        int x, y;
        static PictureBox hppb;
        static Label hplb,sclb;
        public GameForm()
        {
            InitializeComponent();
            Program.InitForm(this);
            SoundPlayer simpleSound = new SoundPlayer(Properties.Resources.background1);
            simpleSound.PlayLooping();  
            panel = panel1;
            hppb = pb_Blood;
            hplb = lb_hp;
            sclb = lb_Score;
            lb_Score.Text = "0";
            MAX_X = panel1.Width-pB_plane.Width;
            MAX_Y = panel1.Height-pB_plane.Height;
            Cx = panel1.Width / 2 - 50;
            Cy = panel1.Height / 2 - 150;
            planex0 = new int[5];
            planey0 = new int[5];
            planex1 = new int[5];
            planey1 = new int[5];
            bw = pb_Blood.Width;
            int count = 0;
            ptype=new int[5][] { new int[]{ 2, 3, 1, 2, 2 }, new int[] { 3, 3, 1, 1, 2, 2 },
                new int[]{ 1, 2, 2, 3, 3, 1 }, new int[]{ 2, 3, 1, 3, 1, 2 }
                , new int[]{ 2, 2, 3, 2, 1, 2 } };
            pictureBoxes = new List<PictureBox> { pb_plane1, pb_plane2, pb_plane3, pb_plane4, pb_plane5 };
            foreach (PictureBox picture in pictureBoxes)
            {
                planex0[count] = picture.Location.X;
                planex1[count] = planex0[count] + picture.Width;
                planey0[count] = picture.Location.Y;
                planey1[count] = planey0[count++] + picture.Height;
            }
            Refresh();
            //SecondaryBuffer buf = new SecondaryBuffer(Properties.Resources.background1, dv);
            //buf.Play(0, BufferPlayFlags.Looping);
            //Control.CheckForIllegalCrossThreadCalls = false;
            //SetStyle(ControlStyles.UserPaint, true);
            //SetStyle(ControlStyles.AllPaintingInWmPaint, true); // 禁止擦除背景.
            //SetStyle(ControlStyles.DoubleBuffer, true); // 双缓冲
            //SetStyle(ControlStyles.OptimizedDoubleBuffer | ControlStyles.AllPaintingInWmPaint, true);
        }
        private void InitPlane()
        {
            System.Drawing.Image[] images = {
            Properties.Resources.plane1,
            Properties.Resources.plane2,
            Properties.Resources.plane3,
            Properties.Resources.plane4,
            Properties.Resources.plane5};
            pictureBox1.Hide();
            pictureBox2.Hide();
            pictureBox3.Hide();
            pictureBox4.Hide();
            pictureBox5.Hide();
            lv1.Hide();
            lv2.Hide();
            lv3.Hide();
            lv4.Hide();
            lv5.Hide();
            pb_query.Hide();
            pb_canel.Hide();
            tmpPic.Hide();
            pB_plane.Show();
            pb_Blood.Show();
            label1.Show();
            lb_hp.Show();
            lb_Score.Show();
            pB_plane.Image = images[select - 1];
            planeSize = pB_plane.Size;
            switch (planeSpeed)
            {
                case 0:planeSpeed = 5;break;
                case 1:planeSpeed = 10;break;
                case 2:planeSpeed = 15;break;
            }
            switch (planefire)
            {
                case 0: planefire = 5; break;
                case 1: planefire = 10; break;
                case 2: planefire = 15; break;
            }
            switch (planelen)
            {
                case 0: planelen = 1; break;
                case 1: planelen = 2; break;
                case 2: planelen = 3; break;
            }
            switch (planehp)
            {
                case 0: planehp = planeMAXhp = 100; break;
                case 1: planehp = planeMAXhp = 300; break;
                case 2: planehp = planeMAXhp = 500; break;
            }
            isStrat = true;
        }
        private void InitEnemyCreate()
        {
            enemyCreated = true;
            while (true)
            {
                Thread.Sleep(3200);

                if (type2Num < type2MAX)
                {
                    Enemy enemy = new Enemy(2);
                    panel.BeginInvoke((MethodInvoker)delegate ()
                    {
                        panel1.Controls.Add(enemy);
                        enemy.SizeMode = PictureBoxSizeMode.AutoSize;
                        enemy.InitHpPictureBox();
                    });
                
                    enemy.Born();
                    enemy.StratEnmeyAI();
      
                    type2Num++;
                }

                if (type2Num > 2 && type1Num < type1MAX)
                {
                    Enemy enemy = new Enemy(1);
                    panel.BeginInvoke((MethodInvoker)delegate ()
                    {

                        panel1.Controls.Add(enemy);
                        enemy.SizeMode = PictureBoxSizeMode.AutoSize;
                        enemy.InitHpPictureBox();
                    });
       
                    enemy.Born();
                    enemy.StratEnmeyAI();
    
                    type1Num++;
                }

                if (type1Num > 1 && type0Num < type0MAX)
                {
                    Enemy enemy = new Enemy(0);
                    panel.BeginInvoke((MethodInvoker)delegate ()
                    {
                        panel1.Controls.Add(enemy);
                        enemy.SizeMode = PictureBoxSizeMode.AutoSize;
                        enemy.InitHpPictureBox();
                    });
             
                    enemy.Born();
                    enemy.StratEnmeyAI();
  
                    type0Num++;
                }

                if (type0Num >= type0MAX && type1Num >= type1MAX && type2Num >= type2MAX)
                {
                    Thread.Sleep(5000);
                    type0Num = type1Num = type2Num = 0;
                }
            }
        }
        bool keyw = false, keys = false, keya = false, keyd = false, keyj = false;
        private void pb_plane2_Click(object sender, EventArgs e)
        {
            select = 2;
            tmpLocation = pb_plane2.Location;
            Hide1(pb_plane2);
            pb_plane2.Location = new Point(Cx, Cy);
            pb_plane2.Refresh();
        }

        private void pb_plane3_Click(object sender, EventArgs e)
        {
            select = 3;
            tmpLocation = pb_plane3.Location;
            Hide1(pb_plane3);
            pb_plane3.Location = new Point(Cx, Cy);
            pb_plane3.Refresh();
        }

        private void pb_plane4_Click(object sender, EventArgs e)
        {
            select = 4;
            tmpLocation = pb_plane4.Location;
            Hide1(pb_plane4);
            pb_plane4.Location = new Point(Cx, Cy);
            pb_plane4.Refresh();
        }

        private void pb_plane5_Click(object sender, EventArgs e)
        {
            select = 5;
            tmpLocation = pb_plane5.Location;
            Hide1(pb_plane5);
            pb_plane5.Location = new Point(Cx, Cy);
            pb_plane5.Refresh();
        }

        private void pb_plane1_Click(object sender, EventArgs e)
        {
            select = 1;
            tmpLocation = pb_plane1.Location;
            Hide1(pb_plane1);
            pb_plane1.Location = new Point(Cx, Cy);
            pb_plane1.Refresh();
        }

        private void pb_query_Click(object sender, EventArgs e)
        {
            isStrat = true;
            InitPlane();
        }

        private void Hide1(PictureBox pictureBox)
        {
            isHide1 = true;
            tmpPic = pictureBox;
            tmpLocation = pictureBox.Location;
            foreach(PictureBox pictureBox1 in pictureBoxes)
            {
                if (pictureBox != pictureBox1)
                    pictureBox1.Hide();
            }
            pb_label1.Hide();
            pb_label2.Hide();
            pb_lable3.Hide();
            pictureBox1.Show();
            pictureBox2.Show();
            pictureBox3.Show();
            pictureBox4.Show();
            pictureBox5.Show();
            lv1.Show();
            lv2.Show();
            lv3.Show();
            lv4.Show();
            lv5.Show();
            Showlv(select-1);
            pb_query.Show();
            pb_canel.Show();
            Refresh();
        }
        PictureBox tmpPic;
        private void Showlv(int select)
        {
            System.Drawing.Image[] images = {
            Properties.Resources.C,
            Properties.Resources.B,
            Properties.Resources.A };
            lv1.Image = images[planeSpeed=ptype[select][0] - 1];
            lv2.Image = images[planefire=ptype[select][1] - 1];
            lv3.Image = images[planelen=ptype[select][2] - 1];
            lv4.Image = images[planehp=ptype[select][3] - 1];
            lv5.Image = images[planegre=ptype[select][4] - 1];
        }
        private void pb_canel_Click(object sender, EventArgs e)
        {
            Show1();
        }

        private void Show1()
        {
            isHide1 = false;
            tmpPic.Location = tmpLocation;
            foreach (PictureBox pictureBox1 in pictureBoxes)
            {
                pictureBox1.Show();
            }
            pb_label1.Show();
            pb_label2.Show();
            pb_lable3.Show();
            pictureBox1.Hide();
            pictureBox2.Hide();
            pictureBox3.Hide();
            pictureBox4.Hide();
            pictureBox5.Hide();
            lv1.Hide();
            lv2.Hide();
            lv3.Hide();
            lv4.Hide();
            lv5.Hide();
            pb_query.Hide();
            pb_canel.Hide();
        }

        private void panel1_MouseMove(object sender, MouseEventArgs e)
        {
            if (isStrat)
                return;
            int x = e.X, y = e.Y;
            if (!isHide1)
            {
                for (int i = 0; i < pictureBoxes.Count; i++)
                {
                    int bx = Math.Abs(x - planex0[i]), by = Math.Abs(y - planey0[i]);

                    if (bx <= 120 && by <= 120)
                    {
                        Spark(pictureBoxes[i]);
                        Spark(pb_label2);
                    }
                }
                Spark(pb_label1);
            }
            else
            {
                int b1x = Math.Abs(x - pb_query.Location.X), b1y = Math.Abs(y - pb_query.Location.Y);
                int b2x = Math.Abs(x - pb_canel.Location.X), b2y = Math.Abs(y - pb_canel.Location.Y);
                if (b1x <= 120 && b1y <= 120)
                {
                    Spark(pb_query);
                }
                if (b2x <= 120 && b2y <= 120)
                {
                    Spark(pb_canel);
                }
            }
        }

        private void GameForm_KeyDown(object sender, KeyEventArgs e)
        {
            if (!isStrat)
                return;
            x = pB_plane.Location.X;
            y = pB_plane.Location.Y;
           
            char key = (char)e.KeyValue;
            switch (key) 
            {
                case 'W':
                    if(!keyw)keyw = true;break ;
                case 'S':
                    if(!keys)keys = true;break;
                case 'A':
                    if(!keya)keya = true;break;
                case 'D':
                    if(!keyd)keyd = true;break;
                case 'J':
                    if (!keyj) keyj = true; break;

            }
            //s Console.WriteLine("Put" + key);
            if (keyw || keys || keya || keyd || keyj) 
            {
                panel1.BeginInvoke((MethodInvoker)delegate ()
                {
                    PlaneMove();

                });
            }
            if (!enemyCreated)
            {
                Thread thread = new Thread((ThreadStart)InitEnemyCreate);
                thread.IsBackground = true;
                thread.Start();
            }
        }
        private void GameForm_KeyUp(object sender, KeyEventArgs e)
        {
            if (!isStrat)
                return;
            switch (e.KeyValue)
            {
                case 'W': keyw = false; break;
                case 'S': keys = false; break;
                case 'A': keya = false; break;
                case 'D': keyd = false; break;
                case 'J': keyj = false; break;
            }
           // Console.WriteLine("UP" + (char)e.KeyValue);
        }
        private void Spark(PictureBox pictureBox)
        {
            panel1.BeginInvoke((MethodInvoker)delegate ()
            {
                pictureBox.Visible = false;
                Thread.Sleep(50);
                pictureBox.Visible = true;
            });

        }
        private void PlaneMove()
        {
            if (keyw)
            {
                pB_plane.Location = new Point(x, y - planeSpeed < 0 ? 0 : y - planeSpeed);
                x = pB_plane.Location.X;
                y = pB_plane.Location.Y;
                pB_plane.Refresh();
            }
            if (keys)
            {
                pB_plane.Location = new Point(x, y + planeSpeed > MAX_Y ? MAX_Y : y + planeSpeed);
                x = pB_plane.Location.X;
                y = pB_plane.Location.Y;
                pB_plane.Refresh();
            }
            if (keyd)
            {
                pB_plane.Location = new Point(x + planeSpeed > MAX_X ? MAX_X : x + planeSpeed, y);
                x = pB_plane.Location.X;
                y = pB_plane.Location.Y;
                pB_plane.Refresh();
            }
            if (keya)
            {
                pB_plane.Location = new Point(x - planeSpeed < 0 ? 0 : x - planeSpeed, y);
                x = pB_plane.Location.X;
                y = pB_plane.Location.Y;
                pB_plane.Refresh();
            }
            if (keyj&&buttleNum<=maxBulletNum)
            {
                switch (planelen)
                {
                    case 1: Thread thread1 = new Thread((ThreadStart)BulletTread1);
                        thread1.IsBackground = true;
                        thread1.Start();
                        //BulletTread1() ;
                        break;
                    case 2: Thread thread2 = new Thread((ThreadStart)BulletTread2);
                        thread2.IsBackground = true;
                        thread2.Start();
                       // BulletTread2(); 
                        break;
                    case 3:
                        Thread thread4 = new Thread((ThreadStart)BulletTread3);
                        thread4.IsBackground = true;
                        thread4.Start();
                        //BulletTread3(); 
                        break;
                }
                buttleNum++;
            }
            planeLocation = pB_plane.Location;
            pB_plane.Refresh();
            //Console.WriteLine("moveStop");
        }
        public static Point planeLocation;
        public static Size planeSize;
        private void BulletTread1()
        {
            Bullet bullet = new Bullet(select-1,planefire);
            Point[] shootLocation = { new Point(pB_plane.Location.X + pB_plane.Width / 2, pB_plane.Location.Y - 20) };
            bullet.Location = shootLocation[0];
            panel1.BeginInvoke((MethodInvoker)delegate ()
            {
                if (bullet != null && !bullet.IsDisposed) 
                panel1.Controls.Add(bullet);
            
            });
            System.Timers.Timer timer = new System.Timers.Timer();
            timer.Interval = 20;
            timer.Elapsed += delegate
            {
                int y = bullet.Location.Y;
                int x = bullet.Location.X;
                Thread.Sleep(24);
                panel1.BeginInvoke((MethodInvoker)delegate ()
                {
                    bullet.SizeMode = PictureBoxSizeMode.AutoSize;
                    bullet.Location = new Point(x, bullet.Location.Y - bulletSpeed < 0 ? 0 : bullet.Location.Y - bulletSpeed);
                    bullet.Refresh();

                });
                if (!(bullet.Location.Y > 0))
                    timer.Stop();
            };

            timer.Start();
            /*
            while (bullet.Location.Y > 0)
            {
                int y = bullet.Location.Y;
                int x = bullet.Location.X;
                Thread.Sleep(24);
                panel1.BeginInvoke((MethodInvoker)delegate ()
                {
                    bullet.SizeMode = PictureBoxSizeMode.AutoSize;
                    bullet.Location = new Point(x, bullet.Location.Y - bulletSpeed < 0 ? 0 : bullet.Location.Y - bulletSpeed);
                    bullet.Refresh();
              
                });
            }*/
            if (buttleNum >= maxBulletNum)
            {
                Thread.Sleep(1000);
                buttleNum = 0;
            }
        }
        private void BulletTread2()
        {
            Bullet bullet1 = new Bullet(select - 1, planefire);
            Bullet bullet2 = new Bullet(select - 1, planefire);
            Point[] shootLocation = { new Point(pB_plane.Location.X + pB_plane.Width / 2-10, pB_plane.Location.Y - 20), 
                new Point(pB_plane.Location.X + pB_plane.Width / 2 + 10, pB_plane.Location.Y - 20) };
            bullet1.Location = shootLocation[0];
            bullet2.Location = shootLocation[1];
            panel1.BeginInvoke((MethodInvoker)delegate ()
            {
                if (bullet1 != null && !bullet1.IsDisposed) 
                    panel1.Controls.Add(bullet1);
                if (bullet2 != null && !bullet2.IsDisposed)
                    panel1.Controls.Add(bullet2);

            });
            System.Timers.Timer timer = new System.Timers.Timer();
            timer.Interval = 20;
            timer.Elapsed += delegate
            {
                int y = bullet1.Location.Y;
                int x = bullet1.Location.X;
                int x1 = bullet2.Location.X;
                Thread.Sleep(24);
                panel1.BeginInvoke((MethodInvoker)delegate ()
                {
                    bullet1.SizeMode = PictureBoxSizeMode.AutoSize;
                    bullet1.Location = new Point(x, bullet1.Location.Y - bulletSpeed < 0 ? 0 : bullet1.Location.Y - bulletSpeed);
                    bullet1.Refresh();
                    bullet2.SizeMode = PictureBoxSizeMode.AutoSize;
                    bullet2.Location = new Point(x1, bullet1.Location.Y - bulletSpeed < 0 ? 0 : bullet1.Location.Y - bulletSpeed);
                    bullet2.Refresh();

                });
                if(!(bullet1.Location.Y > 0))
                    timer.Stop();
            };
            timer.Start();
            /*
            while (bullet1.Location.Y > 0)
            {
                int y = bullet1.Location.Y;
                int x = bullet1.Location.X;
                int x1 = bullet2.Location.X;
                Thread.Sleep(24);
                panel1.BeginInvoke((MethodInvoker)delegate ()
                {
                    bullet1.SizeMode = PictureBoxSizeMode.AutoSize;
                    bullet1.Location = new Point(x, bullet1.Location.Y - bulletSpeed < 0 ? 0 : bullet1.Location.Y - bulletSpeed);
                    bullet1.Refresh();
                    bullet2.SizeMode = PictureBoxSizeMode.AutoSize;
                    bullet2.Location = new Point(x1, bullet1.Location.Y - bulletSpeed < 0 ? 0 : bullet1.Location.Y - bulletSpeed);
                    bullet2.Refresh();

                });
            }*/
            if (buttleNum >= maxBulletNum)
            {
                Thread.Sleep(1000);
                buttleNum = 0;
            }
        }
        private void BulletTread3()
        {
            Bullet bullet1 = new Bullet(select - 1, planefire);
            Bullet bullet2 = new Bullet(select - 1, planefire);
            Bullet bullet3 = new Bullet(select - 1, planefire);
            Point[] shootLocation = { new Point(pB_plane.Location.X + pB_plane.Width / 2-5, pB_plane.Location.Y - 20),
                new Point(pB_plane.Location.X + pB_plane.Width / 2 + 5, pB_plane.Location.Y - 20),
                new Point(pB_plane.Location.X + pB_plane.Width / 2, pB_plane.Location.Y - 20) };
            bullet1.Location = shootLocation[0];
            bullet2.Location = shootLocation[1];
            bullet3.Location = shootLocation[2];
            panel1.BeginInvoke((MethodInvoker)delegate ()
            {
                if (bullet1 != null && !bullet1.IsDisposed)
                    panel1.Controls.Add(bullet1);
                if (bullet2 != null && !bullet2.IsDisposed)
                    panel1.Controls.Add(bullet2);
                if (bullet3 != null && !bullet3.IsDisposed)
                    panel1.Controls.Add(bullet3);

            });
            System.Timers.Timer timer = new System.Timers.Timer();
            timer.Interval = 20;
            timer.Elapsed += delegate
            {
                int y = bullet1.Location.Y;
                int x = bullet1.Location.X;
                int x1 = bullet2.Location.X;
                int x2 = bullet3.Location.X;
                Thread.Sleep(24);
                panel1.BeginInvoke((MethodInvoker)delegate ()
                {
                    bullet3.SizeMode = PictureBoxSizeMode.AutoSize;
                    bullet3.Location = new Point(x2, bullet1.Location.Y - bulletSpeed < 0 ? 0 : bullet1.Location.Y - bulletSpeed);
                    bullet3.Refresh();
                    bullet1.SizeMode = PictureBoxSizeMode.AutoSize;
                    bullet1.Location = new Point(x + bulletSpeed, bullet1.Location.Y - bulletSpeed < 0 ? 0 : bullet1.Location.Y - bulletSpeed);
                    bullet1.Refresh();
                    bullet2.SizeMode = PictureBoxSizeMode.AutoSize;
                    bullet2.Location = new Point(x1 - bulletSpeed, bullet1.Location.Y - bulletSpeed < 0 ? 0 : bullet1.Location.Y - bulletSpeed);
                    bullet2.Refresh();
                    x = bullet1.Location.X;
                    x1 = bullet2.Location.X;
                    x2 = bullet3.Location.X;

                });
                if (!(bullet3.Location.Y > 0))
                    timer.Stop();
            };
            timer.Start();
            /*
            while (bullet3.Location.Y > 0)
            {
                int y = bullet1.Location.Y;
                int x = bullet1.Location.X;
                int x1 = bullet2.Location.X;
                int x2 = bullet3.Location.X;
                Thread.Sleep(24);
                panel1.BeginInvoke((MethodInvoker)delegate ()
                {
                    bullet3.SizeMode = PictureBoxSizeMode.AutoSize;
                    bullet3.Location = new Point(x2, bullet1.Location.Y - bulletSpeed < 0 ? 0 : bullet1.Location.Y - bulletSpeed);
                    bullet3.Refresh();
                    bullet1.SizeMode = PictureBoxSizeMode.AutoSize;
                    bullet1.Location = new Point(x+bulletSpeed, bullet1.Location.Y - bulletSpeed < 0 ? 0 : bullet1.Location.Y - bulletSpeed);
                    bullet1.Refresh();
                    bullet2.SizeMode = PictureBoxSizeMode.AutoSize;
                    bullet2.Location = new Point(x1-bulletSpeed, bullet1.Location.Y - bulletSpeed < 0 ? 0 : bullet1.Location.Y - bulletSpeed);
                    bullet2.Refresh();
                    x = bullet1.Location.X;
                    x1 = bullet2.Location.X;
                    x2 = bullet3.Location.X;

                });
            }*/
            if (buttleNum >= maxBulletNum)
            {
                Thread.Sleep(1000);
                buttleNum = 0;
            }
        }
        public static void DisposeBullet(Bullet bullet)
        {
            panel.BeginInvoke((MethodInvoker)delegate ()
            {
                bullet.Dispose();
                panel.Refresh();
            });
        }
        public static void DisposeEnemy(Enemy enemy)
        {
            panel.BeginInvoke((MethodInvoker)delegate ()
            {
                enemy.Dispose();
            });
        }
        static int bw;
        bool finish = false;
        public static void PlaneBeHint(int hit)
        {
            planehp -= hit;
            panel.BeginInvoke((MethodInvoker)delegate ()
            {
                panel.Refresh();
                double x =( (1.0 * planehp) / planeMAXhp);
                //Console.WriteLine(x);
                if (x > 0)
                {
                    hppb.Width = (int)(bw * x);
                    hplb.Text = (int)(x*100) + "%";
                }
                
            });
           
        }
        public static void AddScore(int _score)
        {
            score += _score;
            panel.BeginInvoke((MethodInvoker)delegate ()
            {
                sclb.Text = score+"";

            });

        }
    }
}
