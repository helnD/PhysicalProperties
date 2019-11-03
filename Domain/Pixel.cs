using System;

namespace System
{
    public class Pixel : ICloneable
    {
        public int Value { get; } 
        public Coordinate Position { get; }

        public Pixel(int value, int x, int y)
        {
            Value = value;
            Position = new Coordinate(x, y);
        }

        public Pixel Inverted() =>
            new Pixel(-Value, Position.X, Position.Y);
        
        public Pixel WithNewValue(int value) =>
            new Pixel(value, Position.X, Position.Y);
        
        public Pixel WithNewPosition(int x, int y) =>
            new Pixel(Value, x, y);

        public object Clone() =>
            new Pixel(Value, Position.X, Position.Y);
    }
}