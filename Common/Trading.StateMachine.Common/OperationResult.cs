﻿using System.Collections.Generic;

namespace Trading.StateMachine.Common
{
    public class OperationResult
    {
        public List<string> Errors { get; set; } = new List<string>();

        public bool IsSuccessfull
        {
            get
            {
                return this.Errors.Count == 0;
            }
        }
    }
}