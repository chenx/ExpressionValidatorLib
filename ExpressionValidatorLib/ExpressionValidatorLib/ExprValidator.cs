using System;

namespace ExpressionValidatorLib
{
    /// <summary>
    /// Parse a math expression to see if it's correct.
    /// Algorithm: recursive descent.
    /// 
    /// Grammar for mathematical expressions:
    /// E = T + E | T - E | T
    /// T = F * T | F / T | F
    /// F = (E) | num | var
    /// 
    /// num is of format a or a.b, cannot be a. or .b,
    /// var type1 is of format id,
    /// var type2 is of format id.id,
    /// id is of format [a-zA-Z][a-zA-Z0-9_]+.
    /// 
    /// Author: X. C.
    /// Created on: 3/12/2015
    /// Last modified: 6/2/2015
    /// License: BSD/MIT/Apache
    /// Dev environment: VS.NET 2010
    /// </summary>
    public class ExprValidator
    {        
        private string expr; // keep a copy of input expression string, for display error position.
        private int p;       // position of current character.
        private int len;     // length of input expression string.
        private string msg;  // error message.
        
        public enum VarType { VarType1, VarType2 };
        public VarType varType // Variable type.
        {
            get;
            set;
        }

        private bool TRACE   // Flag to trace function entrance.
        {
            get;
            set;
        }

        /// <summary>
        /// Constructor.
        /// </summary>
        public ExprValidator()
	    {
            TRACE = false;
            //TRACE = true;

            varType = VarType.VarType1;
	    }


        /// <summary>
        /// Returns error message of validation.
        /// </summary>
        /// <returns>Error message</returns>
        public string Message() {
            return msg;
        }
        

        /// <summary>
        /// Entry point of validation.
        /// </summary>
        /// <param name="expr"></param>
        /// <returns>Boolean value on whether validation succeeds.</returns>
        public bool Validate(string expr) {
            this.expr = expr;
            this.p = 0; 
            this.len = expr.Length;
            this.msg = "";

            bool valid;
            try
            {
                valid = E();
                if (p != len) {
                    // add this error only when no other error is found.
                    if (TRACE || msg == "")
                    {
                        msg += "\nexpression does not end properly" + getPos();
                    }
                    return false;
                }
                return valid;
            }
            catch (Exception ex) {
                valid = false;
                msg += "\nexception: " + ex.Message + getPos();
            }

            return valid;
        }


        /// <summary>
        /// Ignore space in input string.
        /// </summary>
        private void ignore_space() {
            while (p < len && expr[p] == ' ') ++p;
        }


        /// <summary>
        /// Validate E.
        /// </summary>
        /// <returns>Boolean value on whether validation succeeds.</returns>
        private bool E()
        {
            if (TRACE) msg += "\nEnter E";

            if (!T()) return false;
            ignore_space();

            //while (p < len && (expr[p] == '+' || expr[p] == '-'))
            if (p < len && (expr[p] == '+' || expr[p] == '-'))
            {
                if (TRACE) msg += "\n" + expr[p];
                ++p;
                ignore_space();
                if (!E()) return false;
            }
            return true;
        }


        /// <summary>
        /// Validate T.
        /// </summary>
        /// <returns>Boolean value on whether validation succeeds.</returns>
        private bool T()
        {
            if (TRACE) msg += "\nEnter T";

            if (!F()) return false;
            ignore_space();

            //while (p < len && (expr[p] == '*' || expr[p] == '/'))
            if (p < len && (expr[p] == '*' || expr[p] == '/'))
            {
                if (TRACE) msg += "\n" + expr[p];
                ++p;
                ignore_space();
                if (!T()) return false;
            }
            return true;
        }


