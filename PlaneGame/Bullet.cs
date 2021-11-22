using System;
using System.Collections.Generic;
using System.Media;
using System.Threading;

using Microsoft.DirectX.DirectSound;


namespace PlaneGame
{
    public class Bullet : System.Windows.Forms.PictureBox
    {
        private static System.Drawing.Image[] BulletArray = {
            Properties.Resources.ft1,
            Properties.Resources.ft2,
            Properties.Resources.tf3,
            Properties.Resources.tf4,
            Properties.Resources.tf5,
        };

        Thread thread;
        int fire;
        public Barrier createBarrier = new Barrier(2);
        public Bullet() { }
        public Bullet(int type,int fire) : base()
        {
            //  Control.CheckForIllegalCrossThreadCalls = false;
            this.Image = BulletArray[type];
            this.fire = fire;
            this.TabStop = false;
            thread = new Thread((ThreadStart)isHint);
            thread.IsBackground = true;
            SoundPlayer simpleSound = new SoundPlayer(Properties.Resources.biu);
            simpleSound.Play();
            thread.Start();
            Program.AddBullet(this);
        }
        private void isHint()
        {
            bool hint = false;
            while (true)
            {
                Thread.Sleep(10);
                int x = Location.X + Width / 2;
                int y = Location.Y;
                // Console.WriteLine("miss Y" + y);
                List<Enemy> enemies = Program.enemies;

                    foreach (Enemy enemy in enemies)
                    {
                        //Console.WriteLine("enemy" + Program.enemies.Count);
                        int x0 = enemy.Location.X;
                        int x1 = enemy.Location.X + enemy.Width;
                        int y0 = enemy.Location.Y;
                        int y1 = enemy.Location.Y + enemy.Height;
                        if (x > x0 && x < x1)
                            if (y > y0 && y < y1)
                            {
                                //Console.WriteLine("hint" + enemy.hp);
                                enemy.BeHint(fire);
                                hint = true;
                                break;
                            }
                    }

                    if (y <= 0)
                    {
                        hint = true;
                    }
                    if (hint)
                        break;
                
            }
            GameForm.DisposeBullet(this);
        }
        public new void Dispose()
        {
            Program.RmBullet(this);
            Dispose(true);
            GC.SuppressFinalize(this);
            base.Dispose();
        }
        ~Bullet()
        {
            Program.RmBullet(this);
        }
    }
}
