using System.Collections.Generic;
using System.Linq;

namespace System
{
    public class PropertyDeterminant
    {
        public PropertySet DeterminePropertySet(Model model) =>
			new PropertySet(Area(model), MassCenter(model), MassCenter2(model),
				Perimeter(model), Perimeter2(model), CircumferenceRatio(model));

		private int Area(Model model)
		{
			int area = 0;
			model.ExtendedPassing((it, x, y) =>
			{
				if (it[x, y].Value > 0)
				{
					area++;
				}
			});

			return area;
		}

		private int Perimeter(Model model)
		{
			var workModel = model.WorkModel();
			int perimeter = 0;

			workModel.Passing((it, x, y) =>
			{
				if (it[x, y].Value < 0)
				{
					if (it[x, y - 1].Value == 0) perimeter++;
					if (it[x, y + 1].Value == 0) perimeter++;
					if (it[x - 1, y].Value == 0) perimeter++;
					if (it[x + 1, y].Value == 0) perimeter++;
				}
			});

			return perimeter;
		}

		private int Perimeter2(Model model)
		{
			int angles = 0;
			var workModel = model.WorkModel();
			
			workModel.Passing((it, x, y) =>
			{
				int sum = 0;

				if (it[x, y].Value > 0) sum++;
				if (it[x + 1, y].Value  > 0) sum++;
				if (it[x, y + 1].Value  > 0) sum++;
				if (it[x + 1, y + 1].Value  > 0) sum++;

				if (sum == 3) angles++;

			});

			int simplePerimeter = Perimeter(model);
			return simplePerimeter - 2 * angles + (int) Math.Sqrt(2) * angles;
		}

		private int CircumferenceRatio(Model model)
		{
			return (int) (Math.Pow(Perimeter2(model), 2) / Area(model));
		}

		private Coordinate MassCenter(Model model)
		{
			List<int> xCoordinate = new List<int>();
			List<int> yCoordinate = new List<int>();
			
			model.ExtendedPassing((it, x, y) =>
			{
				if (it[x, y].Value > 0)
				{
					if (xCoordinate.All(elem => elem != x))
					{
						xCoordinate.Add(x);
					}
					
					if (yCoordinate.All(elem => elem != y))
					{
						yCoordinate.Add(y);
					}
				}
			});
			
			return new Coordinate(xCoordinate.Sum() / xCoordinate.Count, yCoordinate.Sum() / yCoordinate.Count);
		}

		private Coordinate MassCenter2(Model model)
		{
			int xSum = 0;
			int ySum = 0;
			int area = 0;
			
			model.ExtendedPassing((it, x, y) =>
			{
				if (it[x, y].Value > 0)
				{
					area++;
					xSum += x;
					ySum += y;
				}
			});
			
			return new Coordinate(xSum / area, ySum / area);
		}
    }
}