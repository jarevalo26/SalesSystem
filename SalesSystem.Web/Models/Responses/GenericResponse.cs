namespace SalesSystem.Web.Models.Responses
{
    public class GenericResponse<TObject>
    {
        public bool State { get; set; }
        public string? Message { get; set; }
        public TObject? Object { get; set; }
        public List<TObject>? ObjectList { get; set; }
    }
}
