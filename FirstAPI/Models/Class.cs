namespace FirstAPI.Models
{
    public class Partial<T> where T : class, new()
    {
        public Partial() { }

        public Partial(T model)
        {
            foreach (var prop in typeof(T).GetProperties())
            {
                var thisProp = GetType().GetProperty(prop.Name);
                if (thisProp != null)
                {
                    thisProp.SetValue(this, prop.GetValue(model));
                }
            }
        }
    }
}