Feature: Number Attribute
 I want to have a REST API which includes information
 about a number.

 Scenario Outline: Checking several numbers
	When number <number> is checked for multiple attributes
	Then the answer to know whether is prime or not is <prime>
	And the answer to know whether is odd or not is <odd>
	And the result of square root is <sqrt>
Examples:
	| number | prime | odd  | sqrt               |
	| 2      | true  | false| 1.4142135623730951 |
	| 6      | false | false| 2.4494897427831779 |
	| 7      | true  | true | 2.6457513110645907 |
	| 8      | false | false| 2.8284271247461903 |
	| 9      | false | true | 3                  |
	| 10     | false | false| 3.1622776601683795 |