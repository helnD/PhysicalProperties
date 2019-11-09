namespace System.MarkingBehaviour
{
    public class DepthMarking : IMarkingBehaviour
    {
        private int _index = 1;
        
        public Model Marking(Model model)
        {
            _index = 1;

            var markedModel = model.WorkModel().Passing((it, x, y) =>
            {
                if (it[x, y].Value == -1)
                {
                    Find(it, x, y);
                    _index++;
                }
            });

            return markedModel;
        }

        private void Find(Model model, int x, int y)
        {
            model[x, y] = model[x, y].WithNewValue(_index);
            
            if (model[x - 1, y].Value == -1) Find(model, x - 1, y);
            if (model[x + 1, y].Value == -1) Find(model, x + 1, y);
            if (model[x, y - 1].Value == -1) Find(model, x, y - 1);
            if (model[x, y + 1].Value == -1) Find(model, x, y + 1);
        }
    }
}