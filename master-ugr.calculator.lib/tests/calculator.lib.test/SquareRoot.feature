Feature: Square Root Calculation

Scenario Outline: Calculate the square root of a number
    Given a number for square root <number>
    When I calculate its square root
    Then the result would be <result>

Examples:
    | number | result |
    | 4      | 2.0    |
    | 9      | 3.0    |
    | 16     | 4.0    |
    | 0      | 0.0    |
    | -4     | NaN  |
