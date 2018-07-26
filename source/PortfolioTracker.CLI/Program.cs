using CommandLine;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace PortfolioTracker.CLI
{
    class Program
    {
        private static Dictionary<Type, Type> _argumentsHandlers = new Dictionary<Type, Type>
        {
            { typeof(AddLot.AddLotArguments), typeof(AddLot.AddLotArgumentsHandler) },
            { typeof(ChangeInstrumentPrice.ChangeInstrumentPriceArguments), typeof(ChangeInstrumentPrice.ChangeInstrumentPriceArgumentsHandler) },
        };

        static void Main(string[] args)
        {
            var argsToParse = args.Length > 0 ? args : ReadArgsFromConsole();
            ParseArgsAndExecuteCommand(argsToParse);

            string[] ReadArgsFromConsole()
            {
                Console.WriteLine("Running in console mode. Enter command and press enter.");
                var commandLine = Console.ReadLine();

                var result = new List<List<char>>() { new List<char>() /* first param already there */ };
                var openQuotes = new Dictionary<char, bool> { { '\'', false }, { '"', false } };

                var currentIndex = 0;
                while (currentIndex < commandLine.Length)
                {
                    var currentChar = commandLine[currentIndex];

                    if (currentChar == ' ')
                    {
                        if (openQuotes['\''] || openQuotes['"'])
                            result[result.Count - 1].Add(currentChar);
                        else 
                            //end of param
                            result.Add(new List<char>());
                    }
                    else if (currentChar == '\'' || currentChar == '"')
                        openQuotes[currentChar] = !openQuotes[currentChar];
                    else
                        result[result.Count - 1].Add(currentChar);

                    currentIndex++;
                }

                return result.Select(parameter => new string(parameter.ToArray())).ToArray();
            }
        }

        private static void ParseArgsAndExecuteCommand(string[] argsToParse)
        {
            var parseResult = Parser.Default.ParseArguments(argsToParse, _argumentsHandlers.Keys.ToArray());

            InvokeHandlerForParsedArgumetns(parseResult);
        }

        private static void InvokeHandlerForParsedArgumetns(ParserResult<object> parseResult)
        {
            var handleMethod = typeof(Program).GetMethod("Handle", BindingFlags.Static | BindingFlags.NonPublic);
            foreach (var argType in _argumentsHandlers.Keys)
            {
                handleMethod.MakeGenericMethod(argType).Invoke(null, new[] { parseResult });
            }
        }

        private static void Handle<TArgs>(ParserResult<object> parseResult)
        {
            parseResult.WithParsed<TArgs>(argumentsObject =>
            {
                var parsedArgumentsType = typeof(TArgs);

                foreach (var registeredHandler in _argumentsHandlers)
                {
                    if (registeredHandler.Key == parsedArgumentsType)
                    {
                        var argumentsHandler = IoC.StructureMapIoC.Container.GetInstance(registeredHandler.Value) as ArgumentHandling.IArgumentsHandler<TArgs>;
                        argumentsHandler.Handle(argumentsObject);
                    }
                }
            });
        }
    }
}
