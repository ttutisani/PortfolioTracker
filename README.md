[![Build status](https://ci.appveyor.com/api/projects/status/3w3y1knysveuruuj/branch/master?svg=true)](https://ci.appveyor.com/project/ttutisani/portfoliotracker/branch/master)

# PortfolioTracker
C# models to track investment portfolios

## Classes

Instrument (entity, aggregate root)
	represents a financial instrument by its symbol, name, and current price.

Lot (entity, aggregate root)
	represents a lot - record of financial trade involving Instrument.

Holding (entity)
	represents a portfolio subset, targeting a single instrument, by aggregating lots buying that instrument.

Portfolio (entity, aggregate root)
	represents a single portfolio, aggregating holdings that the portfolio consists of.

PortfolioGroup (entity, aggregate root)
	represents a set of portfolios, for expressing a bigger picture.