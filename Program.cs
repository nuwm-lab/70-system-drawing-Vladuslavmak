using System;
using System.Drawing;
using System.Windows.Forms;

namespace GraphDrawer
{
    public class GraphForm : Form
    {
        private Graph graph;

        public GraphForm()
        {
            this.Text = "Графік функції y = (5*tg(x+7))/((x+3)^2)";
            this.BackColor = Color.White;
            this.graph = new Graph(1.2, 6.3, 0.2);

            this.Resize += (s, e) => this.Invalidate(); // При зміні розміру – перемальовуємо
            this.Paint += OnPaint;
        }

        private void OnPaint(object sender, PaintEventArgs e)
        {
            graph.Draw(e.Graphics, this.ClientSize);
        }

        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.Run(new GraphForm());
        }
    }

    public class Graph
    {
        private double xMin, xMax, step;

        public Graph(double xMin, double xMax, double step)
        {
            this.xMin = xMin;
            this.xMax = xMax;
            this.step = step;
        }

        public void Draw(Graphics g, Size clientSize)
        {
            g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;

            float width = clientSize.Width;
            float height = clientSize.Height;

            // Рамка
            g.DrawRectangle(Pens.Gray, 10, 10, width - 20, height - 20);

            // Масив точок
            double[] xValues = GenerateX();
            double[] yValues = new double[xValues.Length];
            double yMin = double.MaxValue, yMax = double.MinValue;

            for (int i = 0; i < xValues.Length; i++)
            {
                yValues[i] = CalculateY(xValues[i]);
                if (yValues[i] < yMin) yMin = yValues[i];
                if (yValues[i] > yMax) yMax = yValues[i];
            }

            Func<double, float> toScreenX = x => (float)((x - xMin) / (xMax - xMin) * (width - 40) + 20);
            Func<double, float> toScreenY = y => (float)(height - 20 - (y - yMin) / (yMax - yMin) * (height - 40));

            // Осі
            g.DrawLine(Pens.Black, toScreenX(xMin), toScreenY(0), toScreenX(xMax), toScreenY(0)); // OX
            g.DrawLine(Pens.Black, toScreenX(0), toScreenY(yMin), toScreenX(0), toScreenY(yMax)); // OY

            // Графік
            Pen pen = new Pen(Color.Blue, 2);
            for (int i = 1; i < xValues.Length; i++)
            {
                float x1 = toScreenX(xValues[i - 1]);
                float y1 = toScreenY(yValues[i - 1]);
                float x2 = toScreenX(xValues[i]);
                float y2 = toScreenY(yValues[i]);

                if (Math.Abs(yValues[i]) < 1e6 && Math.Abs(yValues[i - 1]) < 1e6)
                    g.DrawLine(pen, x1, y1, x2, y2);
            }
        }

        private double CalculateY(double x)
        {
            return (5 * Math.Tan(x + 7)) / Math.Pow((x + 3), 2);
        }

        private double[] GenerateX()
        {
            int count = (int)((xMax - xMin) / step) + 1;
            double[] xValues = new double[count];
            for (int i = 0; i < count; i++)
                xValues[i] = xMin + i * step;
            return xValues;
        }
    }
}

