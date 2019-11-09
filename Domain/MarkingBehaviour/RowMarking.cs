using System.Collections.Generic;
using System.Linq;

namespace System.MarkingBehaviour
{
    public class RowMarking : IMarkingBehaviour
    {
        public Model Marking(Model model)
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