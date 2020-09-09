using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UnrealEstate.Models.ViewModels
{
    public class CommentResponseViewModel
    {
        public string Text { get; set; }

        public DateTimeOffset CreatedAt { get; set; }

        public string UserEmail { get; set; }
    }
}
