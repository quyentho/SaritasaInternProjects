using System;

namespace UnrealEstate.Models.ViewModels.ResponseViewModels
{
    public class CommentResponse
    {
        public int Id { get; set; }
        public string Text { get; set; }

        public DateTimeOffset CreatedAt { get; set; }

        public string UserEmail { get; set; }
    }
}
