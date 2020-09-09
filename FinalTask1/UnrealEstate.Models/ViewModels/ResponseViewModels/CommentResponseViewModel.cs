using System;

namespace UnrealEstate.Models.ViewModels
{
    public class CommentResponseViewModel
    {
        public string Text { get; set; }

        public DateTimeOffset CreatedAt { get; set; }

        public string UserEmail { get; set; }
    }
}
