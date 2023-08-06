namespace GenericFilters
{
    public class ValueHolder<T>
    {
        public T Value { get; set; }

        public ValueHolder(T value) => Value = value;
    }
}