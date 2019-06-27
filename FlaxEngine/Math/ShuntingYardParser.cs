// Copyright (c) 2012-2019 Wojciech Figat. All rights reserved.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FlaxEngine
{
    /// <summary>
    /// Types of possible tokens used in Shunting-Yard parser.
    /// </summary>
    enum TokenType { Number, Parenthesis, Operator, WhiteSpace };

    /// <summary>
    /// Token representation containing its type and value.
    /// </summary>
    struct Token
    {
        public TokenType Type { get; }
        public string Value { get; }

        public Token(TokenType type, string value)
        {
            Type = type;
            Value = value;
        }
    }

    /// <summary>
    /// Represents simple mathematical operation.
    /// </summary>
    class Operator
    {
        public string Name { get; set; }
        public int Precedence { get; set; }
        public bool RightAssociative { get; set; }
    }

    static class Parser
    {
        /// <summary>
        /// Describe all operators available for parsing.
        /// </summary>
        private static IDictionary<string, Operator> operators = new Dictionary<string, Operator>
        {
            ["+"] = new Operator { Name = "+", Precedence = 1 },
            ["-"] = new Operator { Name = "-", Precedence = 1 },
            ["*"] = new Operator { Name = "*", Precedence = 2 },
            ["/"] = new Operator { Name = "/", Precedence = 2 },
            ["^"] = new Operator { Name = "^", Precedence = 3, RightAssociative = true }
        };

        /// <summary>
        /// Compare operators based on precedence: ^ >> * / >> + -
        /// </summary>
        private static bool CompareOperators(string oper1, string oper2) 
        {
            var op1 = operators[oper1];
            var op2 = operators[oper2];
            return op1.RightAssociative ? op1.Precedence < op2.Precedence : op1.Precedence <= op2.Precedence;
        }

        /// <summary>
        /// Assign a single character to its TokenType.
        /// </summary>
        private static TokenType DetermineType(char ch)
        {
            if (char.IsDigit(ch) || ch == '.')
                return TokenType.Number;

            if (char.IsWhiteSpace(ch))
                return TokenType.WhiteSpace;

            if (ch == '(' || ch == ')')
                return TokenType.Parenthesis;

            if (operators.ContainsKey(Convert.ToString(ch)))
                return TokenType.Operator;

            throw new Exception("Wrong character");
        }

        /// <summary>
        /// First parsing step - tokenization of a string.
        /// </summary>
        public static IEnumerable<Token> Tokenize(string text)
        {
            // Nessesary to correctly parse negative numbers
            var previous = TokenType.WhiteSpace;

            for (int i = 0; i < text.Length; i++)
            {   
                var token = new StringBuilder();
                var type = DetermineType(text[i]);
                
                if (type == TokenType.WhiteSpace)
                    continue;

                token.Append(text[i]);

                // Handle fractions and negative numbers (dot . is considered a figure)
                if (type == TokenType.Number || text[i] == '-' && previous != TokenType.Number)
                {
                    // Continue till the end of the number
                    while (i + 1 < text.Length && DetermineType(text[i + 1]) == TokenType.Number)
                    {
                        i++;
                        token.Append(text[i]);
                    }

                    // Discard solo '-'
                    if (token.Length != 1)
                        type = TokenType.Number;
                }

                previous = type;
                yield return new Token(type, token.ToString());
            }
        }

        /// <summary>
        /// Second parsing step - order tokens in reverse polish notation.
        /// </summary>
        public static IEnumerable<Token> ShuntingYard(IEnumerable<Token> tokens)
        {
            var stack = new Stack<Token>();

            foreach (var tok in tokens)
            {
                switch (tok.Type)
                {
                    // Number tokens go directly to output
                    case TokenType.Number:
                        yield return tok;
                        break;

                    // Operators go on stack, unless last operator on stack has higher presedence
                    case TokenType.Operator:
                        while (stack.Any() && stack.Peek().Type == TokenType.Operator && 
                               CompareOperators(tok.Value, stack.Peek().Value))
                        {
                            yield return stack.Pop();
                        }

                        stack.Push(tok);
                        break;

                    // Left parentasis goes on stack
                    // Right parentasis takes all symbols (...) to output
                    case TokenType.Parenthesis:
                        if (tok.Value == "(")
                            stack.Push(tok);
                        else
                        {
                            while (stack.Peek().Value != "(")
                                yield return stack.Pop();
                            stack.Pop();
                        }
                        break;

                    default:
                        throw new Exception("Wrong token");
                }
            }

            // Pop all remaining operators from stack
            while (stack.Any())
            {
                var tok = stack.Pop();

                // Parenthesis still on stack mean a mismatch
                if (tok.Type == TokenType.Parenthesis)
                    throw new Exception("Mismatched parentheses");

                yield return tok;
            }
        }

        /// <summary>
        /// Third parsing step - evaluate reverse polish notation into single float.
        /// </summary>
        public static float EvaluateRPN(IEnumerable<Token> tokens)
        {
            var stack = new Stack<float>();

            foreach (var token in tokens) 
            {
                if (token.Type == TokenType.Number)
                {
                    stack.Push(float.Parse(token.Value));
                } 
                else
                {
                    // In this step there always should be 2 values on stack
                    if (stack.Count < 2)
                        throw new Exception("Evaluation error");

                    var f1 = (float)stack.Pop();
                    var f2 = (float)stack.Pop();

                    switch (token.Value)
                    {
                        case "+":
                            f2 += f1;
                            break;
                        case "-":
                            f2 -= f1;
                            break;
                        case "*":
                            f2 *= f1;
                            break;
                        case "/":
                            if (f1 == 0)
                                throw new Exception("Division by 0");
                            f2 /= f1;
                            break;
                        case "^":
                            f2 = (float)Math.Pow(f2, f1);
                            break;
                    }

                    stack.Push(f2);
                }
            }
            
            return stack.Pop();
        }
    }
}
