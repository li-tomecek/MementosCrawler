using System.Collections;
using System.Collections.Generic;

using System;

public class CustomException : ApplicationException
{
    public CustomException(string message) : base(message) { }
}
