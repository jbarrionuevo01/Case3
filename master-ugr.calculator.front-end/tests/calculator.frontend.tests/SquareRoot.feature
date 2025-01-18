Feature: Square Root
	As Alice (the customer)
	I want to know the square root of a number
    So I can save time

Scenario Outline: Calculate the square root of a number
    Given a number for square root <number>
    When I calculate its square root
    Then the result would be <result>

Examples:
    | number | result |
    | 4      | 2      |
    | 9      | 3      |
    | 16     | 4      |
    | 0      | 0      |
    | -4     | NaN    |