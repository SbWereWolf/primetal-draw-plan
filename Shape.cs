namespace draw_plan
{
    class Shape
    {
        public Shape(Contour contour)
        {
            Contour = contour;
        }

        public Contour Contour { get; }
    }
}
