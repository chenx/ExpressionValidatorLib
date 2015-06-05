# ExpressionValidatorLib

About
-----------------------
Math expression validator library in C#. Didn't find one online that fits my need, so I wrote one for use.

A console program for testing purpose is also included (Tester).

This is written in C#, but can be easily converted to Java. The syntax involved are almost identical.

Usage
----------------------
You can use it either as a library (<a href="https://github.com/chenx/ExpressionValidatorLib/blob/master/ExpressionValidatorLib/ExpressionValidatorLib/output/Release/ExpressionValidatorLib.dll">ExpressionValidatorLib.dll</a>), or as a class file (<a href="https://github.com/chenx/ExpressionValidatorLib/blob/master/ExpressionValidatorLib/ExpressionValidatorLib/ExprValidator.cs">ExprValidator.cs</a>).

To use the validator, create an object and call method Validate(). It will return either True or False.

Call method Message() to show validation message (error or trace information) in detail.

Set Trace on to show entrance of functions.

Use varType to set variable type (type1 or type2, see below).

Code example:

<pre>string test = "(1 + 2) * 3";  
ExpressionValidatorLib.ExprValidator psr = new ExprValidator();  
bool isValid = psr.Validate(test);
if (! isValid) {
    System.Console.WriteLine("validation error: " + psr.Message());
}
</pre>

Implementation detail
------------

Parse a math expression to see if it's correct.

Algorithm: recursive descent.

Grammar for mathematical expressions:  
E = T + E | T - E | T  
T = F * T | F / T | F  
F = (E) | num | var  

num is of format a or a.b, cannot be a. or .b;  
var type1 is of format id;  
var type2 is of format id.id, i.e., a prefix and a suffix separated by a dot (.);  
id is of format [a-zA-Z][a-zA-Z0-9_]+.  


License
---------------
BSD/MIT/Apache  

Dev environment
-------------
VS.NET 2010

Author
------------------
Author: X. C.  
Created on: 3/12/2015  
Last modified: 6/2/2015  
