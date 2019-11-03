namespace System
{
    public class PropertySet
    {
        public int Area { get; } 
        public Coordinate MassCenter { get; } 
        public Coordinate MassCenter2 { get; } 
        public int Perimeter { get; } 
        public int Perimeter2 { get; } 
        public int CircumferenceRatio { get; } 

        public PropertySet(int area, Coordinate massCenter, Coordinate massCenter2, int perimeter, int perimeter2, int circumferenceRatio)
        {
            Area = area;
            MassCenter = massCenter;
            MassCenter2 = massCenter2;
            Perimeter = perimeter;
            Perimeter2 = perimeter2;
            CircumferenceRatio = circumferenceRatio;
        }
        
    }
}