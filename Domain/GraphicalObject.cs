namespace System
{
    public class GraphicalObject
    {
        public GraphicalObject(Model model, Color objectColor, PropertySet properties, Coordinate position)
        {
            ObjectModel = model;
            ObjectColor = objectColor;
            Properties = properties;
            Position = position;
        }

        public Model ObjectModel { get; }
        public Color ObjectColor { get; }
        public PropertySet Properties { get; }
        public Coordinate Position { get; }
    }
}