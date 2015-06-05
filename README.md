# ExpressionValidatorLib
Math expression validator library in C#. Cannot find one on line, so I wrote one for use.

Parse a math expression to see if it's correct.

Algorithm: recursive descent.

Grammar for mathematical expressions:  
E = T + E | T - E | T  
T = F * T | F / T | F  
F = (E) | num | var  

num is of format a or a.b, cannot be a. or .b,  
var type1 is of format id,  
var type2 is of format id.id,  
ID is of format [a-zA-Z][a-zA-Z0-9_]+.  

Author: X. C.  
Created on: 3/12/2015  
License: BSD/MIT/Apache  
Dev environment: VS.NET 2010
