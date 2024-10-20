namespace Analysis.Animal.System.Models
{
    public class ResponseAssistantMessages
    {
        public List<MessageData>? data { get; set; }
    }

    public class MessageData
    {
        public List<Content>? content { get; set; }
    }

    public class Content
    {
        public string? type { get; set; }
        public TextContent? text { get; set; }
    }

    public class TextContent
    {
        public string? value { get; set; }
        public List<string?>? annotations { get; set; }
    }

}