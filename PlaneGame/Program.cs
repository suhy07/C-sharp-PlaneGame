using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace PlaneGame
{
    static class Program
    {
        /// <summary>
        /// 应用程序的主入口点。
        /// </summary>
        public static int xWidth, yHeight;
        public static List<Bullet> bullets;
        public static List<Enemy> enemies;
        [STAThread]
        static void Main()
        {
            xWidth = SystemInformation.PrimaryMonitorSize.Width;
            yHeight = SystemInformation.PrimaryMonitorSize.Height;
            bullets = new List<Bullet>();
            enemies = new List<Enemy>();
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new GameForm());
        }
        public static void InitForm(Form form)
        {
            form.StartPosition = FormStartPosition.Manual;
            form.Location = new Point(Program.xWidth/2- form.Width/2, Program.yHeight/2 - form.Height/2);
        }
        public static void AddBullet(Bullet bullet)
        {
            bullets.Add(bullet);
        }

        public static void AddEmery(Enemy enemy)
        {
            {
                enemies.Add(enemy);
            }
        }

        public static void RmBullet(Bullet bullet)
        {
            bullets.Remove(bullet);
        }

        public static void RmEmery(Enemy enemy)
        {

            {
                enemies.Remove(enemy);
            }
        }
    }
}