        /// <summary>
        /// Validate F.
        /// </summary>
        /// <returns>Boolean value on whether validation succeeds.</returns>
        private bool F() {
            if (TRACE) msg += "\nEnter F";

            ignore_space();
            if (p == len)
            {
                msg += "\nunexpected end of string" + getPos();
                return false;
            }

            if (expr[p] == '(')
            {
                ++p;
                ignore_space();
                if (!E()) return false;
                ignore_space();

                if (p == len)
                {
                    msg += "\nmissing ')' at end of expression" + getPos();
                    return false;
                }
                else if (expr[p] == ')')
                {
                    ++p;
                    ignore_space();
                    return true;
                }
                else
                {
                    return false;
                }
            }
            else if (expr[p] == '.') {
                msg += "\n'.' is not valid start of number or variable" + getPos();
                return false;
            }
            else if (is_num(expr[p]))
            {
                return num();
            }
            else
            {
                return var();
            }
        }

        /// <summary>
        /// Check if letter c is a digit (0-9).
        /// </summary>
        /// <param name="c"></param>
        /// <returns></returns>
        private bool is_num(char c) {
            return Char.IsDigit(c);
        }

        /// <summary>
        /// Number format must be a or a.b. It cannot be a. or .b.
        /// </summary>
        /// <returns>Boolean value on whether validation succeeds.</returns>
        private bool num() {
            if (TRACE) msg += "\nEnter num";
                
            if (!Char.IsDigit(expr[p]))
            {
                msg += "\nnum does not start with digit" + getPos();
                return false;
            }

            while (p < len && Char.IsDigit(expr[p])) {
                ++p; // there can be not space in between, so cannot use fetch here.
            }

            if (p < len && expr[p] == '.')
            {
                ++p; // there can be not space in between, so cannot use fetch here.

                if (p == len || !Char.IsDigit(expr[p]))
                {
                    msg += "\nnum cannot end with '.'" + getPos();
                    return false;
                }

                while (p < len && Char.IsDigit(expr[p]))
                {
                    ++p;
                }
            }

            return true;
        }


        /// <summary>
        /// Validate var.
        /// Type 1: id
        /// Type 2: id.id
        /// </summary>
        /// <returns>Boolean value on whether validation succeeds.</returns>
        private bool var() {
            if (TRACE) msg += "\nEnter var";

            if (varType == VarType.VarType1) {
                if (!ID()) {
                    msg += "\ninvalid variable" + getPos();
                    return false;
                }
                return true;
            }
            else if (varType == VarType.VarType2)
            {

                if (!ID())
                {
                    msg += "\ninvalid variable (invalid variable prefix)" + getPos();
                    return false;
                }
                if (p == len || expr[p] != '.')
                {
                    msg += "\ninvalid variable (need a dot separate table and column)" + getPos();
                    return false;
                }
                // now p < len, and expr[p] == '.'.
                ++p; 
                if (!ID())
                {
                    msg += "\ninvalid variable (invalid variable suffix)" + getPos();
                    return false;
                }

                return true;
            }
            else {
                msg += "\nunknown variable type" + getPos();
                return false;
            }
        }


        /// <summary>
        /// Validate ID (Identifier).
        /// 
        /// [a-zA-Z][a-zA-Z0-9_]+
        /// Note: do not show error cause for Identifier.
        /// </summary>
        /// <returns>Boolean value on whether validation succeeds.</returns>
        private bool ID() {
            if (TRACE) msg += "\nEnter ID";

            if (p == len) {
                msg += "\ninvalid identifier (empty string)" + getPos();
                return false;
            }
            if (!Char.IsLetter(expr[p])) {
                msg += "\ninvalid identifier (not started with letter)" + getPos();
                return false;
            }
            while (p < len && (Char.IsLetterOrDigit(expr[p]) || expr[p] == '_')) {
                ++p; 
            }

            return true;    
        }


        /// <summary>
        /// Show position where validation error happens.
        /// </summary>
        /// <returns>Error string.</returns>
        private string getPos() {
            string s = " (position: " + p + "/" + len + ")";
            s += "\n" + expr.Substring(0, p) + "{?}" + expr.Substring(p);
            return s;
        }
    }
}
