namespace Assets.Utils
{
    public class Rectangle
    {
        public int X { get; set; }
        public int Y { get; set; }
        public int Height { get; set; }
        public int Width { get; set; }

        public Rectangle()
        {
            
        }

        public Rectangle(int x, int y, int height, int width)
        {
            X = x;
            Y = y;
            Height = height;
            Width = width;
        }
    }
}
