using System.Collections.Generic;
using System.Linq;

namespace System
{
    public class ObjectRecognizer
    {
        public List<GraphicalObject> FindObjects(Model model)
        {
            Dictionary<int, List<Pixel>> recognizedObjects = new Dictionary<int, List<Pixel>>();

            Model markingModel = Marking(model);
            
            markingModel.Passing((it, x, y) =>
            {
                if (it[x, y].Value > 0)
                {
                    if (recognizedObjects.ContainsKey(it[x, y].Value))
                    {
                        recognizedObjects[it[x, y].Value].Add(it[x, y]);
                    }
                    else
                    {
                        recognizedObjects.Add(it[x, y].Value, new List<Pixel> {it[x, y]});
                    }
                }
            });

            List<GraphicalObject> objects = new List<GraphicalObject>();
            objects.AddRange(recognizedObjects.Select(it =>
            {
                Model objModel = new Model(it.Value);
                Color objColor = new Color(it.Key, it.Key, it.Key);
                PropertySet objProperties = new PropertyDeterminant().DeterminePropertySet(objModel);
                Coordinate position = ObjectPosition(it.Value);
                return new GraphicalObject(objModel, objColor, objProperties, position);
            }));

            return objects;
        }
        
        private Coordinate ObjectPosition(List<Pixel> pixels) =>
            new Coordinate(pixels.Min(it => it.Position.X), pixels.Min(it => it.Position.Y));
        
        private Model Marking(Model model)
        {
            List<List<int>> table = new List<List<int>>();
            
            Model workModel = model.WorkModel();

            int index = 1;
            workModel = workModel.Passing((it, x, y) =>
            {
                if (it[x, y].Value < 0)
                {
                    if (it[x, y - 1].Value > 0)
                    {
                        it[x, y] = it[x, y - 1];
                        if (it[x - 1, y].Value > 0 && it[x - 1, y].Value != it[x, y - 1].Value)
                        {
                            table.First(col => col[0] == it[x, y].Value)
                                .Add(it[x - 1, y].Value);
                        }
                    }
                    else if (it[x - 1, y].Value > 0)
                    {
                        it[x, y] = it[x - 1, y];
                    }
                    else
                    {
                        it[x, y] = it[x, y].WithNewValue(index);
                        table.Add(new List<int> {index});
                        index++;
                    }

                }
            });

            foreach (var column in table)
            {
                for (int i = column.Count - 1; i > 0; i--)
                {
                    var j = i;
                    workModel = workModel.Passing((pixels, x, y) =>
                    {
                        if (pixels[x, y].Value == column[j - 1])
                        {
                            pixels[x, y] = pixels[x, y].WithNewValue(column[j]);
                        }
                    });
                }
            }
            
            workModel.PrintModel();
            
            return workModel;
        }
    }
}