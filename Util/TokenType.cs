﻿namespace Calculator.Util;

public enum TokenType
{
    None,
    End,
    Empty,

    Base,
    Comma,

    Symbol,
    Number,

    Operator,
    Pipe,
    OpenParen,
    CloseParen,
}