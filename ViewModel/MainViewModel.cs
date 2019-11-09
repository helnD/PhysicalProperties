using System;
using System.Collections.Generic;
using System.DirectoryServices.ActiveDirectory;
using System.Linq;
using System.Windows.Media;

namespace ViewModel
{
    public class MainViewModel
    {
        public MainViewModel()
        {
            SourceModel = new int[25][];
            for (int i = 0; i < 25; i++)
            {
                SourceModel[i] = new int[25];
                for (int j = 0; j < SourceModel[i].Length; j++)
                {
                    SourceModel[i][j] = 0;
                }
            }
        }
        
        public int[][] SourceModel { get; set; }
        public List<GraphicalObject> GraphicalObjects { get; set; }
        
        public ObjectRecognizer Recognizer { get; }
            = new ObjectRecognizer();

        public void CalculateProperties()
        {
            GraphicalObjects = Recognizer.FindObjects(ToModel(SourceModel));
        }

        public void ClearSourceModel()
        {
            for (int i = 0; i < 25; i++)
            {
                SourceModel[i] = new int[25];
                for (int j = 0; j < SourceModel[i].Length; j++)
                {
                    SourceModel[i][j] = 0;
                }
            }
        }

        private Model ToModel(int[][] sourceModel)
        {
            List<Pixel> pixels = new List<Pixel>();
            for (int i = 0; i < sourceModel.Length; i++)
            {
                for (int j = 0; j < sourceModel[i].Length; j++)
                {
                    pixels.Add(new Pixel(sourceModel[j][i], j, i));
                }
            }
            
            Model model = new Model(pixels);

            return model;
        }
    }
}