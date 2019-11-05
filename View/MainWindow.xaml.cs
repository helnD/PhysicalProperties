using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using ViewModel;
using Color = System.Windows.Media.Color;

namespace View
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            DataContext = new MainViewModel();
        }

        private bool _isDrawing;
        private int _numberOfColumns = 25;
        private int _numberOfRows = 25;
        private List<string> _headers = new List<string>
        {
            "№",
            "Площадь",
            "Периметр 1",
            "Периметр 2",
            "Центр масс 1",
            "Центр масс 2",
            "Коэффициент округлостей"
        };

        private void Draw(Canvas canvas, double x, double y, Brush color)
        {
            double sizeX = (canvas.Width / _numberOfColumns);
            double sizeY = (canvas.Height / _numberOfRows);

            double marginLeft = (int)(x / sizeX) * sizeX;
            double marginTop = (int)(y / sizeY) * sizeY;

            var context = DataContext as MainViewModel;

            Rectangle rect = new Rectangle
            {
                Height = canvas.Height / _numberOfRows,
                Width = canvas.Width / _numberOfColumns,
                Fill = color,
                Margin = new Thickness(marginLeft, marginTop, 0, 0)
            };
            Panel.SetZIndex(rect, 2);
            canvas.Children.Add(rect);

            if (context.SourceModel[(int)(marginLeft / sizeX)][(int)(marginTop / sizeY)] <= 0)
                context.SourceModel[(int)(marginLeft / sizeX)][(int)(marginTop / sizeY)] = 1;
        }

        private void DrawGrid(Canvas canvas)
        {
            double sizeX = (canvas.Width / _numberOfColumns);
            double sizeY = (canvas.Height / _numberOfRows);

            SolidColorBrush gridBrush = new SolidColorBrush(Color.FromArgb(100, 33, 20, 56));

            for (int i = 1; i < _numberOfColumns; i++)
            {
                Line line = new Line
                {
                    X1 = sizeX * i,
                    Y1 = 0,
                    X2 = sizeY * i,
                    Y2 = canvas.Height + 0,
                    Stroke = gridBrush,
                    StrokeThickness = 0.5,
                    StrokeDashArray = { 6, 3 }
                };
                Panel.SetZIndex(line, 10);
                canvas.Children.Add(line);
            }
            for (int i = 1; i < _numberOfRows; i++)
            {
                Line line = new Line
                {
                    Y1 = sizeX * i,
                    X1 = 0,
                    Y2 = sizeX * i,
                    X2 = canvas.Width + 0,
                    Stroke = gridBrush,
                    StrokeThickness = 0.5,
                    StrokeDashArray = { 6, 3 }
                };
                Panel.SetZIndex(line, 10);
                canvas.Children.Add(line);
            }
        }

        private void DrawGridCoordinate(Canvas canvas)
        {
            double sizeX = (canvas.Width / _numberOfColumns);
            double sizeY = (canvas.Height / _numberOfRows);

            for (int i = 0; i < _numberOfRows; i++)
            {
                canvas.Children.Add(new Label
                {
                    Content = i,
                    Foreground = Brushes.Gray,
                    FontSize = 10,
                    Width = sizeX,
                    Height = sizeY,
                    Margin = new Thickness(sizeX * (-1) - 6, i * sizeY, 0, 0),
                    Padding = new Thickness(0, 0, 0, 0),
                    Tag = "coordinate",
                    HorizontalContentAlignment = HorizontalAlignment.Right
                });
            }
            for (int i = 0; i < _numberOfColumns; i++)
            {
                canvas.Children.Add(new Label
                {
                    Content = i,
                    Foreground = Brushes.Gray,
                    FontSize = 10,
                    Width = sizeX,
                    Height = sizeY,
                    Margin = new Thickness(i * sizeX, sizeY * (-1), 0, 0),
                    Padding = new Thickness(0, 0, 0, 0),
                    Tag = "coordinate",
                    HorizontalContentAlignment = HorizontalAlignment.Center
                });
            }
        }

        private void DrawCanvas_OnPreviewMouseDown(object sender, MouseButtonEventArgs e)
        {
            double cursorX = e.GetPosition(sender as Canvas).X;
            double cursorY = e.GetPosition(sender as Canvas).Y;

            Mouse.Capture(sender as Canvas);
            _isDrawing = true;

            if (cursorX < (sender as Canvas).Width && 
                cursorY < (sender as Canvas).Height &&
                cursorX > 0 && cursorY > 0)
            {
                Draw(sender as Canvas, cursorX, cursorY, Brushes.Black);
            }
        }

        private void DrawCanvas_OnPreviewMouseMove(object sender, MouseEventArgs e)
        {
            double cursorX = e.GetPosition(sender as Canvas).X;
            double cursorY = e.GetPosition(sender as Canvas).Y;

            if (!_isDrawing) return;

            if (cursorX < (sender as Canvas).Width && 
                cursorY < (sender as Canvas).Height &&
                cursorX > 0 && cursorY > 0)
            {
                Draw(sender as Canvas, cursorX, cursorY, Brushes.Black);
            }
        }

        private void DrawCanvas_OnPreviewMouseUp(object sender, MouseButtonEventArgs e)
        {
            double cursorX = e.GetPosition(sender as Canvas).X;
            double cursorY = e.GetPosition(sender as Canvas).Y;

            if (cursorX < (sender as Canvas).Width && 
                cursorY < (sender as Canvas).Height &&
                cursorX > 0 && cursorY > 0)
            {
                Draw((sender as Canvas), cursorX, cursorY, Brushes.Black);
            }

            _isDrawing = false;
            Mouse.Capture(null);
        }

        private void ClearCanvasButton_OnPreviewMouseDown(object sender, MouseButtonEventArgs e)
        {
            var context = DataContext as MainViewModel;
            ClearCanvas(DrawCanvas);
            DataTable.RowGroups.Clear();
            context.ClearSourceModel();
        }

        private void DrawCanvas_OnInitialized(object sender, EventArgs e)
        {
            DrawGrid(sender as Canvas);
            DrawGridCoordinate(sender as Canvas);
        }

        private void Border_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            this.DragMove();
        }

        private void Button_PreviewMouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            this.Close();
        }

        private void CalculateButton_PreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            DataTable.RowGroups.Clear();

            var context = DataContext as MainViewModel;
            context.CalculateProperties();
            
            PrintHeaders();
            FillTable(context);
            ClearCanvas(DrawCanvas);
            DrawMulticoloredPixels(context);
        }

        private void DrawMulticoloredPixels(MainViewModel context)
        {
            double sizeX = (DrawCanvas.Width / _numberOfColumns);
            double sizeY = (DrawCanvas.Height / _numberOfRows);
            
            for (int i = 0; i < context.GraphicalObjects.Count; i++)
            {
                GraphicalObject obj = context.GraphicalObjects[i];
                for (int j = 0; j < obj.ObjectModel.Width; j++)
                {
                    for (int k = 0; k < obj.ObjectModel.Height; k++)
                    {
                        if (obj.ObjectModel[j, k].Value > 0)
                        {
                            Draw(
                                DrawCanvas, 
                                (obj.Position.X + j) * sizeX - 1,
                                (obj.Position.Y + k) * sizeY - 1,
                                new LinearGradientBrush(
                                    Color.FromRgb(
                                    (byte) obj.ObjectColor.Red,
                                    (byte) obj.ObjectColor.Green,
                                    (byte) obj.ObjectColor.Blue), 
                                    Colors.Black, 
                                    45.0));
                        }
                    }
                }

                DrawLabels(context, sizeX, sizeY);
            }
        }

        private void DrawLabels(MainViewModel context, double sizeX, double sizeY)
        {
            for (int i = 0; i < context.GraphicalObjects.Count; i++)
            {
                bool haveDrawed = false;
                int x = 0;
                while (!haveDrawed)
                {
                    if (context.GraphicalObjects[i].ObjectModel[x, 0].Value > 0)
                    {
                        double marginLeft = ((int)((context.GraphicalObjects[i].Position.X - 1) * sizeX) / sizeX) * sizeX + x * sizeX;
                        double marginTop = ((int)((context.GraphicalObjects[i].Position.Y - 1.3) * sizeY) / sizeY) * sizeY;

                        Label label = new Label
                        {
                            Content = i + 1,
                            Foreground = Brushes.White,
                            FontSize = 10,
                            FontWeight = FontWeights.Normal,
                            Margin = new Thickness(marginLeft, marginTop,0, 0),
                            Padding = new Thickness(2, 5, 0, 0)
                        };
                        Panel.SetZIndex(label, 20);
                        DrawCanvas.Children.Add(label);

                        haveDrawed = true;
                    }
                    else
                    {
                        x++;
                    }
                }
            }
        }

        private void PrintHeaders()
        {
            TableRow trHeaders = new TableRow();

            foreach (var header in _headers)
            {
                trHeaders.Cells.Add(new TableCell(new Paragraph(new Run(header))));
                SetTableCellStyle(trHeaders, FontStyles.Italic);
            }
            trHeaders.Cells.Last().BorderThickness = new Thickness(0, 0, 0, 1);
            TableRowGroup headers = new TableRowGroup();
            headers.Rows.Add(trHeaders);
            DataTable.RowGroups.Add(headers);
        }

        private void SetTableCellStyle(TableRow tableRow, FontStyle fontStyle)
        {
            foreach (var cell in tableRow.Cells)
            {
                cell.TextAlignment = TextAlignment.Center;
                cell.FontStyle = fontStyle;
                cell.BorderBrush = Brushes.White;
                cell.BorderThickness = new Thickness(0, 0, 1, 1);
                cell.Padding = new Thickness(8, 0, 8, 8);
            }
        }

        private void FillTable(MainViewModel context)
        {
            for (int i = 0; i < context.GraphicalObjects.Count; i++)
            {
                TableRow tr = new TableRow();

                AddCellToRow(tr, i, (i + 1).ToString());
                AddCellToRow(tr, i, context.GraphicalObjects[i].Properties.Area.ToString());
                AddCellToRow(tr, i, context.GraphicalObjects[i].Properties.Perimeter.ToString());
                AddCellToRow(tr, i, context.GraphicalObjects[i].Properties.Perimeter2.ToString());

                Coordinate restoredCoordinate1 =
                    RestoreCoordinate(
                        DrawCanvas, 
                        context.GraphicalObjects[i].Properties.MassCenter, 
                        context.GraphicalObjects[i].Position);
                AddCellToRow(tr, i, 
                    $"({restoredCoordinate1.X}, {restoredCoordinate1.Y})");
                
                Coordinate restoredCoordinate2 =
                    RestoreCoordinate(
                        DrawCanvas, 
                        context.GraphicalObjects[i].Properties.MassCenter2, 
                        context.GraphicalObjects[i].Position);
                AddCellToRow(tr, i,
                    $"({restoredCoordinate2.X}, {restoredCoordinate2.Y})");
                
                AddCellToRow(tr, i, context.GraphicalObjects[i].Properties.CircumferenceRatio.ToString());
                
                SetTableCellStyle(tr, FontStyles.Normal);
                
                TableRowGroup trg = new TableRowGroup();
                trg.Rows.Add(tr);
                DataTable.RowGroups.Add(trg);
            }

            void AddCellToRow(TableRow tr, int objectNumber, string text)
            {
                tr.Cells.Add(new TableCell(new Paragraph(new Run(text))));
                if (objectNumber % 2 == 0)
                    tr.Cells.Last().Background = new SolidColorBrush(Color.FromArgb(80,193,70,70));
            }
        }

        private Coordinate RestoreCoordinate(Canvas canvas, Coordinate relCoordinate, Coordinate position)
        {
            double sizeX = (canvas.Width / _numberOfColumns);
            double sizeY = (canvas.Height / _numberOfRows);

            int x = (int) ((((position.X + relCoordinate.X - 1) * sizeX) / sizeX));
            int y = (int) ((((position.Y + relCoordinate.Y - 1) * sizeY) / sizeY));
            
            return new Coordinate(x, y);
        }

        private void ClearCanvas(Canvas canvas)
        {
            for (int i = canvas.Children.Count - 1; i >= 0; i--)
            {
                if (canvas.Children[i] is Rectangle ||
                    (canvas.Children[i] is Label && (canvas.Children[i] as Label).Tag != "coordinate"))
                {
                    canvas.Children.RemoveAt(i);
                }
            }
        }
    }
}
