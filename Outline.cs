using System.Collections.Generic;

namespace draw_plan
{
    class Outline
    {
        public int Margin { get; } = 10;
        public Shape Body { get; }
        private readonly List<Shape> _apertures = new List<Shape>();

        public string Name { get; }

        public Outline(Shape body, string name)
        {
            this.Body = body;
            this.Name = name;
        }

        public Outline AddShape(Shape item)
        {
            _apertures?.Add(item);
            return this;
        }

        public IEnumerable<Shape> Next()
        {
            if (_apertures != null)
                foreach (var shape in _apertures)
                {
                    yield return shape;
                }
        }

        public Dimensions GetDimensions()
        {
            var body = Body;
            var dimensionX = 0;
            var dimensionY = 0;
            if (body?.Contour != null)
            {
                var pointX = body.Contour.StartX;
                var pointY = body.Contour.StartY;
                var width = new MinMax(pointX);
                var length = new MinMax(pointY);

                // ReSharper disable PossibleNullReferenceException
                foreach (var vector in body.Contour.Next())
                // ReSharper restore PossibleNullReferenceException
                {
                    if (vector != null)
                    {
                        pointX += vector.X;
                        pointY += vector.Y;
                        width.Adopt(pointX);
                        length.Adopt(pointY);
                    }
                }

                dimensionX = width.GetDifference();
                dimensionY = length.GetDifference();
            }

            return new Dimensions(dimensionX, dimensionY);
        }
    }
}
