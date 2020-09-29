using System;
using System.ComponentModel;

namespace UnrealEstate.Business.Comment.ViewModel.Response
{
    public class CommentResponse
    {
        public int Id { get; set; }
        public string Text { get; set; }

        [DisplayName("Created At")] public DateTimeOffset CreatedAt { get; set; }

        [DisplayName("Author Email")] public string UserEmail { get; set; }
    }
}