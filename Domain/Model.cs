using System.Collections.Generic;
using System.Linq;

namespace System
{
    public class Model : ICloneable
    {
        private List<Pixel> _pixels = new List<Pixel>();

        public Model(List<Pixel> pixels)
        {
            Height = pixels.Max(it => it.Position.Y) - pixels.Min(it => it.Position.Y) + 1;
            Width = pixels.Max(it => it.Position.X) - pixels.Min(it => it.Position.X) + 1;

            int StartX(int x)
            {
                return x - pixels.Min(it => it.Position.X);
            }

            int StartY(int y)
            {
                return y - pixels.Min(it => it.Position.Y);
            }

            foreach (var pixel in pixels)
            {
                _pixels.Add(pixel.WithNewPosition(StartX(pixel.Position.X), StartY(pixel.Position.Y)));
            }

            for (int i = 0; i < Height; i++)
            {
                for (int j = 0; j < Width; j++)
                {
                    if (this[j, i] == null)
                    {
                        _pixels.Add(new Pixel(0, j, i));
                    }
                }
            }
        }

        public int Width { get; }
        public int Height { get; }

        public Model WorkModel()
        {

            List<Pixel> init = new List<Pixel>();
            for (var i = 0; i < Height + 2; i++)
            {
                for (var j = 0; j < Width + 2; j++)
                {
                    init.Add(new Pixel(0, j, i));
                }
            }

            Model model = new Model(init);

            for (int i = 0; i < Height; i++)
            {
                for (int j = 0; j < Width; j++)
                {
                    if (this[j, i].Value > 0)
                    {
                        model[j + 1, i + 1] = this[j, i].Inverted();
                    }
                }
            }
            
            return model;
        }
        
        public Model Passing(Action<Model, int, int> action)
        {
            Model model = this.Clone() as Model;
            
            for (var i = 1; i < model.Height - 1; i++)
            {
                for (var j = 1; j < model.Width - 1; j++)
                {
                    action(model, j, i);
                }
            }

            return model;
        }
        
        public Model ExtendedPassing(Action<Model, int, int> action)
        {
            Model model = this.Clone() as Model;
            
            for (var i = 0; i < model.Height; i++)
            {
                for (var j = 0; j < model.Width; j++)
                {
                    action(model, j, i);
                }
            }

            return model;
        }

        public Pixel this[int index1, int index2]
        {
            get => _pixels.Find(it => it.Position.X == index1 && it.Position.Y == index2); 
            set
            {
                if (value == null) throw new ArgumentNullException(nameof(value));
                int index = _pixels.IndexOf(_pixels.Find(it => it.Position.X == index1 && it.Position.Y == index2));
                _pixels[index] = new Pixel(value.Value, index1, index2);
                
            }
        }

        public void PrintModel()
        {
            for (int i = 0; i < Height; i++)
            {
                for (int j = 0; j < Width; j++)
                {
                    Console.Write($"\t{this[j, i].Value} ");
                }
                Console.WriteLine();
            }
            Console.WriteLine();
        }

        public object Clone() =>
            new Model(_pixels.Select(it => it.Clone() as Pixel).ToList());
    }
}