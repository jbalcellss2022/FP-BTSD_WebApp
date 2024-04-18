namespace DataAccessLayer.Classes
{
    public class UserDDDTO
    {
        public Guid? userId { get; set; }
        public dynamic? ddClient { get; set; }
        public dynamic? ddModel { get; set; }
        public dynamic? ddBrand { get; set; }
        public dynamic? ddBrandName { get; set; }
        public dynamic? ddOs { get; set; }
        public dynamic? ddBrowser { get; set; }
        public dynamic? ddtype { get; set; }
        public string? ipAddress { get; set; }
        public string? ddCity { get; set; }
    }
}
