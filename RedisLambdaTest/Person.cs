namespace RedisLambdaTest
{
    internal class Person
    {
        public long Id { get; set; }
        public string Name { get; set; }
        public override string ToString()
        {
            return $"{Id} - {Name}";
        }
    }
}