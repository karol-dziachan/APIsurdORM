﻿using __ProjectName__.Application.Common.Abstractions;

namespace __ProjectName__.Application.Features.__Entity__.Commands.Remove__Entities__ByParameters
{
    public sealed class CommandResult : BaseResult
    {
        public int AffectedRows { get; set; }

        public CommandResult(bool success, string message, int affectedRows) : base(success, message)
        {
            AffectedRows = affectedRows;
        }
    }
}
