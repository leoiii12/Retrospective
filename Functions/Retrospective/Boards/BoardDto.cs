using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Retrospective.Boards
{
    public class BoardDto : IValidatableObject
    {
        private string _boardId;

        public string BoardId
        {
            get => _boardId;
            set => _boardId = value.Trim().Replace(" ", "-");
        }

        public string Password { get; set; }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            var validationResults = new List<ValidationResult>();

            if (BoardId.Length > 20) validationResults.Add(new ValidationResult($"BoardId length should be shorter than 20."));

            return validationResults;
        }
    }
}