Feature: AddLot
	In order to add Lot to the system
	As a Portfolio Tracker user
	I want to have an ability to enter and capture new Lot info

Scenario: Add Lot and Instrument
	Given there are no Lots yet in the system
	And there are no Instruments yet in the system
	When I add Lot with this info: symbol ABC, purchase date 01/02/2003, purchase price $111.22, notes "some of my notes"
	Then a new Lot must appear in the system with this info: symbol ABC, purchase date 01/02/2003, purchase price $111.22, notes "some of my notes", Instrument price $111.22
	And a new Instrument must appear in the system with this info: symbol ABC, name "ABC", current price $111.22

Scenario: Add Lot for existing Instrument
	Given there are no Lots yet in the system
	And there is an existing Instrument with this info: symbol ABC, name "ABC", current price $111.22
	When I add Lot with this info: symbol ABC, purchase date 01/02/2003, purchase price $222.33, notes "some of my notes"
	# existing instrument data is not yet in the new lot (only eventually will get there)
	Then a new Lot must appear in the system with this info: symbol ABC, purchase date 01/02/2003, purchase price $222.33, notes "some of my notes", Instrument price $222.33
	And an existing Instrument should stay with this info: symbol ABC, name "ABC", current price $111.22
