namespace draw_plan
{
    class Dimensions
    {
        public int Width { get; private set; }
        public int Length { get; private set; }

        public Dimensions(int width, int length)
        {
            Width = width;
            Length = length;
        }

        public Dimensions Extends(int margin)
        {
            Width += margin;
            Length += margin;

            return this;
        }
    }
}
