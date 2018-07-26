Feature: Instrument Price Change
	As an investor
	I want to see instruments changing price
	So that I can track my projected income

Scenario: Manual Price Change
	Given I have instrument with this info:
		| Symbol | Name | Current Price |
		| GLD    | Gold | 2.34          |
	And I have entered lot with this info:
		| Purchase Date | Purchase Price | Notes     | Instrument Symbol | Instrument Name | Instrument Current Price |
		| 12/24/2017    | 1.23           | notes 123 | GLD               | Gold            | 2.34                     |
	When I change GLD instrument price to 3.45
	Then I should have instrument with this info:
		| Symbol | Name | Current Price |
		| GLD    | Gold | 3.45          |
	And I should have lot with this info:
		| Purchase Date | Purchase Price | Notes     | Instrument Symbol | Instrument Name | Instrument Current Price |
		| 12/24/2017    | 1.23           | notes 123 | GLD               | Gold            | 3.45                     |