namespace ScraperLinkedInServer.Models.Response
{
    public class ExportFileResponse : BaseResponse
    {
        public string Content { get; set; }
        public string ContentType { get; set; }
        public int ContentEntriesCount { get; set; }
        public string DateCreateUTC { get; set; }
    }
}