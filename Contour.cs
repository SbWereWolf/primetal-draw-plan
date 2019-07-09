using System.Collections.Generic;
using System.Drawing;

namespace draw_plan
{
    class Contour
    {
        public int StartX { get; }
        public int StartY { get; }

        private readonly List<Vector> _vectors = new List<Vector>();

        public Contour(int startX, int startY)
        {
            StartX = startX;
            StartY = startY;
        }

        public Contour AddVector(Vector item)
        {
            _vectors?.Add(item);
            return this;
        }

        public IEnumerable<Vector> Next()
        {
            if (_vectors != null)
            {
                foreach (var vector in _vectors)
                {
                    yield return vector;
                }
            }

        }

        public Contour Draw(Graphics canvas, Pen pen, int margin)
        {
            if (canvas != null && pen != null)
            {
                var pointX = this.StartX + margin;
                var pointY = this.StartY + margin;

                // ReSharper disable PossibleNullReferenceException
                foreach (var vector in this.Next())
                // ReSharper restore PossibleNullReferenceException
                {
                    if (vector != null)
                    {
                        var finishX = pointX + vector.X;
                        var finishY = pointY + vector.Y;
                        canvas.DrawLine(pen, pointX, pointY, finishX, finishY);
                        pointX = finishX;
                        pointY = finishY;
                    }
                }
            }

            return this;
        }
    }
}
