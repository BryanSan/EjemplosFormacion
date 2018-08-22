namespace EjemplosFormacion.AzureFunction
{
    public class Person
    {
        public string PartitionKey { get; set; }
        public string RowKey { get; set; }
        public string Name { get; set; }
        public string MobileNumber { get; set; }
    }
}
