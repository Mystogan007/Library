namespace Library.Models.CRUDResult
{
    public class CrudResult
    {
        public Status StatusOperation { get; set; }
        public string Error { get; set; }
    }

    public enum Status
    {
        Ok,
        Error
    }

}
