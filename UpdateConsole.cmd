if not exist %2BpCop.Console\bin\%1\Rules md %2BpCop.Console\bin\%1\Rules
if not exist %2BpCop.Console.Core\bin\%1\net6.0\Rules md %2BpCop.Console.Core\bin\%1\net6.0\Rules
copy %2%3\bin\%1\netstandard2.0\%3*.* %2BpCop.Console\bin\%1\Rules\
copy %2%3\bin\%1\netstandard2.0\%3*.* %2BpCop.Console.Core\bin\%1\net6.0\Rules\