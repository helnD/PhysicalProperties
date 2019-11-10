using System.Collections.Generic;
using System.Linq;
using System.MarkingBehaviour;

namespace System
{
    public class ObjectRecognizer
    {
        private int _index = 0;
        private readonly List<Color> _colors = new List<Color>
        {
            new Color(0, 123, 127),
            new Color(222, 49, 99),
            new Color(99, 3, 196),
            new Color(13, 152, 186),
            new Color(204, 6, 5),
            new Color(184, 0, 44),
            new Color(255, 79, 0),
            new Color(255, 146, 24),
            new Color(255, 216, 0),
            new Color(173, 250, 2),
            new Color(102, 255, 0),
            new Color(0, 100, 0),
            new Color(25, 25, 112),
            new Color(123, 104, 238),
            new Color(128, 0, 0),
            new Color(245,187,152),
            new Color(255, 182,87),
            new Color(242,139,48),
            new Color(235,194,175),
            new Color(201,196,175),
            new Color(153,153,80),
            new Color(123,160,91),
            new Color(255,138,177),
            new Color(255,97,97),
            new Color(138,200,255)
        };
        
        public IMarkingBehaviour MarkingBehaviour { get; set; }
            = new RowMarking();
        
        public List<GraphicalObject> FindObjects(Model model)
        {
            Dictionary<int, List<Pixel>> recognizedObjects = new Dictionary<int, List<Pixel>>();

            Model markingModel = MarkingBehaviour.Marking(model);
            
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
                Color objColor = _colors[_index];
                PropertySet objProperties = new PropertyDeterminant().DeterminePropertySet(objModel);
                Coordinate position = ObjectPosition(it.Value);

                _index = _index + 1 == _colors.Count ? 0 : _index + 1;
                        
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
                        var primary = Math.Min(it[x - 1, y].Value, it[x, y - 1].Value);
                        var secondary = Math.Max(it[x - 1, y].Value, it[x, y - 1].Value);
                        it[x, y] = it[x, y].WithNewValue(primary);
                        if (it[x - 1, y].Value > 0 && it[x - 1, y].Value != it[x, y - 1].Value)
                        {
                            table.First(col => col[0] == primary)
                                .Add(secondary);
                        }
                        else
                        {
                            it[x, y] = it[x, y - 1];
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

            for (var i = table.Count - 1; i >= 0 ; i--)
            {
                var column = table[i];
                for (int j = column.Count - 1; j > 0; j--)
                {
                    workModel = workModel.Passing((pixels, x, y) =>
                    {
                        if (pixels[x, y].Value == column[j])
                        {
                            pixels[x, y] = pixels[x, y].WithNewValue(column[j - 1]);
                        }
                    });
                    
                    for (var i2 = 0; i2 < table.Count; i2++)
                    {
                        for (var j2 = 0; j2 < table[i2].Count; j2++)
                        {
                            if (table[i2][j2] == column[j])
                            {
                                table[i2][j2] = column[j - 1];
                            }
                        }
                    }
                }
            }

            return workModel;
        }
    }
}